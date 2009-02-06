(*
    Building a (very simple) syntax higligher with Pencil.Unit
*)
#light

open System
open System.Text
open System.IO
open Pencil.Unit

type Token =
    | BeginComment of string
    | EndComment
    | Keyword of string
    | Preprocessor of string
    | BeginString of string
    | EndString
    | Text of string
    | WhiteSpace of string
    | NewLine
    | Operator of string

let Classify x =
    match x with
    | "abstract" | "and" | "as" | "assert"
    | "base" | "begin"
    | "class"
    | "default" | "delegate" | "do" | "done" | "downcast" | "downto"
    | "elif" | "else" | "end" | "exception" | "extern"
    | "false" | "finally" | "for" | "fun" | "function"
    | "if" | "in" | "inherit" | "inline" | "interface" | "internal"
    | "lazy" | "let"
    | "match" | "member" | "module" | "mutable"
    | "namespace" | "new" | "null"
    | "of" | "open" | "or" | "override"
    | "private" | "public"
    | "rec" | "return"
    | "static" | "struct"
    | "then" | "to" | "true" | "try" | "type"
    | "upcast" | "use"
    | "val" | "void"
    | "when" | "while" | "with"
    | "yield" -> Keyword x
    | _ when x.[0] = '#' -> Preprocessor x
    | _ -> Text x

let IsKeyword = function
    | Keyword _ -> true
    | _ -> false

let IsPreprocessor = function
    | Preprocessor _ -> true
    | _ -> false

Theory "Classify should support all F# keywords"

    ("abstract and as assert base begin class default delegate do done
    downcast downto elif else end exception extern false finally for
    fun function if in inherit inline interface internal lazy let
    match member module mutable namespace new null of open or
    override private public rec return static struct then to
    true try type upcast use val void when while with yield"
    .Split([|' ';'\t';'\r';'\n'|], StringSplitOptions.RemoveEmptyEntries))

    (fun x -> Classify x |> IsKeyword |> Should Be true)

Fact "Classify should treat leading # as Preprocessor"
    (Classify "#light" |> IsPreprocessor |> Should Be true)

let Tokenize (s:string) =
    let start = ref 0
    let p = ref 0
    let next() = p := !p + 1
    and hasMore() = !p < s.Length
    and sub() = s.Substring(!start, !p - !start)
    and current() = s.[!p]
    and prev() = s.[!p - 1]
    let peek() = if (!p + 1) < s.Length then
                    s.[!p + 1]
                 else
                    (char)0
    let start() =
         start := !p
         current()
    and isWhite() =
        match current() with
        | ' ' | '\t' -> true
        | _ -> false
    and isOperator() = "_(){}<>,.=|-+:;[]".Contains(string (current()))
    and isNewLine() = current() = '\r' || current() = '\n'
    let notNewline() = not (isNewLine())
    and notBlockEnd() = not(current() = ')' && prev() = '*')
    let inWord() = not (isWhite() || isNewLine() || isOperator())
    let read p eatLast =
        while hasMore() && p() do
            next()
        if eatLast then
            next()
        sub()
    let splitLines (s:string) = s.Split([|'\r';'\n'|], StringSplitOptions.RemoveEmptyEntries)
    let readWhite() = WhiteSpace(read isWhite false)
    and readNewLine() =
        next()
        if isNewLine() then
            next()
        NewLine
    and readWord() = Classify(read inWord false)
    and readOperator() = Operator(read isOperator false)
    let nextToken() =
        match start() with
        | '\"' ->
            seq{
                next()
                yield BeginString(sub())
                start() |> ignore
                let isEscaped() = prev() = '\\'
                let inString() = isEscaped() || current() <> '\"'
                let lines = read inString true |> splitLines
                yield Text lines.[0]
                for i = 1 to lines.Length - 1 do
                    yield NewLine
                    yield Text lines.[i]
                yield EndString } |> Seq.to_list

        | '/' when peek() = '/' ->
            seq {
                yield BeginComment(sub())
                yield Text(read notNewline false)
                yield EndComment} |> Seq.to_list
        | '(' when peek() = '*' ->
            seq {
                yield BeginComment(sub())
                let lines = read notBlockEnd true |> splitLines
                yield Text lines.[0]
                for i = 1 to lines.Length - 1 do
                    yield NewLine
                    yield Text lines.[i]
                yield EndComment} |> Seq.to_list
        | _ when isWhite() -> [readWhite()]
        | _ when isOperator() -> [readOperator()]
        | _ when isNewLine() -> [readNewLine()]
        | _ -> [readWord()]
    seq { while hasMore() do yield! nextToken()}

let ToString x =
    let encode = function
        | BeginComment _ -> "c"
        | EndComment -> "/c"
        | Keyword _ -> "k"
        | Preprocessor _ -> "p"
        | BeginString _ -> "s"
        | EndString -> "/s"
        | Text _ -> "t"
        | WhiteSpace _ -> "w"
        | NewLine -> "n"
        | Operator _ -> "o"
    x |> Seq.fold (fun (r:StringBuilder) -> encode >> r.Append) (StringBuilder())
    |> string

Fact "Tokenize should categorize"(
    Tokenize "#light let foo" |> ToString |> Should Be "pwkwt")

Fact "Tokenize should handle string"(
    Tokenize "\"Hello World\"" |> ToString |> Should Be "s/s")

Fact "Tokenize should split string into lines"(
    Tokenize "\"Hello\r\nWorld\"" |> ToString |> Should Be "sn/s")

Fact "Tokenize should split comment into lines"(
    Tokenize "(*Hello\r\nWorld*)" |> ToString |> Should Be "ctnt/c")

Theory "Tokenize should separate start on operators"
    ("_ ( ) { } < > [ ] , = | - + : ; .".Split([|' '|]))
    (fun x -> Tokenize x |> ToString |> Should Be "o")

Fact "Tokenize should end on separators"(
    Tokenize "foo)" |> ToString |> Should Be "to")

Fact "Tokenize should handle escaped char in string"(
    Tokenize "\"\\\"\"" |>  ToString |> Should Be "s")

Fact "Tokenize should handle //line comment"(
    Tokenize "//line comment" |> ToString |> Should Be "c")

Fact "Tokenize should handle (* block comments *)"(
    Tokenize "(* block comment )*) " |> ToString |> Should Be "cw")

Fact "Tokenize should handle newline"(
    Tokenize "\r\n" |> ToString |> Should Be "n")

Fact "Tokenize should separate whitespace and newline"(
    Tokenize "    \r\n" |> ToString |> Should Be "wn")

let Sanitize (s:string) = s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(" ", "&nbsp;")

Fact "Sanitize should replace < with &lt;"(
    Sanitize "<" |> Should Be "&lt;")

Fact "Sanitize should repalce & with &amp;"(
    Sanitize "&" |> Should Be "&amp;")

Fact "Sanitize should repalce ' ' with &nbsp;"(
    Sanitize " " |> Should Be "&nbsp;")

type IHtmlWriter =
    abstract Literal : string -> unit
    abstract BeginSpan : string -> unit
    abstract EndSpan : unit -> unit
    abstract Span : string -> string -> unit
    abstract NewLine : unit -> unit

let HtmlEncode (w:IHtmlWriter) =
    function
    | Keyword x -> w.Span "kw" x
    | Preprocessor x -> w.Span "pp" x
    | BeginComment x ->
        w.BeginSpan "c"
        w.Literal x
    | EndComment | EndString -> w.EndSpan()
    | BeginString x ->
        w.BeginSpan "tx"
        w.Literal x
    | Operator x -> w.Span "op" x
    | Text x | WhiteSpace x -> w.Literal x
    | NewLine -> w.NewLine()

let AsHtml s =
    let r = StringBuilder("<div class='f-sharp'>")
    let encode = HtmlEncode {new IHtmlWriter with
        member this.Literal s = r.Append(Sanitize s) |> ignore
        member this.BeginSpan c = r.AppendFormat("<span class='{0}'>", c) |> ignore
        member this.EndSpan() = r.Append("</span>") |> ignore
        member this.Span c s = r.AppendFormat("<span class='{0}'>{1}</span>", c, Sanitize s) |> ignore
        member this.NewLine() = r.Append("<br>") |> ignore}
    Tokenize s |> Seq.iter encode
    string (r.Append("</div>"))

Fact "AsHtml sample"(
    let sample = "#light\r\nlet numbers = [1..10]"
    let expected =
        String.Concat [|"<div class='f-sharp'><span class='pp'>#light</span><br>"
        ;"<span class='kw'>let</span>&nbsp;numbers&nbsp;<span class='op'>=</span>&nbsp;"
        ;"<span class='op'>[</span>1<span class='op'>..</span>"
        ;"10<span class='op'>]</span></div>"|]
    sample |> AsHtml |> Should Be expected)

//Render
let input = __SOURCE_FILE__
let template = File.ReadAllText("Template.html")
File.ReadAllText(input) |> AsHtml
|> (fun x -> File.WriteAllText(input + ".html", template.Replace("$code", x)))
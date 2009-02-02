(* Building a (very simple) syntax higligher with Pencil.Unit *)
#light

open System
open System.Text
open System.Collections.Generic
open System.IO
open Pencil.Unit

type Token =
    | Comment of string
    | Keyword of string
    | Preprocessor of string
    | String of string
    | Text of string
    | WhiteSpace of string
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

    (fun x -> Classify x |> IsKeyword |> Should Equal true)

Fact "Classify should treat leading # as Preprocessor"
    (Classify "#light" |> IsPreprocessor |> Should Equal true)

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
    and isWhite() = Char.IsWhiteSpace(current())
    and isOperator() = "_(){}<>,.=|-+:;[]".Contains(string (current()))
    and notNewline() = current() <> '\r'
    and notBlockEnd() = not(current() = ')' && prev() = '*')
    let inWord() = not (isWhite() || isOperator())
    let read p eatLast =
        while hasMore() && p() do
            next()
        if eatLast then
            next()
        sub()
    let readWhite() = WhiteSpace(read isWhite false)
    and readWord() = Classify(read inWord false)
    and readOperator() = Operator(read isOperator false)
    and readString() =
        next()
        while hasMore() && (prev() = '\\' || current() <> '\"') do
            next()
        next()
        String(sub())
    seq {
        while hasMore() do
            start := !p
            let token =
                match current() with
                | '\"' -> readString()
                | '/' when peek() = '/' -> Comment(read notNewline false)
                | '(' when peek() = '*' -> Comment(read notBlockEnd true)
                | _ when isWhite() -> readWhite()
                | _ when isOperator() -> readOperator()
                | _ -> readWord()
            yield token}

let ToString x =
    let encode = function
        | Comment _ -> "c"
        | Keyword _ -> "k"
        | Preprocessor _ -> "p"
        | String _ -> "s"
        | Text _ -> "t"
        | WhiteSpace _ -> "w"
        | Operator _ -> "o"
    x |> Seq.fold (fun (r:StringBuilder) x -> r.Append(encode x)) (StringBuilder())
    |> string

Fact "Tokenize should categorize"(
    Tokenize "#light let foo" |> ToString |> Should Equal "pwkwt")

Fact "Tokenize should handle string"(
    Tokenize "\"Hello World\"" |> ToString |> Should Equal "s")

Theory "Tokenize should separate start on operators"
    ("_ ( ) { } < > [ ] , = | - + : ; .".Split([|' '|]))
    (fun x -> Tokenize x |> ToString |> Should Equal "o")

Fact "Tokenize should end on separators"(
    Tokenize "foo)" |> ToString |> Should Equal "to")

Fact "Tokenize should handle escaped char in string"(
    Tokenize "\"\\\"\"" |>  ToString |> Should Equal "s")
    
Fact "Tokenize should handle //line comment"(
    Tokenize "//line comment" |> ToString |> Should Equal "c")

Fact "Tokenize should handle (* block comments *)"(
    Tokenize "(* block comment )*) " |> ToString |> Should Equal "cw")

let Sanitize (s:string) = s.Replace("&", "&amp;").Replace("<", "&lt;")

Fact "Sanitize should replace < with &lt;"(
    Sanitize "<" |> Should Equal "&lt;")

Fact "Sanitize should repalce & with &amp;"(
    Sanitize "&" |> Should Equal "&amp;")

let HtmlEncode =
    let span style s =
        String.Format("<span class='{0}'>{1}</span>",style, Sanitize s)
    function
    | Comment x -> span "c" x
    | Keyword x -> span "kw" x
    | Preprocessor x -> span "pp" x
    | String x -> span "tx" x
    | Operator x -> span "op" x
    | Text x -> Sanitize x
    | WhiteSpace x ->
        let encodeWhite = function
            | ' ' -> "&nbsp;"
            | x -> string x
        x |> Seq.fold (fun a b -> a + (encodeWhite b)) ""

Fact "HtmlEncode should encode ' ' as &nbsp;"(
    HtmlEncode (WhiteSpace " ") |> Should Equal "&nbsp;")    
    
let AsHtml s =
    let r = StringBuilder("<pre class='f-sharp'>")
    Tokenize s |> Seq.iter (fun x -> r.Append(HtmlEncode x) |> ignore)
    string (r.Append("</pre>"))

Fact "AsHtml sample"(
    let sample = "#light\r\nlet numbers = [1..10]"
    let expected =
        String.Concat [|"<pre class='f-sharp'><span class='pp'>#light</span>\r\n"
        ;"<span class='kw'>let</span>&nbsp;numbers&nbsp;<span class='op'>=</span>&nbsp;"
        ;"<span class='op'>[</span>1<span class='op'>..</span>"
        ;"10<span class='op'>]</span></pre>"|]
    sample |> AsHtml |> Should Equal expected)

//Render myself.
File.ReadAllText(__SOURCE_FILE__)
|> AsHtml |> (fun x -> File.WriteAllText(__SOURCE_FILE__ + ".html", x))

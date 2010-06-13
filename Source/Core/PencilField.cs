using System.Reflection;

namespace Pencil.Core
{
    class PencilField : IField
    {
        readonly ITypeLoader typeLoader;
        readonly FieldInfo field;

        public PencilField(ITypeLoader typeLoader, FieldInfo field) {
            this.typeLoader = typeLoader;
            this.field = field;
        }

        public IType DeclaringType { get { return typeLoader.FromNative(field.DeclaringType); } }

        public string Name { get { return field.Name; } }

        public override string ToString() {
            return string.Format("{0} {1}::{2}", field.FieldType.FullName, field.DeclaringType.FullName, field.Name);
        }

        public override bool Equals(object obj) {
            var other = obj as PencilField;
            return other != null && field.Equals(other.field);
        }

        public override int GetHashCode() {
            return field.GetHashCode();
        }
    }
}

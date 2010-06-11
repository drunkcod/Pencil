using System.Reflection;

namespace Pencil.Core
{
    class PencilField : IField
    {
        readonly FieldInfo field;
        public PencilField(FieldInfo field) {
            this.field = field;
        }

        public string Name { get { return field.Name; } }

        public override string ToString() {
            return string.Format("{0} {1}::{2}", field.FieldType.FullName, field.DeclaringType.FullName, field.Name);
        }
    }
}

using System;

namespace Extensions.Attribute
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class HrmPrimaryKey : System.Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class HrmIgnoreField : System.Attribute
    {

    }
    [AttributeUsage(AttributeTargets.Class)]
    public class TableSchemaAttribute : System.Attribute
    {
        public TableSchemaAttribute(string schema)
        {
            Name = schema;
        }
        public string Name { get; private set; }
    }
}

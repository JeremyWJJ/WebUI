using System;

namespace Common.Modules
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public bool Ignore { get; set; }

        public bool IsRequire { get; set; }

        public object DefaultValue { get; set; }

        public bool IsExtra { get; set; }

        public Type Type { get; set; }

        public ColumnAttribute(string name)
        {
            this.Name = name;
        }

        public ColumnAttribute()
        {
        }
    }
}
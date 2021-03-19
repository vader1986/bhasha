using System;

namespace Bhasha.Common.MongoDB.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ModelFieldAttribute : Attribute
    {
        public string ReferenceFieldName { get; }

        public ModelFieldAttribute(string referenceFieldName)
        {
            ReferenceFieldName = referenceFieldName;
        }
    }
}

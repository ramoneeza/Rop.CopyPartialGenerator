using System;

namespace Rop.CopyPartial.Annotations
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true,Inherited = false)]
    public class CopyPartialToAttribute:Attribute
    {
        public Type ClassName { get; }
        public string[] Exclude { get; }
        public CopyPartialToAttribute(Type classname,params string[] exclude)
        {
            ClassName=ClassName;
            Exclude=exclude;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class CopyPartialAsImmutableRecordAttribute : CopyPartialToAttribute
    {
        public CopyPartialAsImmutableRecordAttribute(Type classname, params string[] exclude) : base(classname, exclude)
        {
        }
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class CopyPartialAsEditableClassAttribute : CopyPartialToAttribute
    {
        public CopyPartialAsEditableClassAttribute(Type classname, params string[] exclude) : base(classname, exclude)
        {
        }
    }

}
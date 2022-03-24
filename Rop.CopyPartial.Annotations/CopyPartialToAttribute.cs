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
}
using System;

namespace Rop.CopyPartial.Annotations
{
    [AttributeUsage(AttributeTargets.All^AttributeTargets.Class,AllowMultiple = true,Inherited = false)]
    public class CopyPartialExcludeAttribute:Attribute
    {
        public CopyPartialExcludeAttribute()
        {
        }
    }
}
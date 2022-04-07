using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.CopyPartial.Annotations;

namespace Test.CopyPartialGenerator
{
    [CopyPartialAsImmutableRecord(typeof(MyImmutable))]
    [CopyPartialAsEditableClass(typeof(MyEditable))]
    public partial class MyDto
    {
        public string PropA { get; set; }
        public string PropB { get; set; }
        public string PropC { get; set; }
    }

}

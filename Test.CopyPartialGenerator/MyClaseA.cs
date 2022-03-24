using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.CopyPartialGenerator
{
    public partial class MyClaseA
    {
        public string Name { get; } = nameof(MyClaseA);
    }

    public interface ICommon
    {
        string Saludo { get; }
        string Despedida();
    }
}

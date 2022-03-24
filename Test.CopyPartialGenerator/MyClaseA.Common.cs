using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rop.CopyPartial.Annotations;

namespace Test.CopyPartialGenerator
{
    [CopyPartialTo(typeof(MyClaseB),nameof(SoloEnA),nameof(MSoloEnA))]
    [CopyPartialTo(typeof(MyClaseC),nameof(SoloEnA),nameof(MSoloEnA))]
    [CopyPartialTo(typeof(MyClaseD),nameof(SoloEnA),nameof(MSoloEnA))]
    public partial class MyClaseA:ICommon
    {
        public string Saludo => "Hola Mundo";
        public string Despedida()=>"Goodbye";
        public string SoloEnA => "SoyA";
        public string MSoloEnA() => "Que soy A";
        [CopyPartialExclude] public string AddSoloEnA => "Que si, solo en A";
        [CopyPartialExclude] public string MAddSoloEnA() => "Que si, solo en A";
        [CopyPartialExclude] public string MAddSoloEnA(string aa) => $"Que si, solo en {aa}";
    }
}

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rop.CopyPartialGenerator
{
    public class PartialClassToAugment
    {
        public bool IsStatic { get; }
        public string Identifier { get; }
        public string FileName { get;  }
        public string Namespace { get;  }
        public IReadOnlyList<(string name, string sentence)> Usings { get;  }
        public ClassDeclarationSyntax Original { get; }
        public PartialClassToAugment(ClassDeclarationSyntax classToAugment)
        {
            Original= classToAugment;
            Identifier = classToAugment.Identifier.ToString();
            var stfp = Path.GetFileNameWithoutExtension(classToAugment.SyntaxTree.FilePath);
            FileName = (string.IsNullOrEmpty(stfp)) ? Identifier : stfp;
            Usings = classToAugment.SyntaxTree.GetUsings().ToList();
            Namespace = classToAugment.SyntaxTree.GetNamespace();
            IsStatic = classToAugment.IsStatic();
        }
        public virtual IEnumerable<string> GetHeader()
        {
            foreach (var u in Usings)
            {
                yield return u.sentence;
            }

            yield return $"namespace {Namespace}";
            yield return "{";
            yield return $"\tpublic {(IsStatic?"static ":"")}partial class {Identifier}";
            yield return "\t{";
        }
        public IEnumerable<string> GetFooter()
        {
            yield return "\t}";
            yield return "}";
        }
    }
}

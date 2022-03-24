using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rop.CopyPartialGenerator
{
    public class CopyClassToAugment
    {
        public PartialClassToAugment ClassToAugment { get; set; }
        public string NewClassName { get; private set; }
        public BaseListSyntax BaseList { get; private set; }
        public List<string> Excludes { get; } = new List<string>();
        public CopyClassToAugment(ClassDeclarationSyntax classToAugment)
        {
            ClassToAugment = new PartialClassToAugment(classToAugment);
            var att = classToAugment.GetDecoratedWith("CopyPartialTo").ArgumentList.ToStringValues().ToList();
            NewClassName = att[0];
            BaseList = classToAugment.BaseList;
            if (att.Count > 1)
            {
                var ex = att.Skip(1);
                Excludes.AddRange(ex);
            }
        }
        public CopyClassToAugment(ClassDeclarationSyntax classToAugment,AttributeSyntax astt)
        {
            ClassToAugment = new PartialClassToAugment(classToAugment);
            var att = astt.ArgumentList.ToStringValues().ToList();
            NewClassName = att[0];
            BaseList = classToAugment.BaseList;
            if (att.Count > 1)
            {
                var ex = att.Skip(1);
                Excludes.AddRange(ex);
            }
        }

        public IEnumerable<string> NewHeader()
        {
            foreach (var u in ClassToAugment.Usings)
            {
                yield return u.sentence;
            }
            yield return $"namespace {ClassToAugment.Namespace}";
            yield return "{";
            var bsstr = BaseList?.ToString() ?? "";
            yield return $"\tpublic partial class {NewClassName}{bsstr}";
            yield return "\t{";
            
            
            
        }

        private List<string> GetInterior()
        {
            var ex = Excludes.ToImmutableHashSet();
            var res = new List<string>();
            foreach (var syntaxNode in ClassToAugment.Original.ChildNodes())
            {
                switch (syntaxNode)
                {
                    case MethodDeclarationSyntax method:
                        var isdecorated = method.IsDecoratedWith("CopyPartialExclude");
                        var name = method.Identifier.ToString();

                        if (!isdecorated && !ex.Contains(name))
                        {
                            res.Add(syntaxNode.ToFullString());
                        }
                        break;
                    case PropertyDeclarationSyntax property:
                        var isdecorated2 = property.IsDecoratedWith("CopyPartialExclude");
                        var name2 = property.Identifier.ToString();
                        if (!isdecorated2 && !ex.Contains(name2))
                        {
                            res.Add(syntaxNode.ToFullString());
                        }
                        break;
                    case AttributeListSyntax attlst:
                        break;
                    case BaseListSyntax baselst:
                        break;
                    default:
                        res.Add(syntaxNode.ToFullString());
                        break;
                }
            }
            return res;
        }

        public IEnumerable<string> NewBody()
        {
            var interior = GetInterior();
            foreach (var str in interior)
            {
                yield return str;
            }
        }
    }
    
}
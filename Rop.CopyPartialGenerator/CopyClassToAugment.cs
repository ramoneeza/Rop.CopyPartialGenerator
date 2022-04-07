using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rop.CopyPartialGenerator
{
    internal enum CopyTypeEnum
    {
        CopyClass,
        CopyImmutable,
        CopyEditable
    }

    public class CopyClassToAugment
    {
        internal CopyTypeEnum AttType { get; }
        public PartialClassToAugment ClassToAugment { get;}
        public string NewClassName { get;}
        public BaseListSyntax BaseList { get; }
        public List<string> Excludes { get; } = new List<string>();
        public CopyClassToAugment(ClassDeclarationSyntax classToAugment,AttributeSyntax astt)
        {
            AttType =_GetAttType(astt.Name.ToString());
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

        private static CopyTypeEnum _GetAttType(string attname)
        {
            switch (attname)
            {
                case "CopyPartialTo": return CopyTypeEnum.CopyClass;
                case "CopyPartialAsImmutableRecord": return CopyTypeEnum.CopyImmutable;
                case "CopyPartialAsEditableClass": return CopyTypeEnum.CopyEditable;
                default: return CopyTypeEnum.CopyClass;
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

            var classorrecord = (AttType == CopyTypeEnum.CopyImmutable) ? "record" : "class";

            yield return $"\tpublic partial {classorrecord} {NewClassName}{bsstr}";
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
                            var full = syntaxNode.ToFullString();
                            switch (AttType)
                            {
                                case CopyTypeEnum.CopyImmutable:
                                    full = full.Replace("set;", "init;");
                                    break;
                                case CopyTypeEnum.CopyEditable:
                                    full = full.Replace("private set;", "set;");
                                    full = full.Replace("init;", "set;");
                                    break;
                            }
                            res.Add(full);
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
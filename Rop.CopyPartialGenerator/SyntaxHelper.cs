using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Rop.CopyPartialGenerator
{
    public static class SyntaxHelper
    {
        /// <summary>
        /// Class is decorated with some attribute
        /// </summary>
        public static bool IsDecoratedWith(this TypeDeclarationSyntax item, string attname)
        {
            return item.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.Name.ToString().Equals(attname));
        }
        /// <summary>
        /// Member is decorated with some attribute
        /// </summary>
        public static bool IsDecoratedWith(this MemberDeclarationSyntax item, string attname)
        {
            return item.AttributeLists.SelectMany(a => a.Attributes).Any(a => a.Name.ToString().Equals(attname));
        }
        /// <summary>
        /// Get decorated attribute for a class
        /// </summary>
        public static AttributeSyntax GetDecoratedWith(this TypeDeclarationSyntax item, string attname)
        {
            return item.AttributeLists.SelectMany(a => a.Attributes).FirstOrDefault(a => a.Name.ToString().Equals(attname));
        }
        public static AttributeSyntax[] GetDecoratedManyWith(this TypeDeclarationSyntax item, string attname)
        {
            return item.AttributeLists.SelectMany(a => a.Attributes).Where(a => a.Name.ToString().Equals(attname)).ToArray();
        }

        /// <summary>
        /// Get decorated attribute for a member
        /// </summary>
        public static AttributeSyntax GetDecoratedWith(this MemberDeclarationSyntax item, string attname)
        {
            return item.AttributeLists.SelectMany(a => a.Attributes).FirstOrDefault(a => a.Name.ToString().Equals(attname));
        }
        /// <summary>
        /// Childs of type T
        /// </summary>
        public static IEnumerable<T> ChildNodesOfType<T>(this SyntaxNode node)
        {
            return node.ChildNodes().OfType<T>();
        }
        public static IEnumerable<SyntaxNode> ChildNodesOfType(this SyntaxNode node,params Type[] types)
        {
            return node.ChildNodes().Where(n => types.Any(t=>IsSameOrSubclass(t,n.GetType())));
        }
        public static bool IsSameOrSubclass(Type potentialBase, Type potentialDescendant)
        {
            return potentialDescendant.IsSubclassOf(potentialBase)
                   || potentialDescendant == potentialBase;
        }

        public static bool IsStatic(this ClassDeclarationSyntax cds)
        {
           return cds.Modifiers.Any(SyntaxKind.StaticKeyword);
        }

        public static string ToStringValue(this ExpressionSyntax expression)
        {
            switch (expression)
            {
                case TypeOfExpressionSyntax tof:
                    return tof.Type.ToString();
                case InvocationExpressionSyntax inv:
                    var a = inv.ArgumentList.Arguments.FirstOrDefault() as ArgumentSyntax;
                    IdentifierNameSyntax i=null;
                    if (a?.Expression is MemberAccessExpressionSyntax b)
                        i = b?.ChildNodes().Last() as IdentifierNameSyntax;
                    else if (a?.Expression is IdentifierNameSyntax ai)
                        i = ai;
                    return i?.Identifier.ToString() ?? "";
                case ArrayCreationExpressionSyntax arr:
                    var arrv = arr.Initializer.Expressions.Select(c => c.ToStringValue());
                    return string.Join(",", arrv);
                default:
                    var v= expression.ToString();
                    if (v.StartsWith("\"")) v = v.Substring(1);
                    if (v.EndsWith("\"")) v = v.Substring(0,v.Length-1);
                    return v;
            }
        }
        public static IEnumerable<string> ToStringValues(this AttributeArgumentListSyntax argumentlist)
        {
            foreach (var arg in argumentlist.Arguments)
            {
                yield return arg.Expression.ToStringValue();
            }
        }

        public static IEnumerable<(string, string)> GetUsings(this SyntaxTree syntaxTree)
        {
            var r = syntaxTree.GetRoot();
            return r.ChildNodesOfType<UsingDirectiveSyntax>().Select(u => (u.Name.ToString(), u.ToString())).ToList();
        }
        public static string GetNamespace(this SyntaxTree syntaxTree){
            var r = syntaxTree.GetRoot();
            return r.ChildNodesOfType<BaseNamespaceDeclarationSyntax>().FirstOrDefault()?.Name.ToString();
        }


        /// <summary>
        /// Appendlines to a stringbuilder
        /// </summary>
        public static void AppendLines(this StringBuilder sb, params string[] lines)
        {
            AppendLines(sb, lines as IEnumerable<string>);
        }
        public static void AppendLines(this StringBuilder sb, IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                sb.AppendLine(line);
            }
        }

    }
}

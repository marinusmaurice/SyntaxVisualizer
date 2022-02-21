/*
 this shitty code im about to write belongs to:
me
 */
using static System.Console;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace WinFormsApp1
{
    // <Snippet3>
    class UsingCollector : CSharpSyntaxWalker
    // </Snippet3>
    {
        // <Snippet4>
        public ICollection<UsingDirectiveSyntax> Usings { get; } = new List<UsingDirectiveSyntax>();
        // </Snippet4>


        public override void DefaultVisit(SyntaxNode node)
        {
            base.DefaultVisit(node);
            Debug.WriteLine(node.ToString());
        }

        // <SNippet5>
        //public override void VisitUsingDirective(UsingDirectiveSyntax node)
        //{
        //    WriteLine($"\tVisitUsingDirective called with {node.Name}.");
        //    if (node.Name.ToString() != "System" &&
        //        !node.Name.ToString().StartsWith("System."))
        //    {
        //        WriteLine($"\t\tSuccess. Adding {node.Name}.");
        //        this.Usings.Add(node);
        //    }
        //}
        //// </Snippet5>
    }
}
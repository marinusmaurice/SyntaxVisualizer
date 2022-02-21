/*
 this shitty code im about to write belongs to:
me
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace WinFormsApp1
{
    class SyntaxTag
    {

        internal TextSpan Span { get; set; }

        internal TextSpan FullSpan { get; set; }

        internal TreeViewItem ParentItem { get; set; }

        internal string Kind { get; set; }

        internal SyntaxNode SyntaxNode { get; set; }

        internal SyntaxToken SyntaxToken { get; set; }

        internal SyntaxTrivia SyntaxTrivia { get; set; }

        internal SyntaxCategory Category { get; set; }
    }
}

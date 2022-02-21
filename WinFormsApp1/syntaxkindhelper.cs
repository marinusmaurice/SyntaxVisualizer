// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using CSharpExtensions = Microsoft.CodeAnalysis.CSharp.CSharpExtensions;
using VisualBasicExtensions = Microsoft.CodeAnalysis.VisualBasic.VisualBasicExtensions;

namespace WinFormsApp1
{
    public static class SyntaxKindHelper
    {
        // Helpers that return the language-specific (C# / VB) SyntaxKind of a language-agnostic
        // SyntaxNode / SyntaxToken / SyntaxTrivia.

        public static string GetKind(this SyntaxNodeOrToken nodeOrToken)
            => nodeOrToken.AsNode() is SyntaxNode node
                ? node.GetKind()
                : nodeOrToken.AsToken().GetKind();

        public static string GetKind(this SyntaxNode node)
            => node.Language == LanguageNames.CSharp
                ? CSharpExtensions.Kind(node).ToString()
                : VisualBasicExtensions.Kind(node).ToString();

        public static string GetKind(this SyntaxToken token)
            => token.Language == LanguageNames.CSharp
                ? CSharpExtensions.Kind(token).ToString()
                : VisualBasicExtensions.Kind(token).ToString();

        public static string GetKind(this SyntaxTrivia trivia)
            => trivia.Language == LanguageNames.CSharp
                ? CSharpExtensions.Kind(trivia).ToString()
                : VisualBasicExtensions.Kind(trivia).ToString();

        public static Document GetOpenDocumentInCurrentContextWithChanges(this SourceText text)
        {
            Workspace workspace;
            if (!Workspace.TryGetWorkspace(text.Container, out workspace))
            {
                return null;
            }
            Solution currentSolution = workspace.CurrentSolution;
            DocumentId documentIdInCurrentContext = workspace.GetDocumentIdInCurrentContext(text.Container);
            if (documentIdInCurrentContext == null || !currentSolution.ContainsDocument(documentIdInCurrentContext))
            {
                return null;
            }
            ImmutableArray<DocumentId> relatedDocumentIds = new ImmutableArray<DocumentId>();// currentSolution.GetRelatedDocumentIds(documentIdInCurrentContext);
            return currentSolution.WithDocumentText(relatedDocumentIds, text, PreservationMode.PreserveIdentity).GetDocument(documentIdInCurrentContext);
        }

    }
}
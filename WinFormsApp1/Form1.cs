/*
 this shitty code im about to write belongs to:
me
 */
using static System.Console;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Classification;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.Host.Mef; 

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private ImmutableArray<ClassifiedSpan> classifiedSpans;
        private bool _isNavigatingFromTreeToSource;
        private bool _isNavigatingFromSourceToTree;

        public bool IsLazy { get; private set; }
        public SyntaxTree SyntaxTree { get; private set; }
        public SemanticModel SemanticModel { get; private set; }

        public Form1()
        {
            InitializeComponent();
            textBox1.MaxLength = Int32.MaxValue;
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            // <Snippet1>
            string programText = textBox1.Text;

       
            var workspace = new AdhocWorkspace();
            var projectId = ProjectId.CreateNewId();
            var versionStamp = VersionStamp.Create();
            var projectInfo = ProjectInfo.Create(projectId, versionStamp, "NewProject", "projName", LanguageNames.CSharp);
            var newProject = workspace.AddProject(projectInfo);
          
            var sourceText = SourceText.From(programText);
            var document = workspace.AddDocument(newProject.Id, "NewFile.cs", sourceText);
            var syntaxTree = await document.GetSyntaxTreeAsync();
          //  var compilation = CSharpCompilation.Create("Dummy").AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location)).AddSyntaxTrees(syntaxTree);
            var semanticModel = await  document.GetSemanticModelAsync();

/*
            var syntaxTree = CSharpSyntaxTree.ParseText(sourceText);
            var compilation = CSharpCompilation.Create("Dummy").AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location)).AddSyntaxTrees(syntaxTree);
            var semanticModel = compilation.GetSemanticModel(syntaxTree);
*/

            DisplaySyntaxTree(syntaxTree, semanticModel, false, workspace);

            int x = 0;


        }
         
        public void DisplaySyntaxTree( SyntaxTree tree, SemanticModel model = null, bool lazy = true, Workspace workspace = null)
        {
            if (tree != null)
            {
                this.IsLazy = lazy;
                this.SyntaxTree = tree;
                this.SemanticModel = model;
                this.AddNode(null, this.SyntaxTree.GetRoot(default(CancellationToken)));
                if (model != null && workspace != null)
                {
                    this.classifiedSpans = ImmutableArray.ToImmutableArray<ClassifiedSpan>(Classifier.GetClassifiedSpans(model, tree.GetRoot(default(CancellationToken)).FullSpan, workspace, default(CancellationToken)));
                    return;
                }
                this.classifiedSpans = ImmutableArray<Microsoft.CodeAnalysis.Classification.ClassifiedSpan>.Empty;
            }
        }
         
        public void DisplaySyntaxNode(SyntaxNode node, SemanticModel model = null, bool lazy = true)
        {
            if (node != null)
            {
                this.IsLazy = lazy;
                this.SyntaxTree = node.SyntaxTree;
                this.SemanticModel = model;
                this.AddNode(null, node);
            }
        }
         
        public bool NavigateToBestMatch(int position, string kind = null, SyntaxCategory category = SyntaxCategory.None, bool highlightMatch = false)
        {
            TreeViewItem treeViewItem = null;
            if (this.treeView.Nodes.Count >0 && !this._isNavigatingFromTreeToSource)
            {
                this._isNavigatingFromSourceToTree = true;
                treeViewItem = this.NavigateToBestMatch((TreeViewItem)this.treeView.Nodes[0], position, kind, category);
                this._isNavigatingFromSourceToTree = false;
            }
            if (!highlightMatch || treeViewItem == null)
            {
                return false;
            }
            //treeViewItem.Background = System.Windows.Media.Brushes.Yellow;
            //treeViewItem.BorderBrush = System.Windows.Media.Brushes.Black;
            //treeViewItem.BorderThickness = SyntaxVisualizerControl.s_defaultBorderThickness;
            return true;
        }

        private TreeViewItem NavigateToBestMatch( TreeViewItem current, int position, string kind = null, SyntaxCategory category = SyntaxCategory.None)
        {
            TreeViewItem treeViewItem = null;
            if (current != null)
            {
                SyntaxTag syntaxTag = (SyntaxTag)current.Tag;
                if (syntaxTag.FullSpan.Contains(position))
                {
                   // this.CollapseEverythingBut(current);
                    foreach (object obj in current.Nodes)
                    {
                        TreeViewItem current2 = (TreeViewItem)obj;
                        treeViewItem = this.NavigateToBestMatch(current2, position, kind, category);
                        if (treeViewItem != null)
                        {
                            break;
                        }
                    }
                    if (treeViewItem == null && (kind == null || syntaxTag.Kind == kind) && (category == SyntaxCategory.None || category == syntaxTag.Category))
                    {
                        treeViewItem = current;
                    }
                }
            }
            return treeViewItem;
        }
 
        private TreeViewItem NavigateToBestMatch(  TreeViewItem current, TextSpan span, string kind = null, SyntaxCategory category = SyntaxCategory.None)
        {
            TreeViewItem treeViewItem = null;
            if (current != null)
            {
                SyntaxTag syntaxTag = (SyntaxTag)current.Tag;
                if (syntaxTag.FullSpan.Contains(span))
                {
                    if ((syntaxTag.Span == span || syntaxTag.FullSpan == span) && (kind == null || syntaxTag.Kind == kind))
                    {
                       // this.CollapseEverythingBut(current);
                        treeViewItem = current;
                    }
                    else
                    {
                       // this.CollapseEverythingBut(current);
                        foreach (object obj in current.Nodes)
                        {
                            TreeViewItem treeViewItem2 = (TreeViewItem)obj;
                            if (category == SyntaxCategory.Operation || ((SyntaxTag)treeViewItem2.Tag).Category != SyntaxCategory.Operation)
                            {
                                treeViewItem = this.NavigateToBestMatch(treeViewItem2, span, kind, category);
                                if (treeViewItem != null)
                                {
                                    break;
                                }
                            }
                        }
                        if (treeViewItem == null && (kind == null || syntaxTag.Kind == kind) && (category == SyntaxCategory.None || category == syntaxTag.Category))
                        {
                            treeViewItem = current;
                        }
                    }
                }
            }
            return treeViewItem;
        }
       
        private void AddNode(TreeViewItem parentItem, SyntaxNode node)
        {
            if (node == null)
            {
                return;
            }
            string kind = node.GetKind();
            SyntaxTag syntaxTag = new SyntaxTag
            {
                SyntaxNode = node,
                Category = SyntaxCategory.SyntaxNode,
                Span = node.Span,
                FullSpan = node.FullSpan,
                Kind = kind,
                ParentItem = parentItem
            };
            TreeViewItem item = this.CreateTreeViewItem(syntaxTag, syntaxTag.Kind + " " + node.Span.ToString(), node.ContainsDiagnostics);
           // item.SetResourceReference(System.Windows.Controls.Control.ForegroundProperty, SyntaxVisualizerControl.SyntaxNodeTextBrushKey);
            if (this.SyntaxTree != null && node.ContainsDiagnostics)
            {
                item.ToolTip = string.Empty;
                foreach (Diagnostic diagnostic in this.SyntaxTree.GetDiagnostics(node))
                {
                    TreeViewItem item2 = item;
                    object toolTip = item2.ToolTip;
                    item2.ToolTip = ((toolTip != null) ? toolTip.ToString() : null) + diagnostic.ToString() + "\n";
                }
                item.ToolTip = item.ToolTip.ToString().Trim();
            }
            item.Selected += delegate (object sender, EventArgs e)
            {
                this._isNavigatingFromTreeToSource = true;
                //this.typeTextLabel.Visibility = Visibility.Visible;
                //this.kindTextLabel.Visibility = Visibility.Visible;
                this.typeValueLabel.Text = node.GetType().Name;
                this.kindValueLabel.Text = kind;
                this._propertyGrid.SelectedObject = node;
                item.IsExpanded = true;
                if (!this._isNavigatingFromSourceToTree && this.SyntaxNodeNavigationToSourceRequested != null)
                {
                    this.SyntaxNodeNavigationToSourceRequested(node);
                }
                this._isNavigatingFromTreeToSource = false;
                //e.Handled = true;
            };
            item.Expanded += delegate (object sender, EventArgs e)
            {
                if (item.Nodes.Count == 1 && item.Nodes[0] == null)
                {
                    item.Nodes.RemoveAt(0);
                    if (this.SemanticModel != null)
                    {
                        IOperation operation = this.SemanticModel.GetOperation(node, default(CancellationToken));
                        if (operation != null && operation.Parent == null)
                        {
                            this.AddOperation(item, operation);
                        }
                    }
                    foreach (SyntaxNodeOrToken nodeOrToken2 in node.ChildNodesAndTokens())
                    {
                        this.AddNodeOrToken(item, nodeOrToken2);
                    }
                }
            };
            if (parentItem == null)
            {
                this.treeView.Nodes.Clear();
                this.treeView.Nodes.Add(item);
            }
            else
            {
                parentItem.Nodes.Add(item);
            }
            if (node.ChildNodesAndTokens().Count > 0)
            {
                if (this.IsLazy)
                {
                    item.Nodes.Add(string.Empty);
                    return;
                }
                foreach (SyntaxNodeOrToken nodeOrToken in node.ChildNodesAndTokens())
                {
                    this.AddNodeOrToken(item, nodeOrToken);
                }
            }
        }

        private void AddNodeOrToken(TreeViewItem parentItem, SyntaxNodeOrToken nodeOrToken)
        {
            if (nodeOrToken.IsNode)
            {
                this.AddNode(parentItem, nodeOrToken.AsNode());
                return;
            }
            this.AddToken(parentItem, nodeOrToken.AsToken());
        }
 
        private void AddOperation(TreeViewItem parentItem, IOperation operation)
        {
            SyntaxNode node = operation.Syntax;
            string kind = operation.Kind.ToString();
            SyntaxTag syntaxTag = new SyntaxTag
            {
                SyntaxNode = node,
                Category = SyntaxCategory.Operation,
                Span = node.Span,
                FullSpan = node.FullSpan,
                Kind = kind,
                ParentItem = parentItem
            };
            TreeViewItem item = this.CreateTreeViewItem(syntaxTag, syntaxTag.Kind + " " + node.Span.ToString(), node.ContainsDiagnostics);
          //  item.SetResourceReference(System.Windows.Controls.Control.ForegroundProperty, SyntaxVisualizerControl.OperationTextBrushKey);
            if (this.SyntaxTree != null && node.ContainsDiagnostics)
            {
                item.ToolTip = string.Empty;
                foreach (Diagnostic diagnostic in this.SyntaxTree.GetDiagnostics(node))
                {
                    TreeViewItem item2 = item;
                    object toolTip = item2.ToolTip;
                    item2.ToolTip = ((toolTip != null) ? toolTip.ToString() : null) + diagnostic.ToString() + "\n";
                }
                item.ToolTip = item.ToolTip.ToString().Trim();
            }
            item.Selected += delegate (object sender, EventArgs e)
            {
                this._isNavigatingFromTreeToSource = true;
                //this.typeTextLabel.Visibility = Visibility.Visible;
                //this.kindTextLabel.Visibility = Visibility.Visible;
                this.typeValueLabel.Text = string.Join(", ", GetOperationInterfaces(operation));
                this.kindValueLabel.Text = kind;
                this._propertyGrid.SelectedObject = operation;
                item.IsExpanded = true;
                if (!this._isNavigatingFromSourceToTree && this.SyntaxNodeNavigationToSourceRequested != null)
                {
                    this.SyntaxNodeNavigationToSourceRequested(node);
                }
                this._isNavigatingFromTreeToSource = false;
             //   e.Handled = true;
            };
            item.Expanded += delegate (object sender, EventArgs e)
            {
                if (item.Nodes.Count == 1 && item.Nodes[0] == null)
                {
                    item.Nodes.RemoveAt(0);
                    foreach (IOperation operation3 in operation.Children)
                    {
                        this.AddOperation(item, operation3);
                    }
                }
            };
            if (parentItem == null)
            {
                this.treeView.Nodes.Clear();
                this.treeView.Nodes.Add(item);
            }
            else
            {
                parentItem.Nodes.Add(item);
            }
            if (operation.Children.Any<IOperation>())
            {
                if (this.IsLazy)
                {
                    item.Nodes.Add(string.Empty);
                    return;
                }
                foreach (IOperation operation2 in operation.Children)
                {
                    this.AddOperation(item, operation2);
                }
            }

            static IEnumerable<string> GetOperationInterfaces(IOperation operation)
            {
                var interfaces = new List<Type>();
                foreach (var interfaceType in operation.GetType().GetInterfaces())
                {
                    if (interfaceType == typeof(IOperation)
                        || !interfaceType.IsPublic
                        || !interfaceType.GetInterfaces().Contains(typeof(IOperation)))
                    {
                        continue;
                    }

                    interfaces.Add(interfaceType);
                }

                if (interfaces.Count == 0)
                {
                    interfaces.Add(typeof(IOperation));
                }

                return interfaces.OrderByDescending(x => x.GetInterfaces().Length).Select(x => x.Name);
            }
        }

        private void AddToken(TreeViewItem parentItem, SyntaxToken token)
        {
            string kind = token.GetKind();
            SyntaxTag syntaxTag = new SyntaxTag
            {
                SyntaxToken = token,
                Category = SyntaxCategory.SyntaxToken,
                Span = token.Span,
                FullSpan = token.FullSpan,
                Kind = kind,
                ParentItem = parentItem
            };
            TreeViewItem item = this.CreateTreeViewItem(syntaxTag, syntaxTag.Kind + " " + token.Span.ToString(), token.ContainsDiagnostics);
           // item.SetResourceReference(System.Windows.Controls.Control.ForegroundProperty, SyntaxVisualizerControl.SyntaxTokenTextBrushKey);
            if (this.SyntaxTree != null && token.ContainsDiagnostics)
            {
                item.ToolTip = string.Empty;
                foreach (Diagnostic diagnostic in this.SyntaxTree.GetDiagnostics(token))
                {
                    TreeViewItem item2 = item;
                    object toolTip = item2.ToolTip;
                    item2.ToolTip = ((toolTip != null) ? toolTip.ToString() : null) + diagnostic.ToString() + "\n";
                }
                item.ToolTip = item.ToolTip.ToString().Trim();
            }
            item.Selected += delegate (object sender, EventArgs e)
            {
                this._isNavigatingFromTreeToSource = true;
                //this.typeTextLabel.Visibility = Visibility.Visible;
                //this.kindTextLabel.Visibility = Visibility.Visible;
                this.typeValueLabel.Text = token.GetType().Name;
                this.kindValueLabel.Text = kind;
                this._propertyGrid.SelectedObject = token;
                item.IsExpanded = true;
                if (!this._isNavigatingFromSourceToTree && this.SyntaxTokenNavigationToSourceRequested != null)
                {
                    this.SyntaxTokenNavigationToSourceRequested(token);
                }
                this._isNavigatingFromTreeToSource = false;
                //e.Handled = true;
            };
            item.Expanded += delegate (object sender, EventArgs e)
            {
                if (item.Nodes.Count == 1 && item.Nodes[0] == null)
                {
                    item.Nodes.RemoveAt(0);
                    foreach (SyntaxTrivia trivia3 in token.LeadingTrivia)
                    {
                        this.AddTrivia(item, trivia3, true);
                    }
                    foreach (SyntaxTrivia trivia4 in token.TrailingTrivia)
                    {
                        this.AddTrivia(item, trivia4, false);
                    }
                }
            };
            if (parentItem == null)
            {
                this.treeView.Nodes.Clear();
                this.treeView.Nodes.Add(item);
            }
            else
            {
                parentItem.Nodes.Add(item);
            }
            if (token.HasLeadingTrivia || token.HasTrailingTrivia)
            {
                if (this.IsLazy)
                {
                    item.Nodes.Add(string.Empty);
                    return;
                }
                foreach (SyntaxTrivia trivia in token.LeadingTrivia)
                {
                    this.AddTrivia(item, trivia, true);
                }
                foreach (SyntaxTrivia trivia2 in token.TrailingTrivia)
                {
                    this.AddTrivia(item, trivia2, false);
                }
            }
        }

        private void AddTrivia(TreeViewItem parentItem, SyntaxTrivia trivia, bool isLeadingTrivia)
        {
            string kind = trivia.GetKind();
            SyntaxTag syntaxTag = new SyntaxTag
            {
                SyntaxTrivia = trivia,
                Category = SyntaxCategory.SyntaxTrivia,
                Span = trivia.Span,
                FullSpan = trivia.FullSpan,
                Kind = kind,
                ParentItem = parentItem
            };
            TreeViewItem item = this.CreateTreeViewItem(syntaxTag, (isLeadingTrivia ? "Lead: " : "Trail: ") + syntaxTag.Kind + " " + trivia.Span.ToString(), trivia.ContainsDiagnostics);
           // item.SetResourceReference(System.Windows.Controls.Control.ForegroundProperty, SyntaxVisualizerControl.SyntaxTriviaTextBrushKey);
            if (this.SyntaxTree != null && trivia.ContainsDiagnostics)
            {
                item.ToolTip = string.Empty;
                foreach (Diagnostic diagnostic in this.SyntaxTree.GetDiagnostics(trivia))
                {
                    TreeViewItem item2 = item;
                    object toolTip = item2.ToolTip;
                    item2.ToolTip = ((toolTip != null) ? toolTip.ToString() : null) + diagnostic.ToString() + "\n";
                }
                item.ToolTip = item.ToolTip.ToString().Trim();
            }
            item.Selected += delegate (object sender, EventArgs e)
            {
                this._isNavigatingFromTreeToSource = true;
                //this.typeTextLabel.Visibility = Visibility.Visible;
                //this.kindTextLabel.Visibility = Visibility.Visible;
                this.typeValueLabel.Text = trivia.GetType().Name;
                this.kindValueLabel.Text = kind;
                this._propertyGrid.SelectedObject = trivia;
                item.IsExpanded = true;
                if (!this._isNavigatingFromSourceToTree && this.SyntaxTriviaNavigationToSourceRequested != null)
                {
                    this.SyntaxTriviaNavigationToSourceRequested(trivia);
                }
                this._isNavigatingFromTreeToSource = false;
                //e.Handled = true;
            };
            item.Expanded += delegate (object sender, EventArgs e)
            {
                if (item.Nodes.Count == 1 && item.Nodes[0] == null)
                {
                    item.Nodes.RemoveAt(0);
                    this.AddNode(item, trivia.GetStructure());
                }
            };
            if (parentItem == null)
            {
                this.treeView.Nodes.Clear();
                this.treeView.Nodes.Add(item);
                //this.typeTextLabel.Visibility = Visibility.Hidden;
                //this.kindTextLabel.Visibility = Visibility.Hidden;
                this.typeValueLabel.Text = string.Empty;
                this.kindValueLabel.Text = string.Empty;
            }
            else
            {
                parentItem.Nodes.Add(item);
            }
            if (trivia.HasStructure)
            {
                if (this.IsLazy)
                {
                    item.Nodes.Add(string.Empty);
                    return;
                }
                this.AddNode(item, trivia.GetStructure());
            }
        }

      
        private TreeViewItem CreateTreeViewItem(SyntaxTag tag, string text, bool containsDiagnostics)
        {
             TreeViewItem treeViewItem = new TreeViewItem
             {
                 Tag = tag,
                 IsExpanded = false,
                // Background = System.Windows.Media.Brushes.Transparent
             };
           /*  if (containsDiagnostics)
             {
                 TextBlock textBlock = new TextBlock(new Run(text));
                 BindingOperations.SetBinding(new SolidColorBrush(), SolidColorBrush.ColorProperty, new System.Windows.Data.Binding
                 {
                     Source = base.FindResource(SyntaxVisualizerControl.ErrorSquiggleBrushKey),
                     Path = new PropertyPath(SolidColorBrush.ColorProperty.Name, Array.Empty<object>())
                 });
                 System.Windows.Shapes.Rectangle rectangle = new System.Windows.Shapes.Rectangle
                 {
                     HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                     VerticalAlignment = VerticalAlignment.Bottom,
                     Height = 3.0
                 };
                 rectangle.SetResourceReference(FrameworkElement.StyleProperty, SyntaxVisualizerControl.SquiggleStyleKey);
                 BindingOperations.SetBinding(rectangle, FrameworkElement.WidthProperty, new System.Windows.Data.Binding
                 {
                     Source = textBlock,
                     Path = new PropertyPath(FrameworkElement.ActualWidthProperty.Name, Array.Empty<object>())
                 });
                 treeViewItem.Header = new Grid
                 {
                     Children =
                     {
                         textBlock,
                         rectangle
                     }
                 };
             }
             else*/
             {
                treeViewItem.Text = text;//new TextBlock(new Run(text));
             }
             
            return treeViewItem;
        }

        public delegate void SyntaxNodeDelegate(SyntaxNode? node);
        public event SyntaxNodeDelegate? SyntaxNodeDirectedGraphRequested;
        public event SyntaxNodeDelegate? SyntaxNodeNavigationToSourceRequested;


        public delegate void SyntaxTokenDelegate(SyntaxToken token);
        public event SyntaxTokenDelegate? SyntaxTokenDirectedGraphRequested;
        public event SyntaxTokenDelegate? SyntaxTokenNavigationToSourceRequested;

        public delegate void SyntaxTriviaDelegate(SyntaxTrivia trivia);
        public event SyntaxTriviaDelegate? SyntaxTriviaDirectedGraphRequested;
        public event SyntaxTriviaDelegate? SyntaxTriviaNavigationToSourceRequested;

        private async void button3_Click(object sender, EventArgs e)
        {
            var code = @"using System;

public class MyClass
{
    public static void MyMethod(int value)
    {
    }
}";
            var host = MefHostServices.Create(MefHostServices.DefaultAssemblies);
            var workspace = new AdhocWorkspace(host);
            var souceText = SourceText.From(code);
            var projectInfo = ProjectInfo.Create(ProjectId.CreateNewId(), VersionStamp.Create(), "MyProject", "MyProject", LanguageNames.CSharp).WithMetadataReferences(new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) });
            var project = workspace.AddProject(projectInfo);
            var document = workspace.AddDocument(project.Id, "MyFile.cs", souceText);

            var classifiedSpans = await Classifier.GetClassifiedSpansAsync(document, new TextSpan(0, code.Length));
            foreach (var classifiedSpan in classifiedSpans)
            {
                var position = souceText.Lines.GetLinePositionSpan(classifiedSpan.TextSpan);
                Console.WriteLine($"{souceText.ToString(classifiedSpan.TextSpan)} - {classifiedSpan.ClassificationType} - {position.Start}:{position.End}");
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.IsSelected)
            {
                ((TreeViewItem)e.Node).IsSelected();
            }
        }
        #region
        private void button4_Click(object sender, EventArgs e)
        {
            
        }
 
    }
    #endregion
    public enum SyntaxCategory
    {
                None,
                SyntaxNode,
                SyntaxToken,
                SyntaxTrivia,
                Operation
    }
   
}

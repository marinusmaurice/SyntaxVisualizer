/*
 this shitty code im about to write belongs to:
me
 */

using System;
using System.Windows.Forms;
namespace WinFormsApp1
{
    public class TreeViewItem : TreeNode
    {
        public string ToolTip { get { return base.ToolTipText; } set { base.ToolTipText = value; } }
        public bool IsExpanded { get { return base.IsExpanded; } set { value = true; } }
        public object Background { get; internal set; }

        public event OnSelectedHandler Selected;
        bool _selected;
        public void IsSelected()
        {
            /*set { _selected =value; 
                if (value)
                Selected(null, new EventArgs());
            }
            get {
                return _selected;            }*/
            Selected(null, null);
           
        }

        public delegate void OnSelectedHandler(object sender, EventArgs e);
           
         
        public event EventHandler Expanded;
        public delegate void ExpandedHandler(object sender, EventArgs e);
     

    }

    public class TreeViewItemEventArgs : EventArgs

    {

        public TreeViewItem _employee;

        public TreeViewItemEventArgs(TreeViewItem employee)

        {

            this._employee = employee;

        }

    }
}
/*
 this shitty code im about to write belongs to:
me
 */
namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.treeView = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.typeValueLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.kindValueLabel = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this._propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(454, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generate Syntax!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_ClickAsync);
            // 
            // treeView
            // 
            this.treeView.Location = new System.Drawing.Point(454, 52);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(245, 377);
            this.treeView.TabIndex = 2;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(847, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Type:";
            // 
            // typeValueLabel
            // 
            this.typeValueLabel.AutoSize = true;
            this.typeValueLabel.Location = new System.Drawing.Point(913, 21);
            this.typeValueLabel.Name = "typeValueLabel";
            this.typeValueLabel.Size = new System.Drawing.Size(86, 15);
            this.typeValueLabel.TabIndex = 4;
            this.typeValueLabel.Text = "typeValueLabel";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(847, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Kind:";
            // 
            // kindValueLabel
            // 
            this.kindValueLabel.AutoSize = true;
            this.kindValueLabel.Location = new System.Drawing.Point(913, 52);
            this.kindValueLabel.Name = "kindValueLabel";
            this.kindValueLabel.Size = new System.Drawing.Size(86, 15);
            this.kindValueLabel.TabIndex = 6;
            this.kindValueLabel.Text = "kindValueLabel";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(732, 17);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // _propertyGrid
            // 
            this._propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._propertyGrid.Location = new System.Drawing.Point(723, 82);
            this._propertyGrid.Name = "_propertyGrid";
            this._propertyGrid.Size = new System.Drawing.Size(276, 343);
            this._propertyGrid.TabIndex = 8;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 21);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(410, 408);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            this.textBox1.WordWrap = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 450);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this._propertyGrid);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.kindValueLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.typeValueLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label typeValueLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label kindValueLabel;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PropertyGrid _propertyGrid;
        private System.Windows.Forms.TextBox textBox1;
    }
}


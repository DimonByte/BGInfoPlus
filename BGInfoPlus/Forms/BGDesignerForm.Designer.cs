namespace BGInfoPlus
{
    partial class BGDesignerForm
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
            TreeNode treeNode1 = new TreeNode("IFStatement1");
            TreeNode treeNode2 = new TreeNode("Text1", new TreeNode[] { treeNode1 });
            TreeNode treeNode3 = new TreeNode("Element1", new TreeNode[] { treeNode2 });
            bgSelectedImage = new PictureBox();
            treeView1 = new TreeView();
            button1 = new Button();
            comboBox1 = new ComboBox();
            label1 = new Label();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            label2 = new Label();
            button5 = new Button();
            ((System.ComponentModel.ISupportInitialize)bgSelectedImage).BeginInit();
            SuspendLayout();
            // 
            // bgSelectedImage
            // 
            bgSelectedImage.BorderStyle = BorderStyle.Fixed3D;
            bgSelectedImage.Location = new Point(48, 301);
            bgSelectedImage.Name = "bgSelectedImage";
            bgSelectedImage.Size = new Size(119, 80);
            bgSelectedImage.SizeMode = PictureBoxSizeMode.StretchImage;
            bgSelectedImage.TabIndex = 0;
            bgSelectedImage.TabStop = false;
            // 
            // treeView1
            // 
            treeView1.Location = new Point(12, 55);
            treeView1.Name = "treeView1";
            treeNode1.Name = "IFStatement";
            treeNode1.Text = "IFStatement1";
            treeNode2.Name = "Node0";
            treeNode2.Text = "Text1";
            treeNode3.Name = "Element";
            treeNode3.Text = "Element1";
            treeView1.Nodes.AddRange(new TreeNode[] { treeNode3 });
            treeView1.Size = new Size(199, 225);
            treeView1.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(121, 26);
            button1.Name = "button1";
            button1.Size = new Size(90, 23);
            button1.TabIndex = 2;
            button1.Text = "Add new...";
            button1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Element", "Text", "Image", "IFStatement" });
            comboBox1.Location = new Point(12, 27);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(103, 23);
            comboBox1.TabIndex = 3;
            comboBox1.Text = "Element";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(78, 15);
            label1.TabIndex = 4;
            label1.Text = "Element Tree:";
            // 
            // button2
            // 
            button2.Location = new Point(467, 387);
            button2.Name = "button2";
            button2.Size = new Size(103, 23);
            button2.TabIndex = 2;
            button2.Text = "Save as .bgip";
            button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(371, 387);
            button3.Name = "button3";
            button3.Size = new Size(90, 23);
            button3.TabIndex = 2;
            button3.Text = "Preview";
            button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(275, 387);
            button4.Name = "button4";
            button4.Size = new Size(90, 23);
            button4.TabIndex = 2;
            button4.Text = "Apply";
            button4.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.Location = new Point(12, 283);
            label2.Name = "label2";
            label2.Size = new Size(199, 15);
            label2.TabIndex = 4;
            label2.Text = "Currently using background:";
            label2.TextAlign = ContentAlignment.TopCenter;
            // 
            // button5
            // 
            button5.Location = new Point(12, 387);
            button5.Name = "button5";
            button5.Size = new Size(199, 23);
            button5.TabIndex = 2;
            button5.Text = "Change...";
            button5.UseVisualStyleBackColor = true;
            // 
            // BGDesignerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(582, 422);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(comboBox1);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button5);
            Controls.Add(button1);
            Controls.Add(treeView1);
            Controls.Add(bgSelectedImage);
            Margin = new Padding(3, 2, 3, 2);
            Name = "BGDesignerForm";
            Text = "BGInfo+";
            Load += BGDesignerForm_Load;
            ((System.ComponentModel.ISupportInitialize)bgSelectedImage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox bgSelectedImage;
        private TreeView treeView1;
        private Button button1;
        private ComboBox comboBox1;
        private Label label1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Label label2;
        private Button button5;
    }
}

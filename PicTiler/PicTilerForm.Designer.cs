namespace PicTiler
{
    partial class PicTilerForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PicTilerForm));
			picList1 = new PicList();
			pictureBox1 = new PictureBox();
			btnUp = new Button();
			menuStrip1 = new MenuStrip();
			fileToolStripMenuItem = new ToolStripMenuItem();
			addImageMenu = new ToolStripMenuItem();
			toolStripMenuItem1 = new ToolStripSeparator();
			exportMenu = new ToolStripMenuItem();
			toolStripMenuItem2 = new ToolStripSeparator();
			quitMenu = new ToolStripMenuItem();
			editToolStripMenuItem = new ToolStripMenuItem();
			upMenu = new ToolStripMenuItem();
			downMenu = new ToolStripMenuItem();
			removeMenu = new ToolStripMenuItem();
			btnDown = new Button();
			btnRemove = new Button();
			numWidth = new NumericUpDown();
			numHeight = new NumericUpDown();
			label1 = new Label();
			label2 = new Label();
			lbInfo = new Label();
			btnExport = new Button();
			btnClear = new Button();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)numWidth).BeginInit();
			((System.ComponentModel.ISupportInitialize)numHeight).BeginInit();
			SuspendLayout();
			// 
			// picList1
			// 
			picList1.AllowDrop = true;
			picList1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			picList1.FormattingEnabled = true;
			picList1.Location = new Point(12, 96);
			picList1.Name = "picList1";
			picList1.ScrollAlwaysVisible = true;
			picList1.Size = new Size(454, 244);
			picList1.TabIndex = 10;
			// 
			// pictureBox1
			// 
			pictureBox1.Image = Properties.Resources.icon048;
			pictureBox1.InitialImage = Properties.Resources.icon048;
			pictureBox1.Location = new Point(12, 42);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new Size(48, 48);
			pictureBox1.TabIndex = 1;
			pictureBox1.TabStop = false;
			// 
			// btnUp
			// 
			btnUp.Enabled = false;
			btnUp.Location = new Point(83, 62);
			btnUp.Name = "btnUp";
			btnUp.Size = new Size(58, 28);
			btnUp.TabIndex = 1;
			btnUp.Text = "Up";
			btnUp.UseVisualStyleBackColor = true;
			// 
			// menuStrip1
			// 
			menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem });
			menuStrip1.Location = new Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Size = new Size(478, 24);
			menuStrip1.TabIndex = 0;
			menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { addImageMenu, toolStripMenuItem1, exportMenu, toolStripMenuItem2, quitMenu });
			fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			fileToolStripMenuItem.Size = new Size(37, 20);
			fileToolStripMenuItem.Text = "File";
			// 
			// addImageMenu
			// 
			addImageMenu.Name = "addImageMenu";
			addImageMenu.Size = new Size(128, 22);
			addImageMenu.Text = "AddImage";
			// 
			// toolStripMenuItem1
			// 
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new Size(125, 6);
			// 
			// exportMenu
			// 
			exportMenu.Enabled = false;
			exportMenu.Name = "exportMenu";
			exportMenu.Size = new Size(128, 22);
			exportMenu.Text = "Export";
			// 
			// toolStripMenuItem2
			// 
			toolStripMenuItem2.Name = "toolStripMenuItem2";
			toolStripMenuItem2.Size = new Size(125, 6);
			// 
			// quitMenu
			// 
			quitMenu.Name = "quitMenu";
			quitMenu.Size = new Size(128, 22);
			quitMenu.Text = "Quit";
			// 
			// editToolStripMenuItem
			// 
			editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { upMenu, downMenu, removeMenu });
			editToolStripMenuItem.Name = "editToolStripMenuItem";
			editToolStripMenuItem.Size = new Size(39, 20);
			editToolStripMenuItem.Text = "Edit";
			// 
			// upMenu
			// 
			upMenu.Enabled = false;
			upMenu.Name = "upMenu";
			upMenu.ShortcutKeys = Keys.Control | Keys.Up;
			upMenu.Size = new Size(169, 22);
			upMenu.Text = "Up";
			// 
			// downMenu
			// 
			downMenu.Enabled = false;
			downMenu.Name = "downMenu";
			downMenu.ShortcutKeys = Keys.Control | Keys.Down;
			downMenu.Size = new Size(169, 22);
			downMenu.Text = "Down";
			// 
			// removeMenu
			// 
			removeMenu.Enabled = false;
			removeMenu.Name = "removeMenu";
			removeMenu.ShortcutKeys = Keys.Delete;
			removeMenu.Size = new Size(169, 22);
			removeMenu.Text = "Remove";
			// 
			// btnDown
			// 
			btnDown.Enabled = false;
			btnDown.Location = new Point(147, 62);
			btnDown.Name = "btnDown";
			btnDown.Size = new Size(58, 28);
			btnDown.TabIndex = 2;
			btnDown.Text = "Down";
			btnDown.UseVisualStyleBackColor = true;
			// 
			// btnRemove
			// 
			btnRemove.Enabled = false;
			btnRemove.Location = new Point(211, 62);
			btnRemove.Name = "btnRemove";
			btnRemove.Size = new Size(58, 28);
			btnRemove.TabIndex = 4;
			btnRemove.Text = "Remove";
			btnRemove.UseVisualStyleBackColor = true;
			// 
			// numWidth
			// 
			numWidth.Location = new Point(115, 30);
			numWidth.Maximum = new decimal(new int[] { 80, 0, 0, 0 });
			numWidth.Minimum = new decimal(new int[] { 4, 0, 0, 0 });
			numWidth.Name = "numWidth";
			numWidth.Size = new Size(41, 23);
			numWidth.TabIndex = 6;
			numWidth.Value = new decimal(new int[] { 32, 0, 0, 0 });
			// 
			// numHeight
			// 
			numHeight.Location = new Point(180, 30);
			numHeight.Maximum = new decimal(new int[] { 80, 0, 0, 0 });
			numHeight.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			numHeight.Name = "numHeight";
			numHeight.Size = new Size(34, 23);
			numHeight.TabIndex = 8;
			numHeight.Value = new decimal(new int[] { 4, 0, 0, 0 });
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(93, 34);
			label1.Name = "label1";
			label1.Size = new Size(19, 15);
			label1.TabIndex = 5;
			label1.Text = "横";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(157, 34);
			label2.Name = "label2";
			label2.Size = new Size(19, 15);
			label2.TabIndex = 7;
			label2.Text = "縦";
			// 
			// lbInfo
			// 
			lbInfo.AutoSize = true;
			lbInfo.Location = new Point(220, 34);
			lbInfo.Name = "lbInfo";
			lbInfo.Size = new Size(38, 15);
			lbInfo.TabIndex = 9;
			lbInfo.Text = "label3";
			// 
			// btnExport
			// 
			btnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			btnExport.Enabled = false;
			btnExport.Location = new Point(408, 62);
			btnExport.Name = "btnExport";
			btnExport.Size = new Size(58, 28);
			btnExport.TabIndex = 3;
			btnExport.Text = "Export";
			btnExport.UseVisualStyleBackColor = true;
			// 
			// btnClear
			// 
			btnClear.Enabled = false;
			btnClear.Location = new Point(275, 62);
			btnClear.Name = "btnClear";
			btnClear.Size = new Size(58, 28);
			btnClear.TabIndex = 11;
			btnClear.Text = "Clear";
			btnClear.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(478, 344);
			Controls.Add(btnClear);
			Controls.Add(btnExport);
			Controls.Add(lbInfo);
			Controls.Add(label2);
			Controls.Add(label1);
			Controls.Add(numHeight);
			Controls.Add(numWidth);
			Controls.Add(btnRemove);
			Controls.Add(btnDown);
			Controls.Add(btnUp);
			Controls.Add(pictureBox1);
			Controls.Add(picList1);
			Controls.Add(menuStrip1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = menuStrip1;
			MinimumSize = new Size(360, 250);
			Name = "Form1";
			Text = "PicTiler - 画像タイルツール";
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)numWidth).EndInit();
			((System.ComponentModel.ISupportInitialize)numHeight).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private PicList picList1;
		private PictureBox pictureBox1;
		private Button btnUp;
		private MenuStrip menuStrip1;
		private ToolStripMenuItem fileToolStripMenuItem;
		private ToolStripMenuItem addImageMenu;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem exportMenu;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripMenuItem quitMenu;
		private Button btnDown;
		private Button btnRemove;
		private ToolStripMenuItem editToolStripMenuItem;
		private ToolStripMenuItem upMenu;
		private ToolStripMenuItem downMenu;
		private ToolStripMenuItem removeMenu;
		private NumericUpDown numWidth;
		private NumericUpDown numHeight;
		private Label label1;
		private Label label2;
		private Label lbInfo;
		private Button btnExport;
		private Button btnClear;
	}
}

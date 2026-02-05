namespace ColorSwitchEdit
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
			colorTableList1 = new ColorTableList();
			colorTablePropertyPanel1 = new ColorTablePropertyPanel();
			menuStrip1 = new MenuStrip();
			fileToolStripMenuItem = new ToolStripMenuItem();
			openMenu = new ToolStripMenuItem();
			saveMenu = new ToolStripMenuItem();
			toolStripSeparator1 = new ToolStripSeparator();
			exportPngMenu = new ToolStripMenuItem();
			importPngMenu = new ToolStripMenuItem();
			toolStripSeparator2 = new ToolStripSeparator();
			quitMenu = new ToolStripMenuItem();
			editToolStripMenuItem = new ToolStripMenuItem();
			moveUpMenu = new ToolStripMenuItem();
			moveDownMenu = new ToolStripMenuItem();
			btnUp = new Button();
			btnDown = new Button();
			btnOpen = new Button();
			btnSave = new Button();
			btnPNGExport = new Button();
			btnPNGImport = new Button();
			menuStrip1.SuspendLayout();
			SuspendLayout();
			// 
			// colorTableList1
			// 
			colorTableList1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			colorTableList1.Location = new Point(90, 105);
			colorTableList1.Name = "colorTableList1";
			colorTableList1.Size = new Size(242, 284);
			colorTableList1.TabIndex = 8;
			// 
			// colorTablePropertyPanel1
			// 
			colorTablePropertyPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			colorTablePropertyPanel1.Location = new Point(12, 32);
			colorTablePropertyPanel1.Name = "colorTablePropertyPanel1";
			colorTablePropertyPanel1.Size = new Size(320, 67);
			colorTablePropertyPanel1.TabIndex = 1;
			// 
			// menuStrip1
			// 
			menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem });
			menuStrip1.Location = new Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Size = new Size(344, 24);
			menuStrip1.TabIndex = 0;
			menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openMenu, saveMenu, toolStripSeparator1, exportPngMenu, importPngMenu, toolStripSeparator2, quitMenu });
			fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			fileToolStripMenuItem.Size = new Size(37, 20);
			fileToolStripMenuItem.Text = "File";
			// 
			// openMenu
			// 
			openMenu.Name = "openMenu";
			openMenu.ShortcutKeys = Keys.Control | Keys.O;
			openMenu.Size = new Size(174, 22);
			openMenu.Text = "Open";
			// 
			// saveMenu
			// 
			saveMenu.Name = "saveMenu";
			saveMenu.ShortcutKeys = Keys.Control | Keys.S;
			saveMenu.Size = new Size(174, 22);
			saveMenu.Text = "Save";
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new Size(171, 6);
			// 
			// exportPngMenu
			// 
			exportPngMenu.Name = "exportPngMenu";
			exportPngMenu.ShortcutKeys = Keys.Control | Keys.E;
			exportPngMenu.Size = new Size(174, 22);
			exportPngMenu.Text = "Export PNG";
			// 
			// importPngMenu
			// 
			importPngMenu.Name = "importPngMenu";
			importPngMenu.ShortcutKeys = Keys.Control | Keys.I;
			importPngMenu.Size = new Size(174, 22);
			importPngMenu.Text = "Import PNG";
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new Size(171, 6);
			// 
			// quitMenu
			// 
			quitMenu.Name = "quitMenu";
			quitMenu.ShortcutKeys = Keys.Control | Keys.Q;
			quitMenu.Size = new Size(174, 22);
			quitMenu.Text = "Quit";
			// 
			// editToolStripMenuItem
			// 
			editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { moveUpMenu, moveDownMenu });
			editToolStripMenuItem.Name = "editToolStripMenuItem";
			editToolStripMenuItem.Size = new Size(39, 20);
			editToolStripMenuItem.Text = "Edit";
			// 
			// moveUpMenu
			// 
			moveUpMenu.Name = "moveUpMenu";
			moveUpMenu.ShortcutKeys = Keys.Control | Keys.Up;
			moveUpMenu.Size = new Size(202, 22);
			moveUpMenu.Text = "Move Up";
			// 
			// moveDownMenu
			// 
			moveDownMenu.Name = "moveDownMenu";
			moveDownMenu.ShortcutKeys = Keys.Control | Keys.Down;
			moveDownMenu.Size = new Size(202, 22);
			moveDownMenu.Text = "Move Down";
			// 
			// btnUp
			// 
			btnUp.FlatStyle = FlatStyle.Flat;
			btnUp.Location = new Point(12, 301);
			btnUp.Name = "btnUp";
			btnUp.Size = new Size(72, 36);
			btnUp.TabIndex = 6;
			btnUp.Text = "UP";
			btnUp.UseVisualStyleBackColor = true;
			// 
			// btnDown
			// 
			btnDown.FlatStyle = FlatStyle.Flat;
			btnDown.Location = new Point(12, 343);
			btnDown.Name = "btnDown";
			btnDown.Size = new Size(72, 36);
			btnDown.TabIndex = 7;
			btnDown.Text = "Down";
			btnDown.UseVisualStyleBackColor = true;
			// 
			// btnOpen
			// 
			btnOpen.FlatStyle = FlatStyle.Flat;
			btnOpen.Location = new Point(12, 105);
			btnOpen.Name = "btnOpen";
			btnOpen.Size = new Size(72, 36);
			btnOpen.TabIndex = 2;
			btnOpen.Text = "Open";
			btnOpen.UseVisualStyleBackColor = true;
			// 
			// btnSave
			// 
			btnSave.FlatStyle = FlatStyle.Flat;
			btnSave.Location = new Point(12, 147);
			btnSave.Name = "btnSave";
			btnSave.Size = new Size(72, 36);
			btnSave.TabIndex = 3;
			btnSave.Text = "Save";
			btnSave.UseVisualStyleBackColor = true;
			// 
			// btnPNGExport
			// 
			btnPNGExport.FlatStyle = FlatStyle.Flat;
			btnPNGExport.Location = new Point(12, 199);
			btnPNGExport.Name = "btnPNGExport";
			btnPNGExport.Size = new Size(72, 36);
			btnPNGExport.TabIndex = 4;
			btnPNGExport.Text = "PNGsave";
			btnPNGExport.UseVisualStyleBackColor = true;
			// 
			// btnPNGImport
			// 
			btnPNGImport.FlatStyle = FlatStyle.Flat;
			btnPNGImport.Location = new Point(12, 241);
			btnPNGImport.Name = "btnPNGImport";
			btnPNGImport.Size = new Size(72, 36);
			btnPNGImport.TabIndex = 5;
			btnPNGImport.Text = "PNGload";
			btnPNGImport.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(344, 401);
			Controls.Add(btnPNGImport);
			Controls.Add(btnPNGExport);
			Controls.Add(btnSave);
			Controls.Add(btnOpen);
			Controls.Add(btnDown);
			Controls.Add(btnUp);
			Controls.Add(colorTablePropertyPanel1);
			Controls.Add(colorTableList1);
			Controls.Add(menuStrip1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = menuStrip1;
			MaximumSize = new Size(360, 2000);
			MinimumSize = new Size(360, 440);
			Name = "Form1";
			Text = "ColorSwitchEdit";
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private ColorTableList colorTableList1;
		private ColorTablePropertyPanel colorTablePropertyPanel1;
		private MenuStrip menuStrip1;
		private ToolStripMenuItem fileToolStripMenuItem;
		private ToolStripMenuItem openMenu;
		private ToolStripMenuItem saveMenu;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripMenuItem exportPngMenu;
		private ToolStripMenuItem importPngMenu;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem quitMenu;
		private ToolStripMenuItem editToolStripMenuItem;
		private ToolStripMenuItem moveUpMenu;
		private ToolStripMenuItem moveDownMenu;
		private Button btnUp;
		private Button btnDown;
		private Button btnOpen;
		private Button btnSave;
		private Button btnPNGExport;
		private Button btnPNGImport;
	}
}

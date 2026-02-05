namespace outflagsUI
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
			outflagsList1 = new OutflagsList();
			textBox1 = new TextBox();
			numericUpDown1 = new NumericUpDown();
			outflagsSwitcher1 = new OutflagsSwitcher();
			menuStrip1 = new MenuStrip();
			fileToolStripMenuItem = new ToolStripMenuItem();
			quitToolStripMenuItem = new ToolStripMenuItem();
			editToolStripMenuItem = new ToolStripMenuItem();
			copyMenu = new ToolStripMenuItem();
			pasteMenu = new ToolStripMenuItem();
			toolStripSeparator1 = new ToolStripSeparator();
			clearAllChecksMenuItem = new ToolStripMenuItem();
			checkAllMenuItem = new ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
			menuStrip1.SuspendLayout();
			SuspendLayout();
			// 
			// outflagsList1
			// 
			outflagsList1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
			outflagsList1.DescriptionTextBox = textBox1;
			outflagsList1.FormattingEnabled = true;
			outflagsList1.IntegralHeight = false;
			outflagsList1.Items.AddRange(new object[] { "PF_OutFlag_KEEP_RESOURCE_OPEN", "PF_OutFlag_WIDE_TIME_INPUT", "PF_OutFlag_NON_PARAM_VARY", "PF_OutFlag_RESERVED6", "PF_OutFlag_SEQUENCE_DATA_NEEDS_FLATTENING", "PF_OutFlag_I_DO_DIALOG", "PF_OutFlag_USE_OUTPUT_EXTENT", "PF_OutFlag_SEND_DO_DIALOG", "PF_OutFlag_DISPLAY_ERROR_MESSAGE", "PF_OutFlag_I_EXPAND_BUFFER", "PF_OutFlag_PIX_INDEPENDENT", "PF_OutFlag_I_WRITE_INPUT_BUFFER", "PF_OutFlag_I_SHRINK_BUFFER", "PF_OutFlag_WORKS_IN_PLACE", "PF_OutFlag_RESERVED8", "PF_OutFlag_CUSTOM_UI", "PF_OutFlag_RESERVED7", "PF_OutFlag_REFRESH_UI", "PF_OutFlag_NOP_RENDER", "PF_OutFlag_I_USE_SHUTTER_ANGLE", "PF_OutFlag_I_USE_AUDIO", "PF_OutFlag_I_AM_OBSOLETE", "PF_OutFlag_FORCE_RERENDER", "PF_OutFlag_PiPL_OVERRIDES_OUTDATA_OUTFLAGS", "PF_OutFlag_I_HAVE_EXTERNAL_DEPENDENCIES", "PF_OutFlag_DEEP_COLOR_AWARE", "PF_OutFlag_SEND_UPDATE_PARAMS_UI", "PF_OutFlag_AUDIO_FLOAT_ONLY", "PF_OutFlag_AUDIO_IIR", "PF_OutFlag_I_SYNTHESIZE_AUDIO", "PF_OutFlag_AUDIO_EFFECT_TOO", "PF_OutFlag_AUDIO_EFFECT_ONLY", "PF_OutFlag_KEEP_RESOURCE_OPEN", "PF_OutFlag_WIDE_TIME_INPUT", "PF_OutFlag_NON_PARAM_VARY", "PF_OutFlag_RESERVED6", "PF_OutFlag_SEQUENCE_DATA_NEEDS_FLATTENING", "PF_OutFlag_I_DO_DIALOG", "PF_OutFlag_USE_OUTPUT_EXTENT", "PF_OutFlag_SEND_DO_DIALOG", "PF_OutFlag_DISPLAY_ERROR_MESSAGE", "PF_OutFlag_I_EXPAND_BUFFER", "PF_OutFlag_PIX_INDEPENDENT", "PF_OutFlag_I_WRITE_INPUT_BUFFER", "PF_OutFlag_I_SHRINK_BUFFER", "PF_OutFlag_WORKS_IN_PLACE", "PF_OutFlag_RESERVED8", "PF_OutFlag_CUSTOM_UI", "PF_OutFlag_RESERVED7", "PF_OutFlag_REFRESH_UI", "PF_OutFlag_NOP_RENDER", "PF_OutFlag_I_USE_SHUTTER_ANGLE", "PF_OutFlag_I_USE_AUDIO", "PF_OutFlag_I_AM_OBSOLETE", "PF_OutFlag_FORCE_RERENDER", "PF_OutFlag_PiPL_OVERRIDES_OUTDATA_OUTFLAGS", "PF_OutFlag_I_HAVE_EXTERNAL_DEPENDENCIES", "PF_OutFlag_DEEP_COLOR_AWARE", "PF_OutFlag_SEND_UPDATE_PARAMS_UI", "PF_OutFlag_AUDIO_FLOAT_ONLY", "PF_OutFlag_AUDIO_IIR", "PF_OutFlag_I_SYNTHESIZE_AUDIO", "PF_OutFlag_AUDIO_EFFECT_TOO", "PF_OutFlag_AUDIO_EFFECT_ONLY", "PF_OutFlag_KEEP_RESOURCE_OPEN", "PF_OutFlag_WIDE_TIME_INPUT", "PF_OutFlag_NON_PARAM_VARY", "PF_OutFlag_RESERVED6", "PF_OutFlag_SEQUENCE_DATA_NEEDS_FLATTENING", "PF_OutFlag_I_DO_DIALOG", "PF_OutFlag_USE_OUTPUT_EXTENT", "PF_OutFlag_SEND_DO_DIALOG", "PF_OutFlag_DISPLAY_ERROR_MESSAGE", "PF_OutFlag_I_EXPAND_BUFFER", "PF_OutFlag_PIX_INDEPENDENT", "PF_OutFlag_I_WRITE_INPUT_BUFFER", "PF_OutFlag_I_SHRINK_BUFFER", "PF_OutFlag_WORKS_IN_PLACE", "PF_OutFlag_RESERVED8", "PF_OutFlag_CUSTOM_UI", "PF_OutFlag_RESERVED7", "PF_OutFlag_REFRESH_UI", "PF_OutFlag_NOP_RENDER", "PF_OutFlag_I_USE_SHUTTER_ANGLE", "PF_OutFlag_I_USE_AUDIO", "PF_OutFlag_I_AM_OBSOLETE", "PF_OutFlag_FORCE_RERENDER", "PF_OutFlag_PiPL_OVERRIDES_OUTDATA_OUTFLAGS", "PF_OutFlag_I_HAVE_EXTERNAL_DEPENDENCIES", "PF_OutFlag_DEEP_COLOR_AWARE", "PF_OutFlag_SEND_UPDATE_PARAMS_UI", "PF_OutFlag_AUDIO_FLOAT_ONLY", "PF_OutFlag_AUDIO_IIR", "PF_OutFlag_I_SYNTHESIZE_AUDIO", "PF_OutFlag_AUDIO_EFFECT_TOO", "PF_OutFlag_AUDIO_EFFECT_ONLY", "PF_OutFlag_KEEP_RESOURCE_OPEN", "PF_OutFlag_WIDE_TIME_INPUT", "PF_OutFlag_NON_PARAM_VARY", "PF_OutFlag_RESERVED6", "PF_OutFlag_SEQUENCE_DATA_NEEDS_FLATTENING", "PF_OutFlag_I_DO_DIALOG", "PF_OutFlag_USE_OUTPUT_EXTENT", "PF_OutFlag_SEND_DO_DIALOG", "PF_OutFlag_DISPLAY_ERROR_MESSAGE", "PF_OutFlag_I_EXPAND_BUFFER", "PF_OutFlag_PIX_INDEPENDENT", "PF_OutFlag_I_WRITE_INPUT_BUFFER", "PF_OutFlag_I_SHRINK_BUFFER", "PF_OutFlag_WORKS_IN_PLACE", "PF_OutFlag_RESERVED8", "PF_OutFlag_CUSTOM_UI", "PF_OutFlag_RESERVED7", "PF_OutFlag_REFRESH_UI", "PF_OutFlag_NOP_RENDER", "PF_OutFlag_I_USE_SHUTTER_ANGLE", "PF_OutFlag_I_USE_AUDIO", "PF_OutFlag_I_AM_OBSOLETE", "PF_OutFlag_FORCE_RERENDER", "PF_OutFlag_PiPL_OVERRIDES_OUTDATA_OUTFLAGS", "PF_OutFlag_I_HAVE_EXTERNAL_DEPENDENCIES", "PF_OutFlag_DEEP_COLOR_AWARE", "PF_OutFlag_SEND_UPDATE_PARAMS_UI", "PF_OutFlag_AUDIO_FLOAT_ONLY", "PF_OutFlag_AUDIO_IIR", "PF_OutFlag_I_SYNTHESIZE_AUDIO", "PF_OutFlag_AUDIO_EFFECT_TOO", "PF_OutFlag_AUDIO_EFFECT_ONLY", "PF_OutFlag_KEEP_RESOURCE_OPEN", "PF_OutFlag_WIDE_TIME_INPUT", "PF_OutFlag_NON_PARAM_VARY", "PF_OutFlag_RESERVED6", "PF_OutFlag_SEQUENCE_DATA_NEEDS_FLATTENING", "PF_OutFlag_I_DO_DIALOG", "PF_OutFlag_USE_OUTPUT_EXTENT", "PF_OutFlag_SEND_DO_DIALOG", "PF_OutFlag_DISPLAY_ERROR_MESSAGE", "PF_OutFlag_I_EXPAND_BUFFER", "PF_OutFlag_PIX_INDEPENDENT", "PF_OutFlag_I_WRITE_INPUT_BUFFER", "PF_OutFlag_I_SHRINK_BUFFER", "PF_OutFlag_WORKS_IN_PLACE", "PF_OutFlag_RESERVED8", "PF_OutFlag_CUSTOM_UI", "PF_OutFlag_RESERVED7", "PF_OutFlag_REFRESH_UI", "PF_OutFlag_NOP_RENDER", "PF_OutFlag_I_USE_SHUTTER_ANGLE", "PF_OutFlag_I_USE_AUDIO", "PF_OutFlag_I_AM_OBSOLETE", "PF_OutFlag_FORCE_RERENDER", "PF_OutFlag_PiPL_OVERRIDES_OUTDATA_OUTFLAGS", "PF_OutFlag_I_HAVE_EXTERNAL_DEPENDENCIES", "PF_OutFlag_DEEP_COLOR_AWARE", "PF_OutFlag_SEND_UPDATE_PARAMS_UI", "PF_OutFlag_AUDIO_FLOAT_ONLY", "PF_OutFlag_AUDIO_IIR", "PF_OutFlag_I_SYNTHESIZE_AUDIO", "PF_OutFlag_AUDIO_EFFECT_TOO", "PF_OutFlag_AUDIO_EFFECT_ONLY", "PF_OutFlag_KEEP_RESOURCE_OPEN", "PF_OutFlag_WIDE_TIME_INPUT", "PF_OutFlag_NON_PARAM_VARY", "PF_OutFlag_RESERVED6", "PF_OutFlag_SEQUENCE_DATA_NEEDS_FLATTENING", "PF_OutFlag_I_DO_DIALOG", "PF_OutFlag_USE_OUTPUT_EXTENT", "PF_OutFlag_SEND_DO_DIALOG", "PF_OutFlag_DISPLAY_ERROR_MESSAGE", "PF_OutFlag_I_EXPAND_BUFFER", "PF_OutFlag_PIX_INDEPENDENT", "PF_OutFlag_I_WRITE_INPUT_BUFFER", "PF_OutFlag_I_SHRINK_BUFFER", "PF_OutFlag_WORKS_IN_PLACE", "PF_OutFlag_RESERVED8", "PF_OutFlag_CUSTOM_UI", "PF_OutFlag_RESERVED7", "PF_OutFlag_REFRESH_UI", "PF_OutFlag_NOP_RENDER", "PF_OutFlag_I_USE_SHUTTER_ANGLE", "PF_OutFlag_I_USE_AUDIO", "PF_OutFlag_I_AM_OBSOLETE", "PF_OutFlag_FORCE_RERENDER", "PF_OutFlag_PiPL_OVERRIDES_OUTDATA_OUTFLAGS", "PF_OutFlag_I_HAVE_EXTERNAL_DEPENDENCIES", "PF_OutFlag_DEEP_COLOR_AWARE", "PF_OutFlag_SEND_UPDATE_PARAMS_UI", "PF_OutFlag_AUDIO_FLOAT_ONLY", "PF_OutFlag_AUDIO_IIR", "PF_OutFlag_I_SYNTHESIZE_AUDIO", "PF_OutFlag_AUDIO_EFFECT_TOO", "PF_OutFlag_AUDIO_EFFECT_ONLY", "PF_OutFlag_KEEP_RESOURCE_OPEN", "PF_OutFlag_WIDE_TIME_INPUT", "PF_OutFlag_NON_PARAM_VARY", "PF_OutFlag_RESERVED6", "PF_OutFlag_SEQUENCE_DATA_NEEDS_FLATTENING", "PF_OutFlag_I_DO_DIALOG", "PF_OutFlag_USE_OUTPUT_EXTENT", "PF_OutFlag_SEND_DO_DIALOG", "PF_OutFlag_DISPLAY_ERROR_MESSAGE", "PF_OutFlag_I_EXPAND_BUFFER", "PF_OutFlag_PIX_INDEPENDENT", "PF_OutFlag_I_WRITE_INPUT_BUFFER", "PF_OutFlag_I_SHRINK_BUFFER", "PF_OutFlag_WORKS_IN_PLACE", "PF_OutFlag_RESERVED8", "PF_OutFlag_CUSTOM_UI", "PF_OutFlag_RESERVED7", "PF_OutFlag_REFRESH_UI", "PF_OutFlag_NOP_RENDER", "PF_OutFlag_I_USE_SHUTTER_ANGLE", "PF_OutFlag_I_USE_AUDIO", "PF_OutFlag_I_AM_OBSOLETE", "PF_OutFlag_FORCE_RERENDER", "PF_OutFlag_PiPL_OVERRIDES_OUTDATA_OUTFLAGS", "PF_OutFlag_I_HAVE_EXTERNAL_DEPENDENCIES", "PF_OutFlag_DEEP_COLOR_AWARE", "PF_OutFlag_SEND_UPDATE_PARAMS_UI", "PF_OutFlag_AUDIO_FLOAT_ONLY", "PF_OutFlag_AUDIO_IIR", "PF_OutFlag_I_SYNTHESIZE_AUDIO", "PF_OutFlag_AUDIO_EFFECT_TOO", "PF_OutFlag_AUDIO_EFFECT_ONLY", "PF_OutFlag_KEEP_RESOURCE_OPEN", "PF_OutFlag_WIDE_TIME_INPUT", "PF_OutFlag_NON_PARAM_VARY", "PF_OutFlag_RESERVED6", "PF_OutFlag_SEQUENCE_DATA_NEEDS_FLATTENING", "PF_OutFlag_I_DO_DIALOG", "PF_OutFlag_USE_OUTPUT_EXTENT", "PF_OutFlag_SEND_DO_DIALOG", "PF_OutFlag_DISPLAY_ERROR_MESSAGE", "PF_OutFlag_I_EXPAND_BUFFER", "PF_OutFlag_PIX_INDEPENDENT", "PF_OutFlag_I_WRITE_INPUT_BUFFER", "PF_OutFlag_I_SHRINK_BUFFER", "PF_OutFlag_WORKS_IN_PLACE", "PF_OutFlag_RESERVED8", "PF_OutFlag_CUSTOM_UI", "PF_OutFlag_RESERVED7", "PF_OutFlag_REFRESH_UI", "PF_OutFlag_NOP_RENDER", "PF_OutFlag_I_USE_SHUTTER_ANGLE", "PF_OutFlag_I_USE_AUDIO", "PF_OutFlag_I_AM_OBSOLETE", "PF_OutFlag_FORCE_RERENDER", "PF_OutFlag_PiPL_OVERRIDES_OUTDATA_OUTFLAGS", "PF_OutFlag_I_HAVE_EXTERNAL_DEPENDENCIES", "PF_OutFlag_DEEP_COLOR_AWARE", "PF_OutFlag_SEND_UPDATE_PARAMS_UI", "PF_OutFlag_AUDIO_FLOAT_ONLY", "PF_OutFlag_AUDIO_IIR", "PF_OutFlag_I_SYNTHESIZE_AUDIO", "PF_OutFlag_AUDIO_EFFECT_TOO", "PF_OutFlag_AUDIO_EFFECT_ONLY" });
			outflagsList1.Location = new Point(12, 74);
			outflagsList1.Name = "outflagsList1";
			outflagsList1.Size = new Size(385, 395);
			outflagsList1.SyncedNumericUpDown = numericUpDown1;
			outflagsList1.TabIndex = 0;
			// 
			// textBox1
			// 
			textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			textBox1.Location = new Point(403, 74);
			textBox1.Multiline = true;
			textBox1.Name = "textBox1";
			textBox1.ReadOnly = true;
			textBox1.ScrollBars = ScrollBars.Vertical;
			textBox1.Size = new Size(365, 395);
			textBox1.TabIndex = 2;
			// 
			// numericUpDown1
			// 
			numericUpDown1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			numericUpDown1.Location = new Point(403, 42);
			numericUpDown1.Maximum = new decimal(new int[] { -1, -1, -1, 0 });
			numericUpDown1.Name = "numericUpDown1";
			numericUpDown1.Size = new Size(365, 23);
			numericUpDown1.TabIndex = 1;
			numericUpDown1.TextAlign = HorizontalAlignment.Right;
			// 
			// outflagsSwitcher1
			// 
			outflagsSwitcher1.Location = new Point(12, 35);
			outflagsSwitcher1.Name = "outflagsSwitcher1";
			outflagsSwitcher1.Size = new Size(200, 30);
			outflagsSwitcher1.TabIndex = 4;
			// 
			// menuStrip1
			// 
			menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem });
			menuStrip1.Location = new Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Size = new Size(780, 24);
			menuStrip1.TabIndex = 3;
			menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { quitToolStripMenuItem });
			fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			fileToolStripMenuItem.Size = new Size(37, 20);
			fileToolStripMenuItem.Text = "File";
			// 
			// quitToolStripMenuItem
			// 
			quitToolStripMenuItem.Name = "quitToolStripMenuItem";
			quitToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Q;
			quitToolStripMenuItem.Size = new Size(139, 22);
			quitToolStripMenuItem.Text = "Quit";
			// 
			// editToolStripMenuItem
			// 
			editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { copyMenu, pasteMenu, toolStripSeparator1, clearAllChecksMenuItem, checkAllMenuItem });
			editToolStripMenuItem.Name = "editToolStripMenuItem";
			editToolStripMenuItem.Size = new Size(39, 20);
			editToolStripMenuItem.Text = "Edit";
			// 
			// copyMenu
			// 
			copyMenu.Name = "copyMenu";
			copyMenu.ShortcutKeys = Keys.Control | Keys.C;
			copyMenu.Size = new Size(229, 22);
			copyMenu.Text = "Copy";
			// 
			// pasteMenu
			// 
			pasteMenu.Name = "pasteMenu";
			pasteMenu.ShortcutKeys = Keys.Control | Keys.V;
			pasteMenu.Size = new Size(229, 22);
			pasteMenu.Text = "Paste";
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new Size(226, 6);
			// 
			// clearAllChecksMenuItem
			// 
			clearAllChecksMenuItem.Name = "clearAllChecksMenuItem";
			clearAllChecksMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.C;
			clearAllChecksMenuItem.Size = new Size(229, 22);
			clearAllChecksMenuItem.Text = "Clear All Checks";
			clearAllChecksMenuItem.Click += ClearAllChecksMenuItem_Click;
			// 
			// checkAllMenuItem
			// 
			checkAllMenuItem.Name = "checkAllMenuItem";
			checkAllMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.A;
			checkAllMenuItem.Size = new Size(229, 22);
			checkAllMenuItem.Text = "Check All";
			checkAllMenuItem.Click += CheckAllMenuItem_Click;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(780, 481);
			Controls.Add(outflagsSwitcher1);
			Controls.Add(textBox1);
			Controls.Add(numericUpDown1);
			Controls.Add(outflagsList1);
			Controls.Add(menuStrip1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = menuStrip1;
			Name = "Form1";
			Text = "OutFlags Viewer";
			((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private OutflagsList outflagsList1;
	private NumericUpDown numericUpDown1;
	private TextBox textBox1;
	private OutflagsSwitcher outflagsSwitcher1;
	private MenuStrip menuStrip1;
	private ToolStripMenuItem fileToolStripMenuItem;
	private ToolStripMenuItem quitToolStripMenuItem;
	private ToolStripMenuItem editToolStripMenuItem;
	private ToolStripMenuItem copyMenu;
	private ToolStripMenuItem pasteMenu;
	private ToolStripSeparator toolStripSeparator1;
	private ToolStripMenuItem clearAllChecksMenuItem;
	private ToolStripMenuItem checkAllMenuItem;
}
}

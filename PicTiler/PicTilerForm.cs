namespace PicTiler
{
	public partial class PicTilerForm : Form
	{
		private int tileSize = 48;
		public PicTilerForm()
		{
			InitializeComponent();
			picList1.Preview = pictureBox1;
			picList1.SelectedIndexChanged += PicList1_SelectedIndexChanged;
			numWidth.ValueChanged += (sender, e) => { CheckStatus(); };
			numHeight.ValueChanged += (sender, e) => { CheckStatus(); };
			btnUp.Click += (sender, e) => { picList1.ItemUp(); };
			btnDown.Click += (sender, e) => { picList1.ItemDown(); };
			btnRemove.Click += (sender, e) => { ItemRemove(); };
			removeMenu.Click += (sender, e) => { ItemRemove(); };
			upMenu.Click += (sender, e) => { picList1.ItemUp(); };
			downMenu.Click += (sender, e) => { picList1.ItemDown(); };
			removeMenu.Click += (sender, e) => { ItemRemove(); };
			addImageMenu.Click += (sender, e) => { AddImage(); };
			btnExport.Click += (sender, e) => { ExportImage(); };
			exportMenu.Click += (sender, e) => { ExportImage(); };
			btnClear.Click += (sender, e) => { ItemClear(); };
			
			// イベントハンドラを追加
			this.Load += PicTilerForm_Load;
			this.FormClosing += PicTilerForm_FormClosing;
			
			CheckStatus();
		}

		private void PicTilerForm_Load(object? sender, EventArgs e)
		{
			// 設定を復元
			LoadSettings();
		}

		private void PicTilerForm_FormClosing(object? sender, FormClosingEventArgs e)
		{
			// 設定を保存
			SaveSettings();
		}

		private void LoadSettings()
		{
			try
			{
				var settings = Properties.Settings.Default;

				// フォームの位置とサイズを復元
				if (settings.FormLocation != null && settings.FormLocation != System.Drawing.Point.Empty)
				{
					// 画面外に表示されないようチェック
					if (IsVisibleOnAnyScreen(settings.FormLocation))
					{
						this.StartPosition = FormStartPosition.Manual;
						this.Location = settings.FormLocation;
					}
				}

				if (settings.FormSize != null && settings.FormSize.Width > 0 && settings.FormSize.Height > 0)
				{
					this.Size = settings.FormSize;
				}

				// NumericUpDownの値を復元
				if (settings.NumWidthValue >= numWidth.Minimum && settings.NumWidthValue <= numWidth.Maximum)
				{
					numWidth.Value = settings.NumWidthValue;
				}

				if (settings.NumHeightValue >= numHeight.Minimum && settings.NumHeightValue <= numHeight.Maximum)
				{
					numHeight.Value = settings.NumHeightValue;
				}
			}
			catch
			{
				// 設定の読み込みに失敗した場合はデフォルト値を使用
			}
		}

		private void SaveSettings()
		{
			try
			{
				var settings = Properties.Settings.Default;

				// フォームの位置とサイズを保存
				settings.FormLocation = this.Location;
				settings.FormSize = this.Size;

				// NumericUpDownの値を保存
				settings.NumWidthValue = numWidth.Value;
				settings.NumHeightValue = numHeight.Value;

				settings.Save();
			}
			catch
			{
				// 設定の保存に失敗した場合は無視
			}
		}

		private bool IsVisibleOnAnyScreen(System.Drawing.Point location)
		{
			foreach (Screen screen in Screen.AllScreens)
			{
				if (screen.WorkingArea.Contains(location))
				{
					return true;
				}
			}
			return false;
		}

		public void ItemRemove()
		{
			int si = picList1.SelectedIndex;
			if (si >= 0)
			{
				DialogResult result = MessageBox.Show(
						"削除してもよろしいですか？",
						"確認",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Question
					);

				if (result == DialogResult.Yes)
				{
					picList1.RemoveAt(si);
					CheckStatus();
				}

			}
		}
		public void ItemClear()
		{
			int si = picList1.Items.Count;
			if (si >= 0)
			{
				DialogResult result = MessageBox.Show(
						"全削除してもよろしいですか？",
						"確認",
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Question
					);

				if (result == DialogResult.Yes)
				{
					picList1.Clear();
					CheckStatus();
				}

			}
		}
		private void PicList1_SelectedIndexChanged(object? sender, EventArgs e)
		{
			CheckStatus();
		}
		private void CheckStatus()
		{
			int index = picList1.SelectedIndex;
			int cnt = picList1.Items.Count;
			downMenu.Enabled =
			btnDown.Enabled = (index >= 0 && index < cnt - 1);
			upMenu.Enabled =
			btnUp.Enabled = (index > 0 && index < cnt);
			exportMenu.Enabled =
			removeMenu.Enabled =
			btnRemove.Enabled = (index >= 0);
			exportMenu.Enabled = (cnt > 0);
			btnExport.Enabled = (cnt > 0);
			btnClear.Enabled = (cnt > 0);
			string status = $"Total:{cnt}({numWidth.Value * numHeight.Value}) W:{numWidth.Value * tileSize},H:{numHeight.Value * tileSize}";
			lbInfo.Text = status;
		}
		public bool AddImage()
		{
			bool result = false;
			using (OpenFileDialog ofd = new OpenFileDialog())
			{
				ofd.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tiff;*.tga";
				ofd.Multiselect = true;
				if (ofd.ShowDialog() != DialogResult.OK)
					return false;
				result = true;
				foreach (string path in ofd.FileNames)
				{
					if (!picList1.AddImageFile(path))
						result = false;
				}
			}
			return result;
		}
		public bool ExportImage()
		{
			bool result = false;
			using (SaveFileDialog sfd = new SaveFileDialog())
			{
				sfd.Filter = "PNG Image|*.png|Targa Image|*.tga";
				if (sfd.ShowDialog() != DialogResult.OK)
					return false;
				result = true;
				string path = sfd.FileName;
				result = picList1.ExportGridImage(tileSize, (int)numWidth.Value, (int)numHeight.Value, path);
			}
			return result;
		}
	}
}

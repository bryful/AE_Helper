namespace PicTiler
{
	public partial class Form1 : Form
	{
		public Form1()
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

			CheckStatus();
		}
		public void ItemRemove()
		{
			int si = picList1.SelectedIndex;
			if (si >= 0)
			{
				DialogResult result = MessageBox.Show(
						"íœ‚µ‚Ä‚à‚æ‚ë‚µ‚¢‚Å‚·‚©H",
						"Šm”F",
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

			string status = $"Total:{cnt}({numWidth.Value * numHeight.Value}) W:{numWidth.Value * 32},H:{numHeight.Value * 32}";
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
				result = picList1.ExportGridImage(32, (int)numWidth.Value, (int)numHeight.Value, path);
			}
			return result;
		}
	}
}

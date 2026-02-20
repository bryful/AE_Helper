using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms.VisualStyles;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace PicTiler
{
	public class PicList : ListBox
	{
		private PictureBox? m_pictureBox = null;
		
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(true)]
		public PictureBox? Preview
		{
			get => m_pictureBox;
			set
			{
				// 既存のイベントハンドラを削除
				if (m_pictureBox != null)
				{
					this.SelectedIndexChanged -= PicList_SelectedIndexChanged;
				}
				
				m_pictureBox = value;
				
				// 新しいPictureBoxにイベントハンドラを登録
				if (m_pictureBox != null)
				{
					this.SelectedIndexChanged += PicList_SelectedIndexChanged;
				}
			}
		}
		private List<string> imagePaths = new List<string>();
		private readonly string[] supportedExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tga" };

		public PicList()
		{
			InitializeComponent();
			this.AllowDrop = true;
			this.DragEnter += PicList_DragEnter;
			this.DragDrop += PicList_DragDrop;
		}

		private void PicList_SelectedIndexChanged(object? sender, EventArgs e)
		{
			if (m_pictureBox == null || this.SelectedIndex < 0 || this.SelectedIndex >= imagePaths.Count)
			{
				return;
			}

			try
			{
				string imagePath = imagePaths[this.SelectedIndex];
				
				// 既存の画像を破棄
				if (m_pictureBox.Image != null)
				{
					var oldImage = m_pictureBox.Image;
					m_pictureBox.Image = null;
					oldImage.Dispose();
				}

			// PictureBoxの設定
			m_pictureBox.SizeMode = PictureBoxSizeMode.Zoom; // 縮小表示
			m_pictureBox.BackColor = System.Drawing.Color.FromArgb(64, 64, 64); // 濃いグレー

				// ImageSharpで画像を読み込み
				using (var image = SixLabors.ImageSharp.Image.Load<Rgba32>(imagePath))
				{
					// ImageSharpからSystem.Drawing.Bitmapに変換
					using (var memoryStream = new MemoryStream())
					{
						image.SaveAsPng(memoryStream);
						memoryStream.Position = 0;
						m_pictureBox.Image = new Bitmap(memoryStream);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"画像の読み込みに失敗しました：{ex.Message}",
					"エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// PicList
			// 
			this.FormattingEnabled = true;
			this.ItemHeight = 20;
			this.Location = new System.Drawing.Point(0, 0);
			this.Name = "PicList";
			this.Size = new System.Drawing.Size(550, 394);
			this.TabIndex = 0;
			this.ResumeLayout(false);
		}

		private void PicList_DragEnter(object? sender, DragEventArgs e)
		{
			if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		private void PicList_DragDrop(object? sender, DragEventArgs e)
		{
			if (e.Data == null) return;

			var droppedItems = e.Data.GetData(DataFormats.FileDrop) as string[];
			if (droppedItems == null || droppedItems.Length == 0) return;

			int addedCount = 0;

			foreach (var item in droppedItems)
			{
				if (Directory.Exists(item))
				{
					if (AddImagesFromFolder(item))
					{
						addedCount++;
					}
				}
				else if (File.Exists(item))
				{
					var extension = Path.GetExtension(item).ToLower();
					if (supportedExtensions.Contains(extension))
					{
						if (AddImageFile(item))
						{
							addedCount++;
						}
					}
				}
			}
		}

		public void Clear()
		{
			base.Items.Clear();
			imagePaths.Clear();
			
			// PictureBoxもクリア
			if (m_pictureBox?.Image != null)
			{
				var oldImage = m_pictureBox.Image;
				m_pictureBox.Image = null;
				oldImage.Dispose();
			}
		}
		public void RemoveAt(int index)
		{
			if (index >= 0 && index < imagePaths.Count)
			{
				imagePaths.RemoveAt(index);
				base.Items.RemoveAt(index);
			}
		}
		public string? GetImagePath(int index)
		{
			if (index >= 0 && index < imagePaths.Count)
			{
				return imagePaths[index];
			}
			return null;
		}
		public List<string> GetAllImagePaths()
		{
			return new List<string>(imagePaths);
		}
		public void SwapItem(int idx0,int idx1)
		{
			if(idx0<0 || idx0 >= imagePaths.Count || idx1 < 0 || idx1 >= imagePaths.Count|| idx0==idx1) 
				return;

			string tempPath = imagePaths[idx0];
			imagePaths[idx0] = imagePaths[idx1];
			imagePaths[idx1] = tempPath;
			tempPath = (string)this.Items[idx0];
			this.Items[idx0] = this.Items[idx1];
			this.Items[idx1] = tempPath;
		}
		public void ItemUp()
		{
			int idx = this.SelectedIndex;
			if (idx > 0)
			{
				SwapItem(idx, idx - 1);
				this.SelectedIndex = idx - 1;
			}
		}
		public void ItemDown()
		{
			int idx = this.SelectedIndex;
			if (idx < this.Items.Count-1 && idx>=0)
			{
				SwapItem(idx, idx + 1);
				this.SelectedIndex = idx + 1;
			}
		}
		public void ItemRemove()
		{
			int idx = this.SelectedIndex;
			if (idx >= 0)
			{
				RemoveAt(idx);
				this.SelectedIndex = Math.Min(idx, this.Items.Count - 1);
			}
		}
		public bool AddImageFile(string filePath)
		{
			bool ret = false;
			if (filePath == null || filePath == "") return ret;

			if (!imagePaths.Contains(filePath))
			{
				try
				{
					using (var image = SixLabors.ImageSharp.Image.Load(filePath))
					{
						imagePaths.Add(filePath);
						this.Items.Add(Path.GetFileName(filePath));
						this.SelectedIndex = this.Items.Count - 1;
						this.Invalidate();
						ret = true;
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"画像ファイルの読み込みに失敗しました：{filePath}\n{ex.Message}",
						"エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			return ret;
		}
		public bool AddImagesFromFolder(string folderPath)
		{
			int cnt = 0;
			try
			{
				var imageFiles = Directory.GetFiles(folderPath)
					.Where(file => supportedExtensions.Contains(Path.GetExtension(file).ToLower()))
					.OrderBy(file => file);


				foreach (var imageFile in imageFiles)
				{
					if (AddImageFile(imageFile))
					{
						cnt++;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"フォルダの読み込み中にエラーが発生しました：{ex.Message}",
					"エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return cnt > 0;
		}
		// 使用例
		/*
		var picList = new PicTiler.PicList();
		var squareImage = picList.CreateSquareImage("image.tga", 32);
		// 後で保存する場合
		squareImage.SaveAsPng("output.png");
		squareImage.Dispose();
		*/
		public Image<Rgba32> CreateSquareImage(string filePath, int size)
		{
			if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
			{
				throw new ArgumentException("有効なファイルパスを指定してください", nameof(filePath));
			}

			if (size <= 0)
			{
				throw new ArgumentException("サイズは正の値を指定してください", nameof(size));
			}

			var sourceImage = SixLabors.ImageSharp.Image.Load<Rgba32>(filePath);

			var resultImage = new Image<Rgba32>(size, size);

			resultImage.Mutate(ctx =>
			{
				int sourceWidth = sourceImage.Width;
				int sourceHeight = sourceImage.Height;

				float scaleX = (float)size / sourceWidth;
				float scaleY = (float)size / sourceHeight;
				float scale = Math.Max(scaleX, scaleY);

				int scaledWidth = (int)(sourceWidth * scale);
				int scaledHeight = (int)(sourceHeight * scale);

				var resizedImage = sourceImage.Clone(x => x.Resize(scaledWidth, scaledHeight));

				int cropX = (scaledWidth - size) / 2;
				int cropY = (scaledHeight - size) / 2;

				ctx.DrawImage(resizedImage, new SixLabors.ImageSharp.Point(-cropX, -cropY), 1f);

				resizedImage.Dispose();
			});

			sourceImage.Dispose();

			return resultImage;
		}

		/// <summary>
		/// リストに登録された画像をグリッド状に配置した画像ファイルを生成
		/// </summary>
		/// <param name="sz">各画像のサイズ（縦横同じ）</param>
		/// <param name="w">横に並べる個数</param>
		/// <param name="h">縦に並べる個数</param>
		/// <param name="name">保存するファイルパス（拡張子で形式を判定: .png, .tga）</param>
		/// <returns>成功した場合true</returns>
		public bool ExportGridImage(int sz, int w, int h, string name)
		{
			if (sz <= 0 || w <= 0 || h <= 0)
			{
				MessageBox.Show("サイズと個数は正の値を指定してください。", 
					"エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			if (string.IsNullOrEmpty(name))
			{
				MessageBox.Show("保存先のファイル名を指定してください。", 
					"エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			if (imagePaths.Count == 0)
			{
				MessageBox.Show("画像が登録されていません。", 
					"エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			try
			{
				// グリッド画像のサイズ
				int gridWidth = sz * w;
				int gridHeight = sz * h;
				
				// 拡張子から保存形式を判定
				string extension = Path.GetExtension(name).ToLower();
				bool isTga = (extension == ".tga");
				bool isPng = (extension == ".png");

				if (!isTga && !isPng)
				{
					MessageBox.Show("サポートされていない形式です。\n.png または .tga を指定してください。", 
						"エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				// 結果画像を作成（透明背景）
				using (var resultImage = new Image<Rgba32>(gridWidth, gridHeight))
				{
					// 全グリッドセルを処理
					int totalCells = w * h;

					for (int i = 0; i < totalCells; i++)
					{
						// 画像のインデックスを循環させる（足りない場合は繰り返し）
						int imageIndex = i % imagePaths.Count;
						string imagePath = imagePaths[imageIndex];
						
						// グリッド上の位置を計算
						int gridX = (i % w) * sz;
						int gridY = (i / w) * sz;

						// 画像を正方形にリサイズ
						using (var squareImage = CreateSquareImage(imagePath, sz))
						{
							// グリッドに配置
							resultImage.Mutate(ctx =>
							{
								ctx.DrawImage(squareImage, 
									new SixLabors.ImageSharp.Point(gridX, gridY), 1f);
							});
						}
					}

					// ディレクトリが存在しない場合は作成
					string? directory = Path.GetDirectoryName(name);
					if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
					{
						Directory.CreateDirectory(directory);
					}

					// 形式に応じて保存
					if (isTga)
					{
						resultImage.SaveAsTga(name);
					}
					else // isPng
					{
						resultImage.SaveAsPng(name);
					}
				}

				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"画像の生成に失敗しました：{ex.Message}", 
					"エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}
	}
}



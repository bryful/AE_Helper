using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace outflagsUI
{
	/// <summary>
	/// アプリケーション設定を保存・復元するクラス
	/// </summary>
	public class ApplicationSettings
	{
		private static readonly string SettingsFilePath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			"OutflagsUI",
			"settings.json"
		);

		/// <summary>
		/// フォームのX座標
		/// </summary>
		[JsonPropertyName("formX")]
		public int FormX { get; set; } = 100;

		/// <summary>
		/// フォームのY座標
		/// </summary>
		[JsonPropertyName("formY")]
		public int FormY { get; set; } = 100;

		/// <summary>
		/// フォームの幅
		/// </summary>
		[JsonPropertyName("formWidth")]
		public int FormWidth { get; set; } = 800;

		/// <summary>
		/// フォームの高さ
		/// </summary>
		[JsonPropertyName("formHeight")]
		public int FormHeight { get; set; } = 600;

		/// <summary>
		/// フォームの最大化状態
		/// </summary>
		[JsonPropertyName("formMaximized")]
		public bool FormMaximized { get; set; } = false;

		/// <summary>
		/// アクティブなフラグタイプ
		/// </summary>
		[JsonPropertyName("activeType")]
		public OutflagsListType ActiveType { get; set; } = OutflagsListType.OutFlags1;

		/// <summary>
		/// OutFlags1の値
		/// </summary>
		[JsonPropertyName("outFlags1Value")]
		public ulong OutFlags1Value { get; set; } = 0;

		/// <summary>
		/// OutFlags2の値
		/// </summary>
		[JsonPropertyName("outFlags2Value")]
		public ulong OutFlags2Value { get; set; } = 0;

		/// <summary>
		/// 設定をファイルから読み込む
		/// </summary>
		public static ApplicationSettings Load()
		{
			try
			{
				if (!File.Exists(SettingsFilePath))
				{
					return new ApplicationSettings();
				}

				string json = File.ReadAllText(SettingsFilePath);
				var settings = JsonSerializer.Deserialize<ApplicationSettings>(json);
				return settings ?? new ApplicationSettings();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"設定の読み込みに失敗しました: {ex.Message}");
				return new ApplicationSettings();
			}
		}

		/// <summary>
		/// 設定をファイルに保存する
		/// </summary>
		public void Save()
		{
			try
			{
				string directory = Path.GetDirectoryName(SettingsFilePath) ?? "";
				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}

				var options = new JsonSerializerOptions
				{
					WriteIndented = true
				};

				string json = JsonSerializer.Serialize(this, options);
				File.WriteAllText(SettingsFilePath, json);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"設定の保存に失敗しました: {ex.Message}");
			}
		}

		/// <summary>
		/// フォームの位置とサイズを設定から適用
		/// </summary>
		public void ApplyToForm(Form form)
		{
			// 画面内に収まるかチェック
			Rectangle workingArea = Screen.PrimaryScreen?.WorkingArea ?? new Rectangle(0, 0, 1024, 768);
			
			int x = Math.Max(workingArea.Left, Math.Min(FormX, workingArea.Right - 200));
			int y = Math.Max(workingArea.Top, Math.Min(FormY, workingArea.Bottom - 200));
			int width = Math.Max(400, Math.Min(FormWidth, workingArea.Width));
			int height = Math.Max(300, Math.Min(FormHeight, workingArea.Height));

			form.StartPosition = FormStartPosition.Manual;
			form.Location = new Point(x, y);
			form.Size = new Size(width, height);

			if (FormMaximized)
			{
				form.WindowState = FormWindowState.Maximized;
			}
		}

		/// <summary>
		/// フォームの現在の位置とサイズを設定に保存
		/// </summary>
		public void CaptureFromForm(Form form)
		{
			if (form.WindowState == FormWindowState.Normal)
			{
				FormX = form.Location.X;
				FormY = form.Location.Y;
				FormWidth = form.Size.Width;
				FormHeight = form.Size.Height;
				FormMaximized = false;
			}
			else if (form.WindowState == FormWindowState.Maximized)
			{
				FormMaximized = true;
				// RestoreBoundsを使用して通常時のサイズを保存
				if (form.RestoreBounds != Rectangle.Empty)
				{
					FormX = form.RestoreBounds.X;
					FormY = form.RestoreBounds.Y;
					FormWidth = form.RestoreBounds.Width;
					FormHeight = form.RestoreBounds.Height;
				}
			}
		}
	}
}

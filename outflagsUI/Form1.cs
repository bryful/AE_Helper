namespace outflagsUI
{
	public partial class Form1 : Form
	{
		private ApplicationSettings settings;

		public Form1()
		{
			InitializeComponent();

			// 設定を読み込み
			settings = ApplicationSettings.Load();

			// フォームのLoad イベントで設定を適用（InitializeComponent後に実行）
			this.Load += Form1_Load;
			this.FormClosing += Form1_FormClosing;

			copyMenu.Click+=(s, e) =>
			{
				if (outflagsList1 != null)
				{
					Clipboard.SetText($"{outflagsList1.FlagsValue}");
				}
			};
			pasteMenu.Click += (s, e) =>
			{
				if (outflagsList1 != null)
				{
					if (ulong.TryParse(Clipboard.GetText(), out ulong value))
					{
						outflagsList1.FlagsValue = value;
					}
				}
			};
		}

		private void Form1_Load(object? sender, EventArgs e)
		{
			// フォームの位置とサイズを復元
			settings.ApplyToForm(this);

			// OutflagsListTypeを復元
			if (outflagsSwitcher1 != null)
			{
				outflagsSwitcher1.CurrentType = settings.ActiveType;
			}

			// FlagsValuesを復元
			if (outflagsList1 != null)
			{
				outflagsList1.FlagsValues = new ulong[] 
				{ 
					settings.OutFlags1Value, 
					settings.OutFlags2Value 
				};
			}
		}

		private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
		{
			// 現在のフォームの状態を保存
			settings.CaptureFromForm(this);

			// OutflagsListTypeを保存
			if (outflagsSwitcher1 != null)
			{
				settings.ActiveType = outflagsSwitcher1.CurrentType;
			}

			// FlagsValuesを保存
			if (outflagsList1 != null)
			{
				var values = outflagsList1.FlagsValues;
				if (values != null && values.Length >= 2)
				{
					settings.OutFlags1Value = values[0];
					settings.OutFlags2Value = values[1];
				}
			}

		// 設定をファイルに保存
		settings.Save();
	}

	private void ClearAllChecksMenuItem_Click(object? sender, EventArgs e)
	{
		outflagsList1?.ClearAllChecks();
	}

	private void CheckAllMenuItem_Click(object? sender, EventArgs e)
	{
		outflagsList1?.CheckAll();
	}
}
}

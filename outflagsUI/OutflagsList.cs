using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace outflagsUI
{
	public enum OutflagsListType
	{
		OutFlags1,
		OutFlags2
	}
	public class OutflagsList : CheckedListBox
	{
	private AfterEffectFlagsManager manager = new();
	private OutflagsListType m_flagType = OutflagsListType.OutFlags1;
	private ulong [] m_flagsValues = new ulong[2];
	private bool isUpdatingChecks = false; // イベントループを防ぐフラグ
	private NumericUpDown? m_numericUpDown = null;
	private TextBox? m_descriptionTextBox = null;

		[Category("outflags"), Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ulong[] FlagsValues
		{
			get { return m_flagsValues; }
			set
			{
				m_flagsValues = value;
				ApplyFlagsValue();
			}
		}
		[Category("outflags"), Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ulong FlagsValue
		{
			get
			{
				if (m_flagType == OutflagsListType.OutFlags1)
				{
					return m_flagsValues[0];
				}
				else
				{
					return m_flagsValues[1];
				}
			}
			set
			{
				if (m_flagType == OutflagsListType.OutFlags1)
				{
					m_flagsValues[0] = value;
				}
				else
				{
					m_flagsValues[1] = value;
				}
				ApplyFlagsValue();
			}
		}

	/// <summary>
	/// FlagsValueと同期するNumericUpDownコントロール
	/// </summary>
	[Category("outflags"), Browsable(true)]
	[DefaultValue(null)]
	public NumericUpDown? SyncedNumericUpDown
	{
		get { return m_numericUpDown; }
		set
		{
			// 既存のイベントハンドラを削除
			if (m_numericUpDown != null)
			{
				m_numericUpDown.ValueChanged -= NumericUpDown_ValueChanged;
			}

			m_numericUpDown = value;

		// 新しいNumericUpDownに設定を適用
		if (m_numericUpDown != null)
		{
			m_numericUpDown.Hexadecimal = false;
			m_numericUpDown.Maximum = decimal.MaxValue;
			m_numericUpDown.Minimum = 0;
			m_numericUpDown.ValueChanged += NumericUpDown_ValueChanged;
			UpdateNumericUpDownValue();
		}
		}
	}

	/// <summary>
	/// 選択された項目の詳細情報を表示するTextBoxコントロール
	/// </summary>
	[Category("outflags"), Browsable(true)]
	[DefaultValue(null)]
	public TextBox? DescriptionTextBox
	{
		get { return m_descriptionTextBox; }
		set
		{
			m_descriptionTextBox = value;

			// TextBoxの設定を適用
			if (m_descriptionTextBox != null)
			{
				m_descriptionTextBox.Multiline = true;
				m_descriptionTextBox.ReadOnly = true;
				m_descriptionTextBox.ScrollBars = ScrollBars.Vertical;
			}
		}
	}

	/// <summary>
	/// SyncedNumericUpDownをシリアライズすべきかどうかを判断
	/// </summary>
	private bool ShouldSerializeSyncedNumericUpDown()
	{
		return m_numericUpDown != null;
	}

	/// <summary>
	/// DescriptionTextBoxをシリアライズすべきかどうかを判断
	/// </summary>
	private bool ShouldSerializeDescriptionTextBox()
	{
		return m_descriptionTextBox != null;
	}

	/// <summary>
	/// CheckOnClickをシリアライズすべきかどうかを判断
	/// デフォルト値がfalseなので、falseの場合はシリアライズしない
	/// </summary>
	private bool ShouldSerializeCheckOnClick()
	{
		return false; // 常にシリアライズしない
	}

	public OutflagsList()
	{
			
		
		this.CheckOnClick = false;
		this.FormattingEnabled = true;
		this.IntegralHeight = false;
		this.ItemHeight = 15;
		this.Location = new Point(0, 0);
		this.Name = "outflagsList";
		this.Size = new Size(150, 150);
		this.TabIndex = 0;

		// チェック状態変更イベントを登録
		this.ItemCheck += OutflagsList_ItemCheck;
		// 選択変更イベントを登録
		this.SelectedIndexChanged += OutflagsList_SelectedIndexChanged;
		manager.LoadFromJsonString(Properties.Resources.OutflagsList);
		m_flagsValues[0] = 0;
		m_flagsValues[1] = 0;
		FlagType = OutflagsListType.OutFlags1;
	}

		/// <summary>
		/// NumericUpDownの値変更イベントハンドラ
		/// </summary>
		private void NumericUpDown_ValueChanged(object? sender, EventArgs e)
		{
			if (isUpdatingChecks || m_numericUpDown == null) return;

			isUpdatingChecks = true;
			try
			{
				ulong newValue = (ulong)m_numericUpDown.Value;
				if (m_flagType == OutflagsListType.OutFlags1)
				{
					m_flagsValues[0] = newValue;
				}
				else
				{
					m_flagsValues[1] = newValue;
				}
				ApplyFlagsValueInternal();
			}
			finally
			{
				isUpdatingChecks = false;
			}
		}

	/// <summary>
	/// NumericUpDownの値を更新
	/// </summary>
	private void UpdateNumericUpDownValue()
	{
		if (m_numericUpDown == null) return;

		bool wasUpdating = isUpdatingChecks;
		isUpdatingChecks = true;
		try
		{
			ulong currentValue = m_flagType == OutflagsListType.OutFlags1 ? m_flagsValues[0] : m_flagsValues[1];
			m_numericUpDown.Value = (decimal)currentValue;
		}
		finally
		{
			isUpdatingChecks = wasUpdating;
		}
	}

		/// <summary>
		/// フラグ値からチェック状態を反映
		/// </summary>
		private void ApplyFlagsValue()
		{
			if (isUpdatingChecks) return; // 無限ループ防止

			isUpdatingChecks = true;
			try
			{
				ApplyFlagsValueInternal();
				UpdateNumericUpDownValue();
			}
			finally
			{
				isUpdatingChecks = false;
			}
		}

		/// <summary>
		/// フラグ値からチェック状態を反映（内部用）
		/// </summary>
		private void ApplyFlagsValueInternal()
		{
			ulong currentValue = m_flagType == OutflagsListType.OutFlags1 ? m_flagsValues[0] : m_flagsValues[1];
			
			for (int i = 0; i < this.Items.Count; i++)
			{
				OutFlagInfo? flagInfo = null;
				if (m_flagType == OutflagsListType.OutFlags1)
				{
					flagInfo = manager.GetOutFlag1ByName(this.Items[i].ToString() ?? string.Empty);
				}
				else
				{
					flagInfo = manager.GetOutFlag2ByName(this.Items[i].ToString() ?? string.Empty);
				}

				if (flagInfo != null)
				{
					bool shouldBeChecked = (currentValue & (1UL << flagInfo.Bit)) != 0;
					this.SetItemChecked(i, shouldBeChecked);
				}
			}
		}

		/// <summary>
		/// チェック状態からフラグ値を計算
		/// </summary>
		private void calculateFlagsValues()
		{
			if (isUpdatingChecks) return; // ApplyFlagsValue実行中は計算しない

			isUpdatingChecks = true;
			try
			{
				if (m_flagType == OutflagsListType.OutFlags1)
				{
					m_flagsValues[0] = 0;
				}
				else
				{
					m_flagsValues[1] = 0;
				}
				for (int i = 0; i < this.Items.Count; i++)
				{
					if (this.GetItemChecked(i))
					{
						OutFlagInfo? flagInfo = null;
						if (m_flagType == OutflagsListType.OutFlags1)
						{
							flagInfo = manager.GetOutFlag1ByName(this.Items[i].ToString() ?? string.Empty);
							if (flagInfo != null)
							{
								m_flagsValues[0] |= (1UL << flagInfo.Bit);
							}
						}
						else
						{
							flagInfo = manager.GetOutFlag2ByName(this.Items[i].ToString() ?? string.Empty);
							if (flagInfo != null)
							{
								m_flagsValues[1] |= (1UL << flagInfo.Bit);
							}
						}
					}
				}
				UpdateNumericUpDownValue();
			}
			finally
			{
				isUpdatingChecks = false;
			}
		}
	private void OutflagsList_ItemCheck(object? sender, ItemCheckEventArgs e)
	{
		// e.Index - チェックが変更された項目のインデックス
		// e.CurrentValue - 変更前の状態
		// e.NewValue - 変更後の状態（Checked, Unchecked, Indeterminate）

		// 変更後の処理を行う場合は、BeginInvokeを使用
		BeginInvoke(new Action(() =>
		{
			// ここで変更後の状態を取得できる
			bool isChecked = GetItemChecked(e.Index);
			object? item = Items[e.Index];
			calculateFlagsValues();
			// チェック状態が変更されたことを通知
			OnCheckedChanged(new CheckedChangedEventArgs(e.Index, item, isChecked));
		}));
	}

	/// <summary>
	/// 選択項目変更イベントハンドラ
	/// </summary>
	private void OutflagsList_SelectedIndexChanged(object? sender, EventArgs e)
	{
		UpdateDescriptionTextBox();
	}

	/// <summary>
	/// DescriptionTextBoxの内容を更新
	/// </summary>
	private void UpdateDescriptionTextBox()
	{
		if (m_descriptionTextBox == null || SelectedIndex < 0) return;

		string itemName = Items[SelectedIndex].ToString() ?? string.Empty;
		OutFlagInfo? flagInfo = null;

		if (m_flagType == OutflagsListType.OutFlags1)
		{
			flagInfo = manager.GetOutFlag1ByName(itemName);
		}
		else
		{
			flagInfo = manager.GetOutFlag2ByName(itemName);
		}

		if (flagInfo != null)
		{
			string description = string.IsNullOrWhiteSpace(flagInfo.Description) 
				? "(説明なし)" 
				: flagInfo.Description;
			description　= description.Replace("。", "。\r\n");


			string relevantCommands = string.IsNullOrWhiteSpace(flagInfo.RelevantCommands) 
				? "(関連コマンドなし)" 
				: flagInfo.RelevantCommands;
			m_descriptionTextBox.Text = $"【{flagInfo.Name}】\r\n\r\n" +
				$"Description:\r\n{description}\r\n\r\n" +
				$"Relevant Commands:\r\n{relevantCommands}";
		}
		else
		{
			m_descriptionTextBox.Text = string.Empty;
		}
	}

		// カスタムイベントを定義
		public event EventHandler<CheckedChangedEventArgs>? CheckedChanged;

		protected virtual void OnCheckedChanged(CheckedChangedEventArgs e)
		{
			CheckedChanged?.Invoke(this, e);
		}

		[Category("outflags"), Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[DefaultValue(OutflagsListType.OutFlags1)]
		public OutflagsListType FlagType
		{
			get { return m_flagType; }
			set
			{
				System.Diagnostics.Debug.WriteLine($"OutflagsList: FlagType setter called with {value} (current: {m_flagType})");
				m_flagType = value;
				if (m_flagType == OutflagsListType.OutFlags1)
				{
					System.Diagnostics.Debug.WriteLine("OutflagsList: Setting OutFlags1");
					SetOutFlags(manager.GetOutFlags1());
				}
				else
				{
					System.Diagnostics.Debug.WriteLine("OutflagsList: Setting OutFlags2");
					SetOutFlags(manager.GetOutFlags2());
				}
				ApplyFlagsValue(); // フラグタイプ変更時にも反映
			}
		}
	public void SetOutFlags(List<OutFlagInfo> flags)
	{
		this.Items.Clear();
		foreach (var flag in flags)
		{
			this.Items.Add(flag.Name);
		}
	}

	/// <summary>
	/// すべてのチェックをオフにする
	/// </summary>
	public void ClearAllChecks()
	{
		if (isUpdatingChecks) return;

		isUpdatingChecks = true;
		try
		{
			// すべてのアイテムのチェックを外す
			for (int i = 0; i < this.Items.Count; i++)
			{
				this.SetItemChecked(i, false);
			}

			// フラグ値をクリア
			if (m_flagType == OutflagsListType.OutFlags1)
			{
				m_flagsValues[0] = 0;
			}
			else
			{
				m_flagsValues[1] = 0;
			}

			// NumericUpDownを更新
			UpdateNumericUpDownValue();
		}
		finally
		{
			isUpdatingChecks = false;
		}
	}

	/// <summary>
	/// すべてのチェックをオンにする
	/// </summary>
	public void CheckAll()
	{
		if (isUpdatingChecks) return;

		isUpdatingChecks = true;
		try
		{
			ulong flagValue = 0;

			// すべてのアイテムをチェック
			for (int i = 0; i < this.Items.Count; i++)
			{
				this.SetItemChecked(i, true);

				// フラグ値を計算
				OutFlagInfo? flagInfo = null;
				if (m_flagType == OutflagsListType.OutFlags1)
				{
					flagInfo = manager.GetOutFlag1ByName(this.Items[i].ToString() ?? string.Empty);
				}
				else
				{
					flagInfo = manager.GetOutFlag2ByName(this.Items[i].ToString() ?? string.Empty);
				}

				if (flagInfo != null)
				{
					flagValue |= (1UL << flagInfo.Bit);
				}
			}

			// フラグ値を設定
			if (m_flagType == OutflagsListType.OutFlags1)
			{
				m_flagsValues[0] = flagValue;
			}
			else
			{
				m_flagsValues[1] = flagValue;
			}

			// NumericUpDownを更新
			UpdateNumericUpDownValue();
		}
		finally
		{
			isUpdatingChecks = false;
		}
	}
}

	// カスタムイベント引数クラス
	public class CheckedChangedEventArgs : EventArgs
	{
		public int Index { get; }
		public object? Item { get; }
		public bool IsChecked { get; }

		public CheckedChangedEventArgs(int index, object? item, bool isChecked)
		{
			Index = index;
			Item = item;
			IsChecked = isChecked;
		}
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace outflagsUI
{
	/// <summary>
	/// OutflagsListTypeを切り替えるためのカスタムコントロール
	/// </summary>
	public class OutflagsSwitcher : UserControl
	{
		private Button btnOutFlags1;
		private Button btnOutFlags2;
		private OutflagsList? m_outflagsList = null;
		private OutflagsListType m_currentType = OutflagsListType.OutFlags1;

		/// <summary>
		/// 連携するOutflagsListコントロール
		/// </summary>
		[Category("outflags"), Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public OutflagsList? SyncedOutflagsList
		{
			get { return m_outflagsList; }
			set
			{
				m_outflagsList = value;
				if (m_outflagsList != null)
				{
					// 現在のタイプをOutflagsListに反映
					m_outflagsList.FlagType = m_currentType;
				}
			}
		}

		/// <summary>
		/// 現在選択されているタイプ
		/// </summary>
		[Category("outflags"), Browsable(true)]
		[DefaultValue(OutflagsListType.OutFlags1)]
		public OutflagsListType CurrentType
		{
			get { return m_currentType; }
			set
			{
				if (m_currentType != value)
				{
					m_currentType = value;
					UpdateButtonStates();
					
					// OutflagsListに反映
					if (m_outflagsList != null)
					{
						m_outflagsList.FlagType = m_currentType;
					}

					// イベント発火
					OnTypeChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// タイプ変更イベント
		/// </summary>
		public event EventHandler? TypeChanged;

		protected virtual void OnTypeChanged(EventArgs e)
		{
			TypeChanged?.Invoke(this, e);
		}

		public OutflagsSwitcher()
		{
			InitializeComponent();
			UpdateButtonStates();
		}

		private void InitializeComponent()
		{
			btnOutFlags1 = new Button();
			btnOutFlags2 = new Button();
			SuspendLayout();

			// 
			// btnOutFlags1
			// 
			btnOutFlags1.FlatStyle = FlatStyle.Flat;
			btnOutFlags1.Location = new Point(0, 0);
			btnOutFlags1.Name = "btnOutFlags1";
			btnOutFlags1.Size = new Size(100, 30);
			btnOutFlags1.TabIndex = 0;
			btnOutFlags1.Text = "OutFlags1";
			btnOutFlags1.UseVisualStyleBackColor = true;
			btnOutFlags1.Click += BtnOutFlags1_Click;

			// 
			// btnOutFlags2
			// 
			btnOutFlags2.FlatStyle = FlatStyle.Flat;
			btnOutFlags2.Location = new Point(100, 0);
			btnOutFlags2.Name = "btnOutFlags2";
			btnOutFlags2.Size = new Size(100, 30);
			btnOutFlags2.TabIndex = 1;
			btnOutFlags2.Text = "OutFlags2";
			btnOutFlags2.UseVisualStyleBackColor = true;
			btnOutFlags2.Click += BtnOutFlags2_Click;

			// 
			// OutflagsSwitcher
			// 
			Controls.Add(btnOutFlags1);
			Controls.Add(btnOutFlags2);
			Name = "OutflagsSwitcher";
			Size = new Size(200, 30);
			ResumeLayout(false);
		}

		private void BtnOutFlags1_Click(object? sender, EventArgs e)
		{
			CurrentType = OutflagsListType.OutFlags1;
		}

		private void BtnOutFlags2_Click(object? sender, EventArgs e)
		{
			CurrentType = OutflagsListType.OutFlags2;
		}

		/// <summary>
		/// ボタンの表示状態を更新（押された状態を表現）
		/// </summary>
		private void UpdateButtonStates()
		{
			if (m_currentType == OutflagsListType.OutFlags1)
			{
				// OutFlags1が押された状態
				btnOutFlags1.BackColor = SystemColors.Highlight;
				btnOutFlags1.ForeColor = SystemColors.HighlightText;
				btnOutFlags1.FlatAppearance.BorderSize = 2;
				
				// OutFlags2は通常状態
				btnOutFlags2.BackColor = SystemColors.Control;
				btnOutFlags2.ForeColor = SystemColors.ControlText;
				btnOutFlags2.FlatAppearance.BorderSize = 1;
			}
			else
			{
				// OutFlags2が押された状態
				btnOutFlags2.BackColor = SystemColors.Highlight;
				btnOutFlags2.ForeColor = SystemColors.HighlightText;
				btnOutFlags2.FlatAppearance.BorderSize = 2;
				
				// OutFlags1は通常状態
				btnOutFlags1.BackColor = SystemColors.Control;
				btnOutFlags1.ForeColor = SystemColors.ControlText;
				btnOutFlags1.FlatAppearance.BorderSize = 1;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				btnOutFlags1?.Dispose();
				btnOutFlags2?.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}

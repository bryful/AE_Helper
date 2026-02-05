using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ColorSwitchEdit;

/// <summary>
/// ColorTableを可視化するカスタムコントロール
/// </summary>
public class ColorTableList : UserControl
{
    private ColorTable? _colorTable;
    private const int MaxItems = 32;
    private const int ColorBoxSize = 32;
    private const int ColorBoxSpacing = 8;
    private Font _textFont = new Font("Segoe UI", 9, FontStyle.Regular);
    private bool[] _checkedStates = new bool[MaxItems];
    private VScrollBar _vScrollBar;
    private int _selectedIndex = -1;
    private const int ItemHeight = 40;

    public ColorTableList()
    {
        DoubleBuffered = true;
        
        _vScrollBar = new VScrollBar
        {
            Dock = DockStyle.Right,
            SmallChange = ItemHeight,
            LargeChange = ItemHeight * 5
        };
        _vScrollBar.Scroll += VScrollBar_Scroll;
        
        Controls.Add(_vScrollBar);
        
        UpdateScrollBar();
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            if (_selectedIndex != value)
            {
                _selectedIndex = value;
                Invalidate();
            }
        }
    }

    /// <summary>
    /// スクロールバーのパラメータを更新
    /// </summary>
    private void UpdateScrollBar()
    {
        int totalHeight = ItemHeight * MaxItems;
        int clientHeight = Height;
        
        // 最大値 = 全体の高さ - 表示領域の高さ
        int maxScroll = Math.Max(0, totalHeight - clientHeight);
        
        _vScrollBar.Maximum = maxScroll + _vScrollBar.LargeChange - 1;
        _vScrollBar.Visible = maxScroll > 0;
    }

    private void VScrollBar_Scroll(object? sender, ScrollEventArgs e)
    {
        Invalidate();
    }

    protected override void OnSizeChanged(EventArgs e)
    {
        base.OnSizeChanged(e);
        UpdateScrollBar();
        Invalidate();
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public ColorTable? ColorTable
    {
        get => _colorTable;
        set
        {
            _colorTable = value;
            UpdateFromColorTable();
        }
    }

    /// <summary>
    /// チェック状態を取得
    /// </summary>
    private bool GetItemChecked(int index)
    {
        if (index < 0 || index >= MaxItems) return false;
        return _checkedStates[index];
    }

    /// <summary>
    /// チェック状態を設定
    /// </summary>
    private void SetItemChecked(int index, bool value)
    {
        if (index < 0 || index >= MaxItems) return;
        _checkedStates[index] = value;
    }

    /// <summary>
    /// ColorTableからコントロールを更新
    /// </summary>
    private void UpdateFromColorTable()
    {
        if (_colorTable == null) return;
        
        for (int i = 0; i < MaxItems; i++)
        {
            SetItemChecked(i, _colorTable.TurnonColors[i]);
        }
        
        Invalidate();
    }

    /// <summary>
    /// コントロールからColorTableを更新
    /// </summary>
    public void UpdateColorTable()
    {
        if (_colorTable == null) return;

        for (int i = 0; i < MaxItems; i++)
        {
            _colorTable.TurnonColors[i] = GetItemChecked(i);
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        int scrollOffset = _vScrollBar.Value;
        int y = -scrollOffset;
        int drawWidth = Width - (_vScrollBar.Visible ? _vScrollBar.Width : 0);

        for (int i = 0; i < MaxItems; i++)
        {
            // 表示領域外はスキップ
            if (y + ItemHeight < 0)
            {
                y += ItemHeight;
                continue;
            }
            if (y > Height)
                break;

            DrawItem(e.Graphics, i, new Rectangle(0, y, drawWidth, ItemHeight));
            y += ItemHeight;
        }
    }

    private void DrawItem(Graphics g, int index, Rectangle bounds)
    {
        // 背景
        using (var bgBrush = new SolidBrush(index == _selectedIndex ? SystemColors.Highlight : BackColor))
        {
            g.FillRectangle(bgBrush, bounds);
        }

        bool isEnabled = _colorTable != null && index < _colorTable.ActiveParamCount;
        bool isChecked = GetItemChecked(index);
        int x = bounds.Left + 2;
        int centerY = bounds.Top + (bounds.Height - ColorBoxSize) / 2;

        // チェックボックスを手動で描画
        var checkBoxSize = 16;
        var checkBoxY = bounds.Top + (bounds.Height - checkBoxSize) / 2;
        var checkBoxRect = new Rectangle(x, checkBoxY, checkBoxSize, checkBoxSize);
        
        CheckBoxState state = isEnabled 
            ? (isChecked ? CheckBoxState.CheckedNormal : CheckBoxState.UncheckedNormal)
            : (isChecked ? CheckBoxState.CheckedDisabled : CheckBoxState.UncheckedDisabled);
        
        CheckBoxRenderer.DrawCheckBox(g, checkBoxRect.Location, state);
        x += 22;

        // インデックス番号
        var foreColor = index == _selectedIndex ? SystemColors.HighlightText : (isEnabled ? ForeColor : Color.Gray);
        using (var brush = new SolidBrush(foreColor))
        {
            var indexText = index.ToString("D2");
            g.DrawString(indexText, _textFont, brush, x, bounds.Top + 10);
        }
        x += 35;

        // ColorTableがnullの場合はデフォルト色を使用
        Color oldColor = _colorTable != null ? _colorTable.OldColors[index] : Color.White;
        Color newColor = _colorTable != null ? _colorTable.NewColors[index] : Color.White;

        // Old Color
        var oldColorRect = new Rectangle(x, centerY, ColorBoxSize, ColorBoxSize);
        using (var brush = new SolidBrush(oldColor))
        {
            g.FillRectangle(brush, oldColorRect);
        }
        g.DrawRectangle(Pens.Black, oldColorRect);
        x += ColorBoxSize + ColorBoxSpacing;

        // 矢印
        using (var brush = new SolidBrush(index == _selectedIndex ? SystemColors.HighlightText : ForeColor))
        {
            g.DrawString("→", _textFont, brush, x, bounds.Top + 8);
        }
        x += 25;

        // New Color
        var newColorRect = new Rectangle(x, centerY, ColorBoxSize, ColorBoxSize);
        using (var brush = new SolidBrush(newColor))
        {
            g.FillRectangle(brush, newColorRect);
        }
        g.DrawRectangle(Pens.Black, newColorRect);

        // フォーカス
        if (index == _selectedIndex)
        {
            ControlPaint.DrawFocusRectangle(g, bounds);
        }
    }


    /// <summary>
    /// 色をダブルクリックで編集
    /// </summary>
    protected override void OnDoubleClick(EventArgs e)
    {
        base.OnDoubleClick(e);

        if (_colorTable == null || _selectedIndex < 0) return;

        var mousePos = PointToClient(Cursor.Position);
        int y = GetItemY(_selectedIndex);
        if (y < 0) return;

        int x = 59; // チェックボックス(22) + インデックス番号(35) + 2
        int centerY = y + (ItemHeight - ColorBoxSize) / 2;

        var oldColorRect = new Rectangle(x, centerY, ColorBoxSize, ColorBoxSize);
        var newColorRect = new Rectangle(x + ColorBoxSize + ColorBoxSpacing + 25, centerY, ColorBoxSize, ColorBoxSize);

        using (var colorDialog = new ColorDialog())
        {
            if (oldColorRect.Contains(mousePos))
            {
                colorDialog.Color = _colorTable.OldColors[_selectedIndex];
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    _colorTable.OldColors[_selectedIndex] = colorDialog.Color;
                    Invalidate();
                }
            }
            else if (newColorRect.Contains(mousePos))
            {
                colorDialog.Color = _colorTable.NewColors[_selectedIndex];
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    _colorTable.NewColors[_selectedIndex] = colorDialog.Color;
                    Invalidate();
                }
            }
        }
    }

    private int GetItemY(int index)
    {
        return index * ItemHeight - _vScrollBar.Value;
    }

    private int GetItemIndexAt(Point location)
    {
        int y = location.Y + _vScrollBar.Value;
        int index = y / ItemHeight;
        return (index >= 0 && index < MaxItems) ? index : -1;
    }

    /// <summary>
    /// マウスクリック時にチェックボックスエリアのみチェック処理を行う
    /// </summary>
    protected override void OnMouseDown(MouseEventArgs e)
    {
        base.OnMouseDown(e);

        if (e.Button != MouseButtons.Left) return;

        int index = GetItemIndexAt(e.Location);
        if (index == -1) return;

        // 選択
        SelectedIndex = index;

        // チェックボックスの領域（左から約20ピクセル）
        var checkBoxBounds = new Rectangle(2, 0, 18, ItemHeight);

        var localY = e.Y + _vScrollBar.Value - (index * ItemHeight);
        var localPoint = new Point(e.X, (int)localY);

        // チェックボックスエリアをクリックした場合のみチェック状態を切り替え
        if (checkBoxBounds.Contains(localPoint))
        {
            if (_colorTable != null && index < _colorTable.ActiveParamCount)
            {
                SetItemChecked(index, !GetItemChecked(index));
                UpdateColorTable();
                Invalidate();
            }
        }
    }

    /// <summary>
    /// マウスホイールでスクロール
    /// </summary>
    protected override void OnMouseWheel(MouseEventArgs e)
    {
        base.OnMouseWheel(e);

        int delta = -e.Delta / 120 * _vScrollBar.SmallChange;
        int newValue = Math.Max(_vScrollBar.Minimum, Math.Min(_vScrollBar.Maximum - _vScrollBar.LargeChange + 1, _vScrollBar.Value + delta));
        _vScrollBar.Value = newValue;
        Invalidate();
    }

    /// <summary>
    /// 選択項目を上に移動
    /// </summary>
    public void MoveSelectedItemUp()
    {
        if (_colorTable == null || SelectedIndex <= 0) return;

        int currentIndex = SelectedIndex;
        if (_colorTable.MoveUp(currentIndex))
        {
            // チェック状態も入れ替え
            (_checkedStates[currentIndex], _checkedStates[currentIndex - 1]) = 
                (_checkedStates[currentIndex - 1], _checkedStates[currentIndex]);

            // 選択位置を更新
            SelectedIndex = currentIndex - 1;
            Invalidate();
        }
    }

    /// <summary>
    /// 選択項目を下に移動
    /// </summary>
    public void MoveSelectedItemDown()
    {
        if (_colorTable == null || SelectedIndex < 0 || SelectedIndex >= MaxItems - 1) return;

        int currentIndex = SelectedIndex;
        if (_colorTable.MoveDown(currentIndex))
        {
            // チェック状態も入れ替え
            (_checkedStates[currentIndex], _checkedStates[currentIndex + 1]) = 
                (_checkedStates[currentIndex + 1], _checkedStates[currentIndex]);

            // 選択位置を更新
            SelectedIndex = currentIndex + 1;
            Invalidate();
        }
    }

    /// <summary>
    /// キーボードショートカットで上下移動
    /// </summary>
    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Control)
        {
            if (e.KeyCode == Keys.Up)
            {
                MoveSelectedItemUp();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                MoveSelectedItemDown();
                e.Handled = true;
            }
        }
    }
}



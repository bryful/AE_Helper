using System.ComponentModel;
using System.Windows.Forms;

namespace ColorSwitchEdit;

/// <summary>
/// ColorTableのプロパティを編集するパネル
/// </summary>
public class ColorTablePropertyPanel : UserControl
{
    private ColorTable? _colorTable;
    private NumericUpDown numActiveParamCount;
    private ComboBox comboMode;
    private Label lblActiveParamCount;
    private Label lblMode;

    public ColorTablePropertyPanel()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        lblActiveParamCount = new Label
        {
            Text = "Active Param Count:",
            Location = new Point(10, 10),
            AutoSize = true
        };

        numActiveParamCount = new NumericUpDown
        {
            Location = new Point(140, 8),
            Width = 80,
            Minimum = 0,
            Maximum = 32,
            Enabled = true
        };
        numActiveParamCount.ValueChanged += NumActiveParamCount_ValueChanged;

        lblMode = new Label
        {
            Text = "Mode:",
            Location = new Point(10, 40),
            AutoSize = true
        };

        comboMode = new ComboBox
        {
            Location = new Point(140, 38),
            Width = 120,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        comboMode.Items.AddRange(new object[] { "Replace", "Key", "Extract" });
        comboMode.SelectedIndexChanged += (s, e) => UpdateColorTable();

        Controls.AddRange(new Control[] 
        { 
            lblActiveParamCount, 
            numActiveParamCount, 
            lblMode, 
            comboMode 
        });

        Height = 80;
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

    private void UpdateFromColorTable()
    {
        if (_colorTable == null) return;

        numActiveParamCount.Value = _colorTable.ActiveParamCount;
        comboMode.SelectedIndex = _colorTable.Mode - 1; // 1-based to 0-based
    }

    private void UpdateColorTable()
    {
        if (_colorTable == null) return;

        _colorTable.Mode = comboMode.SelectedIndex + 1; // 0-based to 1-based
    }

    private void NumActiveParamCount_ValueChanged(object? sender, EventArgs e)
    {
        if (_colorTable == null) return;

        _colorTable.ActiveParamCount = (int)numActiveParamCount.Value;
        
        // ActiveParamCountChanged イベントを発火
        ActiveParamCountChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// ActiveParamCountが変更された時に発火するイベント
    /// </summary>
    public event EventHandler? ActiveParamCountChanged;
}
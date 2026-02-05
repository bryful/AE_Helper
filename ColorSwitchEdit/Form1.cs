using System;
using System.Windows.Forms;

namespace ColorSwitchEdit;

public partial class Form1 : Form
{
    private ColorTable? colorTable;
	private ApplicationSettings settings;

    public Form1()
    {
        InitializeComponent();
		
		// 設定を読み込む
		settings = ApplicationSettings.Load();
		
		colorTablePropertyPanel1.ActiveParamCountChanged += PropertyPanel_ActiveParamCountChanged;
		quitMenu.Click +=(s, e) => { Application.Exit(); };
        openMenu.Click += BtnLoad_Click;
        saveMenu.Click += BtnSave_Click;
		exportPngMenu.Click += ExportPngMenu_Click;
		importPngMenu.Click += ImportPngMenu_Click;
		moveUpMenu.Click += MoveUpMenu_Click;
		moveDownMenu.Click += MoveDownMenu_Click;
		
		// ボタンのイベント接続
		btnOpen.Click += BtnLoad_Click;
		btnSave.Click += BtnSave_Click;
		btnPNGExport.Click += ExportPngMenu_Click;
		btnPNGImport.Click += ImportPngMenu_Click;
		btnUp.Click += MoveUpMenu_Click;
		btnDown.Click += MoveDownMenu_Click;
		
		// フォームイベント
		Load += Form1_Load;
		FormClosing += Form1_FormClosing;
	}

	private void Form1_Load(object? sender, EventArgs e)
	{
		// フォームのサイズと位置を復元
		if (settings.FormWidth > 0 && settings.FormHeight > 0)
		{
			Width = settings.FormWidth;
			Height = settings.FormHeight;
		}
		
		if (settings.FormX >= 0 && settings.FormY >= 0)
		{
			// 画面内に収まるか確認
			var screen = Screen.FromPoint(new Point(settings.FormX, settings.FormY));
			if (screen.WorkingArea.Contains(settings.FormX, settings.FormY))
			{
				StartPosition = FormStartPosition.Manual;
				Left = settings.FormX;
				Top = settings.FormY;
			}
		}
	}

	private void Form1_FormClosing(object? sender, FormClosingEventArgs e)
	{
		// フォームのサイズと位置を保存
		settings.FormX = Left;
		settings.FormY = Top;
		settings.FormWidth = Width;
		settings.FormHeight = Height;
		settings.Save();
	}

	private void PropertyPanel_ActiveParamCountChanged(object? sender, EventArgs e)
    {
        // ActiveParamCountが変更されたらリストを再描画
        colorTableList1.Invalidate();
    }

    private void BtnLoad_Click(object? sender, EventArgs e)
    {
        using var openFileDialog = new OpenFileDialog
        {
            Filter = "CSJ files (*.csj)|*.csj|All files (*.*)|*.*",
			InitialDirectory = !string.IsNullOrEmpty(settings.LastJsonPath) 
				? Path.GetDirectoryName(settings.LastJsonPath) ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
				: Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            colorTable = ColorTable.LoadFromFile(openFileDialog.FileName);
			colorTablePropertyPanel1.ColorTable = colorTable;
			colorTableList1.ColorTable = colorTable;
			settings.LastJsonPath = openFileDialog.FileName;
        }
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (colorTable == null) return;

		colorTableList1.UpdateColorTable();

        using var saveFileDialog = new SaveFileDialog
        {
            Filter = "CSJ files (*.csj)|*.csj|All files (*.*)|*.*",
			InitialDirectory = !string.IsNullOrEmpty(settings.LastJsonSavePath) 
				? Path.GetDirectoryName(settings.LastJsonSavePath) ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
				: Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        };

        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            colorTable.SaveToFile(saveFileDialog.FileName);
			settings.LastJsonSavePath = saveFileDialog.FileName;
        }
    }

	private void MoveUpMenu_Click(object? sender, EventArgs e)
	{
		colorTableList1.MoveSelectedItemUp();
	}

	private void MoveDownMenu_Click(object? sender, EventArgs e)
	{
		colorTableList1.MoveSelectedItemDown();
	}

	private void ExportPngMenu_Click(object? sender, EventArgs e)
	{
		if (colorTable == null)
		{
			MessageBox.Show("No color table loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		colorTableList1.UpdateColorTable();

		using var saveFileDialog = new SaveFileDialog
		{
			Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*",
			DefaultExt = "png",
			InitialDirectory = !string.IsNullOrEmpty(settings.LastPngExportPath) 
				? Path.GetDirectoryName(settings.LastPngExportPath) ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
				: Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
		};

		if (saveFileDialog.ShowDialog() == DialogResult.OK)
		{
			try
			{
				colorTable.ExportToPng(saveFileDialog.FileName);
				settings.LastPngExportPath = saveFileDialog.FileName;
				MessageBox.Show("PNG exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to export PNG: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}

	private void ImportPngMenu_Click(object? sender, EventArgs e)
	{
		if (colorTable == null)
		{
			MessageBox.Show("Please load a color table first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;
		}

		using var openFileDialog = new OpenFileDialog
		{
			Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*",
			InitialDirectory = !string.IsNullOrEmpty(settings.LastPngImportPath) 
				? Path.GetDirectoryName(settings.LastPngImportPath) ?? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
				: Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
		};

		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			try
			{
				colorTable.ImportFromPng(openFileDialog.FileName);
				settings.LastPngImportPath = openFileDialog.FileName;
				colorTableList1.Invalidate();
				MessageBox.Show("PNG imported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Failed to import PNG: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}

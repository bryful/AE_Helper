using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ColorSwitchEdit;

/// <summary>
/// アプリケーション設定を保存・復元するクラス
/// </summary>
public class ApplicationSettings
{
	private static readonly string SettingsFilePath = Path.Combine(
		Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
		"ColorSwitchEdit",
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
	public int FormWidth { get; set; } = 360;

	/// <summary>
	/// フォームの高さ
	/// </summary>
	[JsonPropertyName("formHeight")]
	public int FormHeight { get; set; } = 440;

	/// <summary>
	/// 最後に開いたJSONファイルのパス
	/// </summary>
	[JsonPropertyName("lastJsonPath")]
	public string LastJsonPath { get; set; } = "";

	/// <summary>
	/// 最後に保存したJSONファイルのパス
	/// </summary>
	[JsonPropertyName("lastJsonSavePath")]
	public string LastJsonSavePath { get; set; } = "";

	/// <summary>
	/// 最後にエクスポートしたPNGファイルのパス
	/// </summary>
	[JsonPropertyName("lastPngExportPath")]
	public string LastPngExportPath { get; set; } = "";

	/// <summary>
	/// 最後にインポートしたPNGファイルのパス
	/// </summary>
	[JsonPropertyName("lastPngImportPath")]
	public string LastPngImportPath { get; set; } = "";

	/// <summary>
	/// 設定ファイルから読み込む
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
	/// 設定ファイルに保存する
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
}

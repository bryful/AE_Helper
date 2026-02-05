using System.Drawing;
using System.Drawing.Imaging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ColorSwitchEdit;

/// <summary>
/// カラーテーブル設定を表すクラス
/// </summary>
public class ColorTable
{
    [JsonPropertyName("active_param_count")]
    public int ActiveParamCount { get; set; }

    [JsonPropertyName("enable_all")]
    public int EnableAll { get; set; }

    [JsonPropertyName("mode")]
    public int Mode { get; set; }

    [JsonPropertyName("new_colors")]
    [JsonConverter(typeof(ColorArrayConverter))]
    public Color[] NewColors { get; set; } = new Color[32];

    [JsonPropertyName("old_colors")]
    [JsonConverter(typeof(ColorArrayConverter))]
    public Color[] OldColors { get; set; } = new Color[32];

    [JsonPropertyName("turnon_colors")]
    [JsonConverter(typeof(BoolArrayConverter))]
    public bool[] TurnonColors { get; set; } = new bool[32];

    /// <summary>
    /// JSONファイルから読み込む
    /// </summary>
    public static ColorTable LoadFromFile(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<ColorTable>(json) 
            ?? throw new InvalidOperationException("Failed to deserialize color table");
    }

    /// <summary>
    /// JSONファイルに保存する
    /// </summary>
    public void SaveToFile(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize(this, options);
        File.WriteAllText(filePath, json);
    }

    /// <summary>
    /// 非同期でJSONファイルから読み込む
    /// </summary>
    public static async Task<ColorTable> LoadFromFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        await using var stream = File.OpenRead(filePath);
        return await JsonSerializer.DeserializeAsync<ColorTable>(stream, cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Failed to deserialize color table");
    }

    /// <summary>
    /// 非同期でJSONファイルに保存する
    /// </summary>
    public async Task SaveToFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        await using var stream = File.Create(filePath);
        await JsonSerializer.SerializeAsync(stream, this, options, cancellationToken);
    }

    /// <summary>
    /// 2つの項目を入れ替える
    /// </summary>
    public void SwapItems(int index1, int index2)
    {
        if (index1 < 0 || index1 >= 32 || index2 < 0 || index2 >= 32)
            throw new ArgumentOutOfRangeException("Index must be between 0 and 31");

        // OldColorsを入れ替え
        (OldColors[index1], OldColors[index2]) = (OldColors[index2], OldColors[index1]);

        // NewColorsを入れ替え
        (NewColors[index1], NewColors[index2]) = (NewColors[index2], NewColors[index1]);

        // TurnonColorsを入れ替え
        (TurnonColors[index1], TurnonColors[index2]) = (TurnonColors[index2], TurnonColors[index1]);
    }

    /// <summary>
    /// 項目を上に移動
    /// </summary>
    public bool MoveUp(int index)
    {
        if (index <= 0 || index >= 32) return false;
        SwapItems(index, index - 1);
        return true;
    }

    /// <summary>
    /// 項目を下に移動
    /// </summary>
    public bool MoveDown(int index)
    {
        if (index < 0 || index >= 31) return false;
        SwapItems(index, index + 1);
        return true;
    }

    /// <summary>
    /// PNG画像に書き出す
    /// </summary>
    public void ExportToPng(string filePath)
    {
        const int width = 88;
        const int height = 900;
        const int itemSize = 24;
        const int gap = 4;
        const int margin = 2;
        const int rowHeight = 28;

        using var bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        using var g = Graphics.FromImage(bitmap);

        // 背景を透明に
        g.Clear(Color.Transparent);

        // テキストレンダリング設定：アンチエイリアシングなし
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

        // フォント設定
        using var font = new Font("Arial", 10, FontStyle.Bold);
        using var format = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        for (int i = 0; i < 32; i++)
        {
            int y = margin + i * rowHeight;

            // インデックス番号を描画（4-28px、上下左右2px余白）
            var indexRect = new Rectangle(margin + margin, y, itemSize, itemSize);
            using (var brush = new SolidBrush(Color.Black))
            {
                g.DrawString(i.ToString("D2"), font, brush, indexRect, format);
            }

            // Old Color を描画（32-56px、上下左右2px余白）
            var oldRect = new Rectangle(margin + margin + itemSize + gap, y, itemSize, itemSize);
            using (var brush = new SolidBrush(OldColors[i]))
            {
                g.FillRectangle(brush, oldRect);
            }

            // New Color を描画（60-84px、上下左右2px余白）
            var newRect = new Rectangle(margin + margin + itemSize + gap + itemSize + gap, y, itemSize, itemSize);
            using (var brush = new SolidBrush(NewColors[i]))
            {
                g.FillRectangle(brush, newRect);
            }
        }

        bitmap.Save(filePath, ImageFormat.Png);
    }

    /// <summary>
    /// PNG画像から読み込む
    /// </summary>
    public void ImportFromPng(string filePath)
    {
        using var bitmap = new Bitmap(filePath);

        const int itemSize = 24;
        const int gap = 4;
        const int margin = 2;
        const int rowHeight = 28;

        for (int i = 0; i < 32; i++)
        {
            int y = margin + i * rowHeight;
            int centerY = y + itemSize / 2;

            // Old Color の中央のピクセルをサンプリング（32-56pxの中央 = 44px）
            int oldX = margin + margin + itemSize + gap + itemSize / 2;
            OldColors[i] = bitmap.GetPixel(oldX, centerY);

            // New Color の中央のピクセルをサンプリング（60-84pxの中央 = 72px）
            int newX = margin + margin + itemSize + gap + itemSize + gap + itemSize / 2;
            NewColors[i] = bitmap.GetPixel(newX, centerY);
        }
    }
}

/// <summary>
/// Color配列をJSON配列（RGB値）に変換するコンバーター
/// </summary>
public class ColorArrayConverter : JsonConverter<Color[]>
{
    public override Color[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var colors = new List<Color>();
        
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();
        
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;
            
            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();
            
            reader.Read();
            int r = reader.GetInt32();
            reader.Read();
            int g = reader.GetInt32();
            reader.Read();
            int b = reader.GetInt32();
            reader.Read(); // EndArray
            
            colors.Add(Color.FromArgb(r, g, b));
        }
        
        return colors.ToArray();
    }

    public override void Write(Utf8JsonWriter writer, Color[] value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        
        foreach (var color in value)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(color.R);
            writer.WriteNumberValue(color.G);
            writer.WriteNumberValue(color.B);
            writer.WriteEndArray();
        }
        
        writer.WriteEndArray();
    }
}

/// <summary>
/// bool配列をJSON数値配列（0/1）に変換するコンバーター
/// </summary>
public class BoolArrayConverter : JsonConverter<bool[]>
{
    public override bool[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var bools = new List<bool>();
        
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();
        
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;
            
            bools.Add(reader.GetInt32() != 0);
        }
        
        return bools.ToArray();
    }

    public override void Write(Utf8JsonWriter writer, bool[] value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        
        foreach (var b in value)
        {
            writer.WriteNumberValue(b ? 1 : 0);
        }
        
        writer.WriteEndArray();
    }
}
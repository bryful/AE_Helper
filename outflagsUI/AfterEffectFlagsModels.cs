using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace outflagsUI
{
    /// <summary>
    /// 個別のフラグ情報を表すクラス
    /// </summary>
    public class OutFlagInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("value")]
        public long Value { get; set; }

        [JsonPropertyName("bit")]
        public int Bit { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("relevant_commands")]
        public string RelevantCommands { get; set; } = string.Empty;

        public override string ToString() => Name;
    }

    /// <summary>
    /// JSONファイル全体の構造を表すクラス
    /// </summary>
    public class AfterEffectFlags
    {
        [JsonPropertyName("outFlags1")]
        public List<OutFlagInfo> OutFlags1 { get; set; } = new();

        [JsonPropertyName("outFlags2")]
        public List<OutFlagInfo> OutFlags2 { get; set; } = new();
    }
}
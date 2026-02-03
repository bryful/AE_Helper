using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace outflagsUI
{
    /// <summary>
    /// After EffectのフラグデータをJSONから読み込み、管理するクラス
    /// </summary>
    public class AfterEffectFlagsManager
    {
        private AfterEffectFlags? _flags;

        /// <summary>
        /// OutFlags1のリスト
        /// </summary>
        public IReadOnlyList<OutFlagInfo> OutFlags1 => _flags?.OutFlags1 ?? new List<OutFlagInfo>();

        /// <summary>
        /// OutFlags2のリスト
        /// </summary>
        public IReadOnlyList<OutFlagInfo> OutFlags2 => _flags?.OutFlags2 ?? new List<OutFlagInfo>();

        /// <summary>
        /// JSONファイルからフラグデータを読み込む
        /// </summary>
        /// <param name="filePath">JSONファイルのパス</param>
        public void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"ファイルが見つかりません: {filePath}");
            }

            string jsonContent = File.ReadAllText(filePath);
            _flags = JsonSerializer.Deserialize<AfterEffectFlags>(jsonContent);
        }
        public void LoadFromJsonString(string jsonContent)
        {
            _flags = JsonSerializer.Deserialize<AfterEffectFlags>(jsonContent);
		}
        public List<OutFlagInfo> GetOutFlags1()
        {
                       return OutFlags1.ToList();
		}
		public List<OutFlagInfo> GetOutFlags2()
		{
			return OutFlags2.ToList();
		}
		/// <summary>
		/// フラグ名からOutFlags1のフラグ情報を取得
		/// </summary>
		public OutFlagInfo? GetOutFlag1ByName(string name)
        {
            return OutFlags1.FirstOrDefault(f => f.Name == name);
        }

        /// <summary>
        /// フラグ名からOutFlags2のフラグ情報を取得
        /// </summary>
        public OutFlagInfo? GetOutFlag2ByName(string name)
        {
            return OutFlags2.FirstOrDefault(f => f.Name == name);
        }

        /// <summary>
        /// ビット位置からOutFlags1のフラグ情報を取得
        /// </summary>
        public OutFlagInfo? GetOutFlag1ByBit(int bit)
        {
            return OutFlags1.FirstOrDefault(f => f.Bit == bit);
        }

        /// <summary>
        /// ビット位置からOutFlags2のフラグ情報を取得
        /// </summary>
        public OutFlagInfo? GetOutFlag2ByBit(int bit)
        {
            return OutFlags2.FirstOrDefault(f => f.Bit == bit);
        }

        /// <summary>
        /// 値からアクティブなフラグのリストを取得（OutFlags1）
        /// </summary>
        public List<OutFlagInfo> GetActiveOutFlags1(long flagValue)
        {
            return OutFlags1.Where(f => (flagValue & f.Value) != 0).ToList();
        }

        /// <summary>
        /// 値からアクティブなフラグのリストを取得（OutFlags2）
        /// </summary>
        public List<OutFlagInfo> GetActiveOutFlags2(long flagValue)
        {
            return OutFlags2.Where(f => (flagValue & f.Value) != 0).ToList();
        }

        /// <summary>
        /// フラグリストから値を計算
        /// </summary>
        public long CalculateFlagValue(IEnumerable<OutFlagInfo> flags)
        {
            return flags.Aggregate(0L, (current, flag) => current | flag.Value);
        }
    }
}
#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <filesystem>
#include <algorithm>
#include <windows.h>
#include <io.h>
#include <fcntl.h>
#include <sstream>
#include "json.hpp"

using json = nlohmann::json;
namespace fs = std::filesystem;

// UTF-8からワイド文字列に変換
std::wstring Utf8ToWide(const std::string& utf8Str) {
    if (utf8Str.empty()) return L"";
    int wideSize = MultiByteToWideChar(CP_UTF8, 0, utf8Str.c_str(), -1, nullptr, 0);
    if (wideSize == 0) return L"";
    std::vector<wchar_t> wideStr(wideSize);
    MultiByteToWideChar(CP_UTF8, 0, utf8Str.c_str(), -1, wideStr.data(), wideSize);
    return std::wstring(wideStr.data());
}

struct FlagInfo {
    std::string name;
    uint64_t value;
    int bit;
    std::string description;
    std::string relevant_commands;
};

class AE_FlagManager {
private:
    json jsonData;
    fs::path jsonPath;
    fs::path currentDir;

    std::vector<FlagInfo> ParseFlags(const std::string& key) {
        std::vector<FlagInfo> flags;
        if (jsonData.contains(key)) {
            for (const auto& item : jsonData[key]) {
                FlagInfo info;
                info.name = item["name"].get<std::string>();
                info.value = item["value"].get<uint64_t>();
                info.bit = item["bit"].get<int>();
                info.description = item["description"].get<std::string>();
                info.relevant_commands = item["relevant_commands"].get<std::string>();
                flags.push_back(info);
            }
        }
        return flags;
    }

    std::vector<std::string> SplitByMaruten(const std::string& text) {
        std::vector<std::string> result;
        // UTF-8の「。」のバイト列: 0xE3 0x80 0x82
        const char maruten[] = {'\xE3', '\x80', '\x82', '\0'};
        const size_t marutenLen = 3;
        
        size_t start = 0;
        size_t pos = 0;
        
        while ((pos = text.find(maruten, start)) != std::string::npos) {
            // 「。」を含めて切り出す
            std::string segment = text.substr(start, pos - start + marutenLen);
            if (!segment.empty()) {
                result.push_back(segment);
            }
            start = pos + marutenLen;
        }
        
        // 残りの部分を追加
        if (start < text.length()) {
            std::string remaining = text.substr(start);
            // 空白のみの文字列でなければ追加
            if (!remaining.empty() && remaining.find_first_not_of(" \t\r\n") != std::string::npos) {
                result.push_back(remaining);
            }
        }
        
        return result;
    }

    void ExportList(const std::string& key, uint64_t reverseValue = 0, bool useReverse = false) {
        auto flags = ParseFlags(key);
        fs::path fileName = currentDir / (key + ".txt");

        std::ofstream file(fileName, std::ios::binary);
        if (!file) {
            std::wcerr << L"エラー: ファイルを作成できません: " << fileName << std::endl;
            return;
        }

        // UTF-8 BOM
        const unsigned char bom[] = { 0xEF, 0xBB, 0xBF };
        file.write(reinterpret_cast<const char*>(bom), 3);

        for (const auto& flag : flags) {
            // descriptionを「。」で分割
            auto descLines = SplitByMaruten(flag.description);
            
            if (useReverse && (reverseValue & flag.value) == flag.value && flag.value != 0) {
                // 有効な行（セミコロンなし）
                for (const auto& descLine : descLines) {
                    file << descLine << "\r\n";
                }
                file << flag.name << "\r\n";
            }
            else {
                // 無効な行（セミコロンあり）
                for (const auto& descLine : descLines) {
                    file << "; " << descLine << "\r\n";
                }
                file << "; " << flag.name << "\r\n";
            }
            file << "\r\n";  // 各フラグブロックの後に空行を追加
        }

        file.close();
        std::wcout << L"Success: " << fileName.filename() << L" を作成しました。" << std::endl;
    }

    uint64_t ParseValue(const std::string& str) {
        if (str.substr(0, 2) == "0x" || str.substr(0, 2) == "0X") {
            return std::stoull(str, nullptr, 16);
        }
        return std::stoull(str);
    }

public:
    AE_FlagManager(const fs::path& jsonFilePath) : jsonPath(jsonFilePath) {
        currentDir = fs::current_path();

        std::ifstream file(jsonPath);
        if (!file) {
            throw std::runtime_error("JSONファイルが見つかりません: " + jsonPath.string());
        }
        file >> jsonData;
    }

    void ShowHelp() {
        std::wcout << LR"(AE_FlagManager - After Effects SDK Flag Utility

Usage:
  outflags -write               : 全リストをカレントに書き出し (; 付き)
  outflags -outflag             : outFlags1 をカレントに書き出し (; 付き)
  outflags -outflag2            : outFlags2 をカレントに書き出し (; 付き)
  outflags -calc <File>         : 有効な行の値を合算
  outflags -routflag <Value>    : 値に含まれるFlagの ";" を外してカレントに書き出し
  outflags -routflag2 <Value>   : 値に含まれるFlag2の ";" を外してカレントに書き出し

Options:
  -json <Path>                  : JSONファイルのパスを明示的に指定
)" << std::endl;
    }

    void WriteAllLists() {
        ExportList("outFlags1");
        ExportList("outFlags2");
    }

    void WriteOutFlag() {
        ExportList("outFlags1");
    }

    void WriteOutFlag2() {
        ExportList("outFlags2");
    }

    void CalcFile(const std::string& filePath) {
        if (!fs::exists(filePath)) {
            std::wcerr << L"エラー: ファイルが見つかりません: " << Utf8ToWide(filePath) << std::endl;
            return;
        }

        std::ifstream file(filePath);
        uint64_t totalValue = 0;
        bool found1 = false, found2 = false;

        auto flags1 = ParseFlags("outFlags1");
        auto flags2 = ParseFlags("outFlags2");

        std::string line;
        while (std::getline(file, line)) {
            // トリム
            size_t start = line.find_first_not_of(" \t\r\n");
            if (start == std::string::npos) continue;
            line = line.substr(start);

            // コメント行をスキップ（セミコロンで始まる行も除外）
            if (line.empty() || line[0] == '#' || line[0] == ';' ||
                line.substr(0, 2) == "//" ||
                line.substr(0, 2) == "/*") continue;

            // "//" 以前の部分を取得
            size_t commentPos = line.find("//");
            if (commentPos != std::string::npos) {
                line = line.substr(0, commentPos);
            }

            // トリム
            size_t end = line.find_last_not_of(" \t\r\n");
            if (end != std::string::npos) {
                line = line.substr(0, end + 1);
            }

            if (line.empty()) continue;

            // outFlags1をチェック
            for (const auto& flag : flags1) {
                if (flag.name == line) {
                    found1 = true;
                    totalValue |= flag.value;
                    break;
                }
            }

            // outFlags2をチェック
            for (const auto& flag : flags2) {
                if (flag.name == line) {
                    found2 = true;
                    totalValue |= flag.value;
                    break;
                }
            }
        }

        if (found1 && found2) {
            std::wcerr << L"エラー: outFlags1 と outFlags2 が混在しています。" << std::endl;
            return;
        }

        std::wcout << L"--- 計算結果 ---" << std::endl;
        std::wcout << L"10進数: " << totalValue << std::endl;
        std::wcout << L"16進数: 0x";
        wprintf(L"%08llX\n", totalValue);
    }

    void ReverseOutFlag(const std::string& valueStr) {
        uint64_t value = ParseValue(valueStr);
        ExportList("outFlags1", value, true);
    }

    void ReverseOutFlag2(const std::string& valueStr) {
        uint64_t value = ParseValue(valueStr);
        ExportList("outFlags2", value, true);
    }
};

int main(int argc, char* argv[]) {
    // コンソールをUnicodeモードに設定
    _setmode(_fileno(stdout), _O_U16TEXT);
    _setmode(_fileno(stderr), _O_U16TEXT);

    try {
        // デフォルトのJSONパス（実行ファイルと同じフォルダ）
        fs::path exePath = fs::path(argv[0]).parent_path();
        fs::path jsonPath = exePath / "AfterEffect_H.json";

        // コマンドライン引数の解析
        std::string command;
        std::string param;

        for (int i = 1; i < argc; i++) {
            std::string arg = argv[i];
            if (arg == "-json" && i + 1 < argc) {
                jsonPath = argv[++i];
            }
            else if (arg == "-write") {
                command = "write";
            }
            else if (arg == "-outflag") {
                command = "outflag";
            }
            else if (arg == "-outflag2") {
                command = "outflag2";
            }
            else if (arg == "-calc" && i + 1 < argc) {
                command = "calc";
                param = argv[++i];
            }
            else if (arg == "-routflag" && i + 1 < argc) {
                command = "routflag";
                param = argv[++i];
            }
            else if (arg == "-routflag2" && i + 1 < argc) {
                command = "routflag2";
                param = argv[++i];
            }
            else if (arg == "-help" || arg == "--help" || arg == "-h" || arg == "/?") {
                command = "help";
            }
        }

        AE_FlagManager manager(jsonPath);

        if (command.empty()) {
            manager.ShowHelp();
        }
        else if (command == "help") {
            manager.ShowHelp();
        }
        else if (command == "write") {
            manager.WriteAllLists();
        }
        else if (command == "outflag") {
            manager.WriteOutFlag();
        }
        else if (command == "outflag2") {
            manager.WriteOutFlag2();
        }
        else if (command == "calc") {
            manager.CalcFile(param);
        }
        else if (command == "routflag") {
            manager.ReverseOutFlag(param);
        }
        else if (command == "routflag2") {
            manager.ReverseOutFlag2(param);
        }

        return 0;
    }
    catch (const std::exception& e) {
        std::wcerr << L"エラー: " << Utf8ToWide(e.what()) << std::endl;
        return 1;
    }
}
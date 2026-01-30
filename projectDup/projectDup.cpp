#include <iostream>
#include <filesystem>
#include <fstream>
#include <string>
#include <vector>
#include <windows.h>
#include <io.h>
#include <fcntl.h>

namespace fs = std::filesystem;

// Shift-JISからUTF-8への変換
std::string ConvertShiftJISToUTF8(const std::string& sjisStr) {
    // Shift-JISからワイド文字へ
    int wideSize = MultiByteToWideChar(932, 0, sjisStr.c_str(), -1, nullptr, 0);
    if (wideSize == 0) return sjisStr;

    std::vector<wchar_t> wideStr(wideSize);
    MultiByteToWideChar(932, 0, sjisStr.c_str(), -1, wideStr.data(), wideSize);

    // ワイド文字からUTF-8へ
    int utf8Size = WideCharToMultiByte(CP_UTF8, 0, wideStr.data(), -1, nullptr, 0, nullptr, nullptr);
    if (utf8Size == 0) return sjisStr;

    std::vector<char> utf8Str(utf8Size);
    WideCharToMultiByte(CP_UTF8, 0, wideStr.data(), -1, utf8Str.data(), utf8Size, nullptr, nullptr);

    return std::string(utf8Str.data());
}

// UTF-8文字列をワイド文字列に変換
std::wstring Utf8ToWide(const std::string& utf8Str) {
    if (utf8Str.empty()) return L"";

    int wideSize = MultiByteToWideChar(CP_UTF8, 0, utf8Str.c_str(), -1, nullptr, 0);
    if (wideSize == 0) return L"";

    std::vector<wchar_t> wideStr(wideSize);
    MultiByteToWideChar(CP_UTF8, 0, utf8Str.c_str(), -1, wideStr.data(), wideSize);

    return std::wstring(wideStr.data());
}

// ファイルがShift-JISかどうかを判定（簡易版）
bool IsShiftJIS(const std::vector<unsigned char>& content) {
    // BOMチェック
    if (content.size() >= 3 && content[0] == 0xEF && content[1] == 0xBB && content[2] == 0xBF) {
        return false; // UTF-8 BOM
    }

    // Shift-JIS特有のバイトパターンをチェック
    for (size_t i = 0; i < content.size(); i++) {
        if ((content[i] >= 0x81 && content[i] <= 0x9F) || (content[i] >= 0xE0 && content[i] <= 0xFC)) {
            return true; // Shift-JIS範囲
        }
    }
    return false;
}

// ファイルの読み込み
std::string ReadFile(const fs::path& path, bool& wasShiftJIS) {
    std::ifstream file(path, std::ios::binary);
    if (!file) {
        throw std::runtime_error("ファイルを開けません: " + path.string());
    }

    std::vector<unsigned char> content((std::istreambuf_iterator<char>(file)), std::istreambuf_iterator<char>());
    file.close();

    wasShiftJIS = IsShiftJIS(content);

    std::string result(content.begin(), content.end());

    if (wasShiftJIS) {
        result = ConvertShiftJISToUTF8(result);
    }

    return result;
}

// ファイルの書き込み（UTF-8 BOM付き）
void WriteFile(const fs::path& path, const std::string& content, bool addBOM) {
    std::ofstream file(path, std::ios::binary);
    if (!file) {
        throw std::runtime_error("ファイルを書き込めません: " + path.string());
    }

    if (addBOM) {
        // UTF-8 BOM
        const unsigned char bom[] = { 0xEF, 0xBB, 0xBF };
        file.write(reinterpret_cast<const char*>(bom), 3);
    }

    file.write(content.c_str(), content.size());
    file.close();
}

// 文字列置換
void ReplaceAll(std::string& str, const std::string& from, const std::string& to) {
    if (from.empty()) return;

    size_t pos = 0;
    while ((pos = str.find(from, pos)) != std::string::npos) {
        str.replace(pos, from.length(), to);
        pos += to.length();
    }
}

// ディレクトリを再帰的にコピーして置換
void CopyAndReplaceDirectory(const fs::path& srcDir, const fs::path& dstDir,
    const std::string& oldStr, const std::string& newStr) {
    if (!fs::exists(srcDir)) {
        throw std::runtime_error("ソースディレクトリが存在しません: " + srcDir.string());
    }

    fs::create_directories(dstDir);

    for (const auto& entry : fs::recursive_directory_iterator(srcDir)) {
        fs::path relativePath = fs::relative(entry.path(), srcDir);
        fs::path dstPath = dstDir / relativePath;

        // パス名の置換
        std::string pathStr = dstPath.string();
        ReplaceAll(pathStr, oldStr, newStr);
        dstPath = pathStr;

        if (entry.is_directory()) {
            fs::create_directories(dstPath);
        }
        else if (entry.is_regular_file()) {
            std::string ext = entry.path().extension().string();

            // テキストファイルの場合は内容を置換
            if (ext == ".cpp" || ext == ".h" || ext == ".hpp" || ext == ".c" ||
                ext == ".vcxproj" || ext == ".vcxproj.filters" || ext == ".vcxproj.user" ||
                ext == ".sln" || ".slnx" || ext == ".txt" || ext == ".rc" || ext == ".r" || ext == ".xml") {

                try {
                    bool wasShiftJIS = false;
                    std::string content = ReadFile(entry.path(), wasShiftJIS);
                    ReplaceAll(content, oldStr, newStr);
                    WriteFile(dstPath, content, wasShiftJIS);
                    std::wcout << L"置換: " << entry.path().filename() << L" -> " << dstPath.filename() << std::endl;
                }
                catch (const std::exception& e) {
                    std::wcerr << L"警告: " << Utf8ToWide(e.what()) << std::endl;
                    fs::copy_file(entry.path(), dstPath, fs::copy_options::overwrite_existing);
                }
            }
            else {
                // バイナリファイルはそのままコピー
                fs::copy_file(entry.path(), dstPath, fs::copy_options::overwrite_existing);
                std::wcout << L"コピー: " << entry.path().filename() << std::endl;
            }
        }
    }
}

int main(int argc, char* argv[]) {
    // コンソールをUnicodeモードに設定
    _setmode(_fileno(stdout), _O_U16TEXT);
    _setmode(_fileno(stderr), _O_U16TEXT);

    try {
        if (argc < 3) {
            std::wcout << L"使用法: projectDup <元のプロジェクトパス> <新しいプロジェクトパス> [元の文字] [新しい文字]" << std::endl;
            std::wcout << L"  元の文字と新しい文字を省略した場合、フォルダ名が使用されます。" << std::endl;
            return 1;
        }

        fs::path srcPath = argv[1];
        fs::path dstPath = argv[2];
        std::string oldStr, newStr;

        if (argc >= 5) {
            oldStr = argv[3];
            newStr = argv[4];
        }
        else {
            // フォルダ名から自動取得
            oldStr = srcPath.filename().string();
            newStr = dstPath.filename().string();
        }

        std::wcout << L"元のプロジェクト: " << srcPath << std::endl;
        std::wcout << L"新しいプロジェクト: " << dstPath << std::endl;
        std::wcout << L"置換: \"" << Utf8ToWide(oldStr) << L"\" -> \"" << Utf8ToWide(newStr) << L"\"" << std::endl;
        std::wcout << std::endl;

        CopyAndReplaceDirectory(srcPath, dstPath, oldStr, newStr);

        std::wcout << std::endl << L"プロジェクトの複製が完了しました!" << std::endl;
        return 0;
    }
    catch (const std::exception& e) {
        std::wcerr << L"エラー: " << Utf8ToWide(e.what()) << std::endl;
        return 1;
    }
}
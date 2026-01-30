// renProj.cpp : このファイルには 'main' 関数が含まれています。プログラム実行の開始と終了がそこで行われます。
//

#include <iostream>
#include <filesystem>
#include <fstream>
#include <vector>
#include <string>
#include <algorithm>
#include <Windows.h>
#include <fcntl.h>
#include <io.h>

namespace fs = std::filesystem;

// ワイド文字列置換関数
std::wstring replaceAll(std::wstring str, const std::wstring& from, const std::wstring& to) {
    size_t start_pos = 0;
    while ((start_pos = str.find(from, start_pos)) != std::wstring::npos) {
        str.replace(start_pos, from.length(), to);
        start_pos += to.length();
    }
    return str;
}

// UTF-8 BOMチェック
bool hasUtf8Bom(const std::vector<unsigned char>& bytes) {
    return bytes.size() >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF;
}

// UTF-8文字列からワイド文字列に変換
std::wstring utf8ToWide(const std::string& utf8) {
    if (utf8.empty()) return L"";
    int size = MultiByteToWideChar(CP_UTF8, 0, utf8.c_str(), (int)utf8.size(), nullptr, 0);
    std::wstring result(size, 0);
    MultiByteToWideChar(CP_UTF8, 0, utf8.c_str(), (int)utf8.size(), &result[0], size);
    return result;
}

// ワイド文字列からUTF-8文字列に変換
std::string wideToUtf8(const std::wstring& wide) {
    if (wide.empty()) return "";
    int size = WideCharToMultiByte(CP_UTF8, 0, wide.c_str(), (int)wide.size(), nullptr, 0, nullptr, nullptr);
    std::string result(size, 0);
    WideCharToMultiByte(CP_UTF8, 0, wide.c_str(), (int)wide.size(), &result[0], size, nullptr, nullptr);
    return result;
}

// ファイル内容を読み込み（Shift-JISまたはUTF-8）→ワイド文字列で返す
std::wstring readFile(const fs::path& filePath) {
    std::ifstream file(filePath, std::ios::binary);
    if (!file) return L"";

    std::vector<unsigned char> bytes((std::istreambuf_iterator<char>(file)), std::istreambuf_iterator<char>());
    file.close();

    if (bytes.empty()) return L"";

    if (hasUtf8Bom(bytes)) {
        // UTF-8 BOM付き → UTF-8としてデコード
        std::string utf8(bytes.begin() + 3, bytes.end());
        return utf8ToWide(utf8);
    }
    else {
        // Shift-JISとして読み込み
        int size = MultiByteToWideChar(932, 0, (LPCCH)bytes.data(), (int)bytes.size(), nullptr, 0);
        if (size > 0) {
            std::wstring result(size, 0);
            MultiByteToWideChar(932, 0, (LPCCH)bytes.data(), (int)bytes.size(), &result[0], size);
            return result;
        }
    }
    return L"";
}

// UTF-8 BOM付きで保存
bool writeUtf8BomFile(const fs::path& filePath, const std::wstring& content) {
    std::ofstream file(filePath, std::ios::binary);
    if (!file) return false;

    // BOM書き込み
    const unsigned char bom[] = { 0xEF, 0xBB, 0xBF };
    file.write((const char*)bom, 3);

    // ワイド文字列をUTF-8に変換して書き込み
    std::string utf8 = wideToUtf8(content);
    file.write(utf8.c_str(), utf8.size());
    file.close();
    return true;
}

// 対象拡張子かチェック
bool isTargetExtension(const fs::path& path) {
    static const std::vector<std::wstring> extensions = {
        L".cpp", L".h", L".hpp", L".c", L".cxx", L".r", L".rc", L".sln", L".slnx", L".vcxproj", L".filters"
    };
    std::wstring ext = path.extension().wstring();
    std::transform(ext.begin(), ext.end(), ext.begin(), ::towlower);
    return std::find(extensions.begin(), extensions.end(), ext) != extensions.end();
}

// ファイル内容の置換と文字コード変換
void processFileContent(const fs::path& targetDir, const std::wstring& oldText, const std::wstring& newText) {
    std::wcout << L"\n--- 1. 内容置換 & 文字コード変換を開始します ---\n";

    for (const auto& entry : fs::recursive_directory_iterator(targetDir)) {
        if (entry.is_regular_file() && isTargetExtension(entry.path())) {
            std::wstring content = readFile(entry.path());
            std::wstring newContent = replaceAll(content, oldText, newText);

            if (writeUtf8BomFile(entry.path(), newContent)) {
                std::wcout << L"Processed: " << entry.path().filename().wstring() << L"\n";
            }
        }
    }
}

// ファイル・フォルダ名の置換
void renameItems(const fs::path& targetDir, const std::wstring& oldText, const std::wstring& newText) {
    std::wcout << L"\n--- 2. 名前（ファイル・フォルダ）の置換を開始します ---\n";

    // パスの長さでソート（深い階層から処理）
    std::vector<fs::path> items;
    for (const auto& entry : fs::recursive_directory_iterator(targetDir)) {
        items.push_back(entry.path());
    }
    items.push_back(targetDir);

    std::sort(items.begin(), items.end(), [](const fs::path& a, const fs::path& b) {
        return a.wstring().length() > b.wstring().length();
        });

    for (const auto& item : items) {
        std::wstring name = item.filename().wstring();
        if (name.find(oldText) != std::wstring::npos) {
            std::wstring newName = replaceAll(name, oldText, newText);
            fs::path destination = item.parent_path() / newName;

            if (item != destination) {
                std::wcout << L"Renaming: " << name << L" -> " << newName << L"\n";
                try {
                    fs::rename(item, destination);
                }
                catch (const std::exception& e) {
                    std::cerr << "Error renaming: " << e.what() << "\n";
                }
            }
        }
    }
}

int wmain(int argc, wchar_t* argv[]) {
    // コンソールをUTF-16モードに設定
    _setmode(_fileno(stdout), _O_U16TEXT);
    _setmode(_fileno(stderr), _O_U16TEXT);

    if (argc != 4) {
        std::wcout << L"使い方: renPrj <TargetDir> <OldText> <NewText>\n";
        std::wcout << L"例: renPrj C:\\MyProject OldName NewName\n";
        return 1;
    }

    fs::path targetDir = argv[1];
    std::wstring oldText = argv[2];
    std::wstring newText = argv[3];

    if (!fs::exists(targetDir) || !fs::is_directory(targetDir)) {
        std::wcout << L"エラー: 指定されたディレクトリが存在しません: " << targetDir.wstring() << L"\n";
        return 1;
    }

    try {
        processFileContent(targetDir, oldText, newText);
        renameItems(targetDir, oldText, newText);

        std::wcout << L"\nすべての処理が完了しました。\n";
    }
    catch (const std::exception& e) {
        std::cerr << "エラー: " << e.what() << "\n";
        return 1;
    }

    return 0;
}
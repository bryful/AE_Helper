// Add-DateToAex.cpp : .aexファイルに日付を付与/削除するコンソールアプリケーション
//

#include <iostream>
#include <filesystem>
#include <string>
#include <regex>
#include <chrono>
#include <iomanip>
#include <sstream>

namespace fs = std::filesystem;

// 現在の日付をyyyyMMdd形式で取得
std::string GetTodayString()
{
    auto now = std::chrono::system_clock::now();
    auto time = std::chrono::system_clock::to_time_t(now);
    std::tm tm;
    localtime_s(&tm, &time);
    
    std::ostringstream oss;
    oss << std::put_time(&tm, "%Y%m%d");
    return oss.str();
}

// ファイル名の末尾に "_数字8桁" があるかチェック
bool HasDateSuffix(const std::string& basename)
{
    std::regex pattern("_\\d{8}$");
    return std::regex_search(basename, pattern);
}

// ファイル名から末尾の "_数字8桁" を削除
std::string RemoveDateSuffix(const std::string& basename)
{
    std::regex pattern("(_\\d{8})$");
    return std::regex_replace(basename, pattern, "");
}

// 使用方法を表示
void ShowUsage(const char* programName)
{
    std::cout << "Usage: " << programName << " <TargetDir> [-clear]\n";
    std::cout << "\n";
    std::cout << "  <TargetDir>  : 対象ディレクトリのパス (必須)\n";
    std::cout << "  -clear       : 日付を削除するモード (オプション)\n";
    std::cout << "\n";
    std::cout << "Examples:\n";
    std::cout << "  " << programName << " C:\\Plugins\n";
    std::cout << "  " << programName << " C:\\Plugins -clear\n";
}

int main(int argc, char* argv[])
{
    // コマンドライン引数のチェック
    if (argc < 2)
    {
        std::cerr << "Error: TargetDir is required.\n\n";
        ShowUsage(argv[0]);
        return 1;
    }

    std::string targetDir;
    bool clearMode = false;
    bool foundDir = false;

    // すべての引数を解析
    for (int i = 1; i < argc; i++)
    {
        std::string arg = argv[i];
        
        // オプションのチェック
        if (arg == "-clear" || arg == "/clear" || arg == "--clear")
        {
            clearMode = true;
        }
        // ディレクトリパスと判定
        else if (!foundDir)
        {
            targetDir = arg;
            foundDir = true;
        }
    }

    // ディレクトリが指定されていない場合
    if (!foundDir || targetDir.empty())
    {
        std::cerr << "Error: TargetDir is required.\n\n";
        ShowUsage(argv[0]);
        return 1;
    }

    // ディレクトリの存在確認
    if (!fs::exists(targetDir) || !fs::is_directory(targetDir))
    {
        std::cerr << "Error: Directory not found: " << targetDir << "\n";
        return 1;
    }

    std::string today = GetTodayString();
    int processedCount = 0;
    int skippedCount = 0;
    int errorCount = 0;

    // .aexファイルを列挙
    for (const auto& entry : fs::directory_iterator(targetDir))
    {
        if (!entry.is_regular_file())
            continue;

        fs::path filePath = entry.path();
        if (filePath.extension() != ".aex")
            continue;

        std::string basename = filePath.stem().string();
        std::string extension = filePath.extension().string();

        if (clearMode)
        {
            // --- 削除モード ---
            if (HasDateSuffix(basename))
            {
                std::string newBasename = RemoveDateSuffix(basename);
                fs::path newPath = filePath.parent_path() / (newBasename + extension);

                try
                {
                    fs::rename(filePath, newPath);
                    std::cout << "\033[33mCleared: " << filePath.filename().string() 
                              << " -> " << newPath.filename().string() << "\033[0m\n";
                    processedCount++;
                }
                catch (const std::exception& e)
                {
                    std::cerr << "\033[31mError: Could not clear " << filePath.filename().string() 
                              << " (" << e.what() << ")\033[0m\n";
                    errorCount++;
                }
            }
        }
        else
        {
            // --- 付与モード ---
            if (HasDateSuffix(basename))
            {
                std::cout << "\033[90mSkipped: " << filePath.filename().string() 
                          << " (Already has a date stamp)\033[0m\n";
                skippedCount++;
                continue;
            }

            std::string newBasename = basename + "_" + today;
            fs::path newPath = filePath.parent_path() / (newBasename + extension);

            try
            {
                fs::rename(filePath, newPath);
                std::cout << "\033[32mRenamed: " << filePath.filename().string() 
                          << " -> " << newPath.filename().string() << "\033[0m\n";
                processedCount++;
            }
            catch (const std::exception& e)
            {
                std::cerr << "\033[31mError: Failed to rename " << filePath.filename().string() 
                          << " (" << e.what() << ")\033[0m\n";
                errorCount++;
            }
        }
    }

    // 結果サマリー
    std::cout << "\n\033[36m処理が完了しました。\033[0m\n";
    std::cout << "Processed: " << processedCount << " file(s)\n";
    if (skippedCount > 0)
        std::cout << "Skipped:   " << skippedCount << " file(s)\n";
    if (errorCount > 0)
        std::cout << "Errors:    " << errorCount << " file(s)\n";

    return (errorCount > 0) ? 1 : 0;
}


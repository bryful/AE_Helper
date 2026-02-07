// Organize-Aex.cpp : .aexファイルをカテゴリ別に整理/復元するコンソールアプリケーション
//

#include <iostream>
#include <filesystem>
#include <fstream>
#include <string>
#include <vector>
#include <algorithm>

namespace fs = std::filesystem;

// ファイル名に使えない文字を置き換え
std::string SanitizeFileName(const std::string& str)
{
    std::string result = str;
    const char invalidChars[] = { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };
    
    for (char& c : result)
    {
        for (char invalid : invalidChars)
        {
            if (c == invalid)
            {
                c = '_';
                break;
            }
        }
    }
    
    return result;
}

// バイナリファイルからカテゴリを抽出
std::string ExtractCategory(const fs::path& filePath)
{
    const std::vector<uint8_t> signature = { 0x4D, 0x49, 0x42, 0x38, 0x67, 0x74, 0x61, 0x63 }; // "MIB8gtac"
    
    std::ifstream file(filePath, std::ios::binary);
    if (!file)
        return "";
    
    // ファイル全体を読み込む
    file.seekg(0, std::ios::end);
    size_t fileSize = file.tellg();
    file.seekg(0, std::ios::beg);
    
    std::vector<uint8_t> buffer(fileSize);
    file.read(reinterpret_cast<char*>(buffer.data()), fileSize);
    file.close();
    
    // シグネチャを検索
    for (size_t i = 0; i <= buffer.size() - signature.size(); i++)
    {
        bool match = true;
        for (size_t j = 0; j < signature.size(); j++)
        {
            if (buffer[i + j] != signature[j])
            {
                match = false;
                break;
            }
        }
        
        if (match)
        {
            // シグネチャ + 16バイト目に文字列の長さ
            size_t lengthIndex = i + 16;
            if (lengthIndex >= buffer.size())
                continue;
            
            uint8_t stringLength = buffer[lengthIndex];
            size_t stringStartIndex = i + 17;
            
            if (stringStartIndex + stringLength <= buffer.size())
            {
                std::string category(buffer.begin() + stringStartIndex, 
                                   buffer.begin() + stringStartIndex + stringLength);
                
                // 前後の空白を削除
                category.erase(0, category.find_first_not_of(" \t\r\n"));
                category.erase(category.find_last_not_of(" \t\r\n") + 1);
                
                return SanitizeFileName(category);
            }
        }
    }
    
    return "";
}

// 整理モード
void OrganizeMode(const fs::path& targetDir)
{
    std::cout << "Organizing .aex files in: " << targetDir << "\n\n";
    
    int organizedCount = 0;
    int noCategoryCount = 0;
    
    for (const auto& entry : fs::directory_iterator(targetDir))
    {
        if (!entry.is_regular_file())
            continue;
        
        if (entry.path().extension() != ".aex")
            continue;
        
        std::cout << "Scanning: " << entry.path().filename().string() << "...";
        
        std::string category = ExtractCategory(entry.path());
        
        if (!category.empty())
        {
            fs::path destFolder = targetDir / category;
            
            // カテゴリフォルダを作成
            if (!fs::exists(destFolder))
            {
                fs::create_directory(destFolder);
            }
            
            fs::path destPath = destFolder / entry.path().filename();
            
            try
            {
                fs::rename(entry.path(), destPath);
                std::cout << " \033[32m-> [" << category << "]\033[0m\n";
                organizedCount++;
            }
            catch (const std::exception& e)
            {
                std::cerr << " \033[31mError: " << e.what() << "\033[0m\n";
            }
        }
        else
        {
            std::cout << " \033[33m-> No Category Found\033[0m\n";
            noCategoryCount++;
        }
    }
    
    std::cout << "\n\033[36m完了しました。\033[0m\n";
    std::cout << "Organized: " << organizedCount << " file(s)\n";
    if (noCategoryCount > 0)
        std::cout << "No Category: " << noCategoryCount << " file(s)\n";
}

// 復元モード
void RestoreMode(const fs::path& targetDir)
{
    std::cout << "\033[33mRestoring files to: " << targetDir << "\033[0m\n\n";
    
    int movedCount = 0;
    int skippedCount = 0;
    int removedFolderCount = 0;
    
    // 1階層下のサブフォルダを列挙
    for (const auto& folder : fs::directory_iterator(targetDir))
    {
        if (!folder.is_directory())
            continue;
        
        // サブフォルダ内の.aexファイルを列挙
        for (const auto& file : fs::directory_iterator(folder.path()))
        {
            if (!file.is_regular_file())
                continue;
            
            if (file.path().extension() != ".aex")
                continue;
            
            fs::path destPath = targetDir / file.path().filename();
            
            // 同名ファイルが親にある場合
            if (fs::exists(destPath))
            {
                std::cout << "\033[31mSkipped: " << file.path().filename().string() 
                          << " already exists in parent.\033[0m\n";
                skippedCount++;
            }
            else
            {
                try
                {
                    fs::rename(file.path(), destPath);
                    std::cout << "\033[32mMoved up: " << file.path().filename().string() << "\033[0m\n";
                    movedCount++;
                }
                catch (const std::exception& e)
                {
                    std::cerr << "\033[31mError: " << e.what() << "\033[0m\n";
                }
            }
        }
        
        // フォルダが空になったら削除
        if (fs::is_empty(folder.path()))
        {
            fs::remove(folder.path());
            std::cout << "\033[90mRemoved empty folder: " << folder.path().filename().string() << "\033[0m\n";
            removedFolderCount++;
        }
    }
    
    std::cout << "\n\033[36m完了しました。\033[0m\n";
    std::cout << "Moved up: " << movedCount << " file(s)\n";
    if (skippedCount > 0)
        std::cout << "Skipped: " << skippedCount << " file(s)\n";
    if (removedFolderCount > 0)
        std::cout << "Removed: " << removedFolderCount << " folder(s)\n";
}

// 使用方法を表示
void ShowUsage(const char* programName)
{
    std::cout << "Usage: " << programName << " <TargetDir> [-restore]\n";
    std::cout << "\n";
    std::cout << "  <TargetDir>  : 対象ディレクトリのパス (必須)\n";
    std::cout << "  -restore     : 元に戻すモード (オプション)\n";
    std::cout << "\n";
    std::cout << "Examples:\n";
    std::cout << "  " << programName << " C:\\Plugins\n";
    std::cout << "  " << programName << " C:\\Plugins -restore\n";
    std::cout << "  " << programName << " -restore C:\\Plugins\n";
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
    bool restoreMode = false;
    bool foundDir = false;
    
    // すべての引数を解析
    for (int i = 1; i < argc; i++)
    {
        std::string arg = argv[i];
        
        // オプションのチェック
        if (arg == "-restore" || arg == "/restore" || arg == "--restore")
        {
            restoreMode = true;
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
    
    // モードに応じて処理を実行
    if (restoreMode)
    {
        RestoreMode(targetDir);
    }
    else
    {
        OrganizeMode(targetDir);
    }
    
    return 0;
}


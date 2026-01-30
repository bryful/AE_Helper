#include <iostream>
#include <vector>
#include <string>
#include <filesystem>
#include <regex>
#include <algorithm>
#include <nlohmann/json.hpp>

#ifdef _WIN32
#include <windows.h>
#endif

namespace fs = std::filesystem;
using json = nlohmann::json;

namespace GLS {

    // UTF-16(wstring) を UTF-8(string) に変換するヘルパー
    std::string to_utf8(const std::wstring& wstr) {
        if (wstr.empty()) return std::string();
        int size_needed = WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), NULL, 0, NULL, NULL);
        std::string strTo(size_needed, 0);
        WideCharToMultiByte(CP_UTF8, 0, &wstr[0], (int)wstr.size(), &strTo[0], size_needed, NULL, NULL);
        return strTo;
    }

    class ImageDirInfo {
    public:
        std::string FullDirName; // UTF-8
        std::string DirName;     // UTF-8
        std::string NodeName;    // UTF-8
        std::vector<std::string> Items; // UTF-8 list

        bool AddItem(const std::string& item) {
            if (item.empty()) return false;
            if (std::find(Items.begin(), Items.end(), item) != Items.end()) return false;
            Items.push_back(item);
            return true;
        }

        json ToJsonObject() const {
            json j;
            j["fullDirName"] = FullDirName;
            j["dirName"] = DirName;
            j["nodeName"] = NodeName;
            j["items"] = Items;
            return j;
        }
    };

    class ImageLister {
    private:
        fs::path m_TargetDir;
        std::vector<ImageDirInfo> m_dirInfos;

        bool IsImageFile(const fs::path& path) {
            std::wstring ext = path.extension().wstring();
            std::transform(ext.begin(), ext.end(), ext.begin(), ::towlower);
            static const std::vector<std::wstring> validExts = {
                L".tga", L".jpg", L".jpeg", L".png", L".psd", L".bmp", L".tif", L".tiff", L".pic"
            };
            return std::find(validExts.begin(), validExts.end(), ext) != validExts.end();
        }

        // 正規表現エラーを避けるため、wstringで処理
        std::pair<std::string, std::string> ExtractFileNameParts(const fs::path& path) {
            std::wstring stem = path.stem().wstring();
            std::wregex re(L"^(.+?)(\\d+)$");
            std::wsmatch match;

            if (std::regex_search(stem, match, re)) {
                return { to_utf8(match[1].str()), to_utf8(match[2].str()) };
            }
            return { to_utf8(stem), "" };
        }

        bool ListupFromDirSub(const fs::path& dirPath) {
            if (!fs::exists(dirPath)) return false;

            ImageDirInfo dirInfo;
            std::vector<ImageDirInfo> tempDirInfos;
            int count = 0;

            std::vector<fs::directory_entry> entries;
            for (const auto& entry : fs::directory_iterator(dirPath)) {
                if (entry.is_regular_file() && IsImageFile(entry.path())) {
                    entries.push_back(entry);
                }
            }
            std::sort(entries.begin(), entries.end());

            for (const auto& entry : entries) {
                auto path = entry.path();
                auto [name, number] = ExtractFileNameParts(path);
                std::string fileName = to_utf8(path.filename().wstring());

                if (dirInfo.NodeName.empty()) {
                    dirInfo.FullDirName = to_utf8(fs::absolute(dirPath).wstring());
                    dirInfo.DirName = to_utf8(dirPath.filename().wstring());
                    dirInfo.NodeName = name;
                    dirInfo.AddItem(fileName);
                }
                else if (dirInfo.NodeName != name) {
                    tempDirInfos.push_back(dirInfo);
                    dirInfo = ImageDirInfo();
                    dirInfo.FullDirName = to_utf8(fs::absolute(dirPath).wstring());
                    dirInfo.DirName = to_utf8(dirPath.filename().wstring());
                    dirInfo.NodeName = name;
                    dirInfo.AddItem(fileName);
                }
                else {
                    dirInfo.AddItem(fileName);
                }
                count++;
            }

            if (!dirInfo.Items.empty()) tempDirInfos.push_back(dirInfo);
            m_dirInfos.insert(m_dirInfos.end(), tempDirInfos.begin(), tempDirInfos.end());
            return (count > 0);
        }

    public:
        void SetTargetDir(const std::string& dir) {
            fs::path p(dir);
            if (fs::exists(p) && fs::is_directory(p)) m_TargetDir = p;
            else m_TargetDir = "";
        }

        // 再帰的にディレクトリを探索する関数（階層制限付き）
        void ListupRecursive(const fs::path& dir, int currentLevel) {
            // 20階層を超えたら終了
            if (currentLevel > 20) return;

            // 現在のディレクトリ内の画像をリストアップ
            if (ListupFromDirSub(dir)) {
                // 画像が見つかった場合、内部で m_dirInfos に追加される
            }

            // サブディレクトリを探して再帰呼び出し
            try {
                for (const auto& entry : fs::directory_iterator(dir)) {
                    if (entry.is_directory()) {
                        ListupRecursive(entry.path(), currentLevel + 1);
                    }
                }
            }
            catch (const std::exception& e) {
                std::cerr << "[ERROR] Directory access failed: " << e.what() << std::endl;
            }
        }

        bool ListupFromDir(const fs::path& dir) {
            if (dir.empty()) return false;
            m_dirInfos.clear();

            // 階層1からスタートして再帰開始
            ListupRecursive(dir, 1);

            return !m_dirInfos.empty();
        }

        std::string ToJsonString() {
            json jArray = json::array();
            for (const auto& info : m_dirInfos) jArray.push_back(info.ToJsonObject());
            return jArray.dump(4);
        }

        std::string Exec() {
            if (!ListupFromDir(m_TargetDir)) return "";
            return ToJsonString();
        }
    };
}

int main(int argc, char* argv[]) {
#ifdef _WIN32
    SetConsoleOutputCP(65001); // コンソール出力をUTF-8に設定
#endif

    try {
        GLS::ImageLister lister;
        std::string pathStr = (argc > 1) ? argv[1] : fs::current_path().string();

        lister.SetTargetDir(pathStr);
        std::string result = lister.Exec();

        if (result.empty()) {
            std::cout << "画像が見つからないか、ディレクトリが無効です。" << std::endl;
        }
        else {
            std::cout << result << std::endl;
        }
    }
    catch (const std::exception& e) {
        std::cerr << "[FATAL ERROR] " << e.what() << std::endl;
        return 1;
    }
    return 0;
}
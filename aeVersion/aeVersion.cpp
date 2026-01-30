// aeVersion.cpp : このファイルには 'main' 関数が含まれています。プログラム実行の開始と終了がそこで行われます。
//

#include <iostream>
#include <string>
#include <map>

#include <vector>
#include <windows.h>
#include <io.h>
#include <fcntl.h>
#include <cctype>
#include <algorithm>
enum {
	PF_Stage_DEVELOP,
	PF_Stage_ALPHA,
	PF_Stage_BETA,
	PF_Stage_RELEASE
};
typedef long PF_Stage;
#define PF_Vers_BUILD_BITS		0x1ffL
#define PF_Vers_BUILD_SHIFT		0
#define PF_Vers_STAGE_BITS		0x3L
#define PF_Vers_STAGE_SHIFT		9
#define PF_Vers_BUGFIX_BITS		0xfL
#define PF_Vers_BUGFIX_SHIFT	11
#define PF_Vers_SUBVERS_BITS	0xfL
#define PF_Vers_SUBVERS_SHIFT	15
#define PF_Vers_VERS_BITS		0x7L	// incomplete without high bits, below
#define PF_Vers_VERS_SHIFT		19
// skipping these bits for similarity to Up_Vers_ARCH_*, currently unused in PF
#define PF_Vers_VERS_HIGH_BITS		0xfL	// expand version max from 7 to 127
#define PF_Vers_VERS_HIGH_SHIFT		26

// b/c we are stripping the stand alone vers value for two fields
#define PF_Vers_VERS_LOW_SHIFT  3
#define PF_Vers_VERS_HIGH(vers) ((vers)>>PF_Vers_VERS_LOW_SHIFT)


typedef unsigned long PFVersionInfo;

#define PF_VERSION(vers, subvers, bugvers, stage, build)	\
	(PFVersionInfo)(	\
 		((((unsigned long)PF_Vers_VERS_HIGH(vers)) & PF_Vers_VERS_HIGH_BITS) << PF_Vers_VERS_HIGH_SHIFT) |   \
		((((unsigned long)(vers)) & PF_Vers_VERS_BITS) << PF_Vers_VERS_SHIFT) |	\
		((((unsigned long)(subvers)) & PF_Vers_SUBVERS_BITS)<<PF_Vers_SUBVERS_SHIFT) |\
		((((unsigned long)(bugvers)) & PF_Vers_BUGFIX_BITS) << PF_Vers_BUGFIX_SHIFT) |\
		((((unsigned long)(stage)) & PF_Vers_STAGE_BITS) << PF_Vers_STAGE_SHIFT) |	\
		((((unsigned long)(build)) & PF_Vers_BUILD_BITS) << PF_Vers_BUILD_SHIFT)	\
	)
// clang-format on
/*
	out_data->my_version = PF_VERSION(	MAJOR_VERSION,
										MINOR_VERSION,
										BUG_VERSION,
										STAGE_VERSION,
										BUILD_VERSION);
*/
// 文字列を大文字に変換
std::string toUpper(std::string str) {
	std::transform(str.begin(), str.end(), str.begin(),
		[](unsigned char c) { return std::toupper(c); });
	return str;
}

// std::string を std::wstring に変換
std::wstring toWideString(const std::string& str) {
	if (str.empty()) return L"";
	int size = MultiByteToWideChar(CP_ACP, 0, str.c_str(), (int)str.size(), nullptr, 0);
	std::wstring result(size, 0);
	MultiByteToWideChar(CP_ACP, 0, str.c_str(), (int)str.size(), &result[0], size);
	return result;
}

int usage()
{
	std::wcout << L"[aeVersion.exe] プラグインのバージョン値の計算\n";
	std::wcout << L"使い方: aeVersion <1:vers> <2:subvers> <3:bugvers> <4:stage> <5:build>\n";
	std::wcout << L"  stage: DEVELOP　or D\n";
	std::wcout << L"            ALPHA or A\n";
	std::wcout << L"             BETA or B\n";
	std::wcout << L"          RELEASE or R\n\n";
	std::wcout << L"例: aeVersion 1 0 0 DEVELOP 0\n";
	return 1;
}
unsigned long parseUnsignedLong(const char* str,bool *ok)
{
	try {
		if (str != NULL) {
			*ok = true;
			return std::stoul(str);
		}
	}
	catch (...) {
		*ok = false;
		return 0;
	}
	*ok = false;
	return 0;
}

int main(int argc, char* argv[])
{
	// コンソールをUnicodeモードに設定
	_setmode(_fileno(stdout), _O_U16TEXT);
	_setmode(_fileno(stderr), _O_U16TEXT);

	unsigned int vers = 1;
	unsigned int subvers = 0;
	unsigned int bugvers = 0;
	std::string stageStr = "PF_Stage_RELEASE";
	unsigned int build = 0;

	/*
	for (int i = 0; i < argc; i++) {
		std::cout << "argv[" << i << "]=";
		if (argv[i] != NULL) {
			std::cout << argv[i] << "\n";
		}
		else {
			std::cout << "NULL\n";
		}
	}
	*/

	PF_Stage stage = PF_Stage_RELEASE;
	bool ok = true;
	if (argc <= 1) {
		return usage();
	}
	if ((argc >= 2)&&(argv[1] != NULL)) {
		ok = true;
		vers = parseUnsignedLong(argv[1],&ok);
		if (ok == false) {
			return usage();
		}
	}
	if ((argc >= 3) && (argv[2] != NULL)) {
		ok = true;
		subvers = parseUnsignedLong(argv[2], &ok);
		if (ok == false) {
			return usage();
		}
	}
	if ((argc >= 4) && (argv[3] != NULL)) {
		ok = true;
		bugvers = parseUnsignedLong(argv[3], &ok);
		if (ok == false) {
			return usage();
		}
	}
	if ((argc >= 5) && (argv[4] != NULL)) {
		stageStr = toUpper(argv[4]);
		if ((stageStr == "DEVELOP")|| (stageStr == "D") || (stageStr == "DEV")) {
			stage = PF_Stage_DEVELOP;
			stageStr = "PF_Stage_DEVELOP";
		}
		else if ((stageStr == "ALPHA")|| (stageStr == "A")) {
			stage = PF_Stage_ALPHA;
			stageStr = "PF_Stage_ALPHA";
		}
		else if ((stageStr == "BETA")|| (stageStr == "B")) {
			stage = PF_Stage_BETA;
			stageStr = "PF_Stage_BETA";
		}
		else if ((stageStr == "RELEASE")|| (stageStr == "R")) {
			stage = PF_Stage_RELEASE;
			stageStr = "PF_Stage_RELEASE";
		}
		else {
			stageStr = "PF_Stage_RELEASE !";
		}
		
	}
	if ((argc >= 6) && (argv[5] != NULL)) {
		ok = true;
		build = parseUnsignedLong(argv[5], &ok);
		if (ok == false) {
			return usage();
		}
	}

	
	PFVersionInfo myVersion = PF_VERSION(vers, subvers, bugvers, stage, build);

	// 出力
	std::wcout << L"#define\tMAJOR_VERSION\t" << vers << L"\n";
	std::wcout << L"#define\tMINOR_VERSION\t" << subvers << L"\n";
	std::wcout << L"#define\tBUG_VERSION\t" << bugvers << L"\n";
	std::wcout << L"#define\tSTAGE_VERSION\t" << toWideString(stageStr) << L"\n";
	std::wcout << L"#define\tBUILD_VERSION\t" << build << L"\n";
	std::wcout << L"#define MY_VERSION " << myVersion << L"\n";

	return 0;
}
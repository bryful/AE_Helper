#include <iostream>
#include <cstdio>
#include <cstring>

// 元のファイルをインクルードして、SPT[60][196] をロード
// ※インクルード前にマクロが衝突しないよう配慮が必要な場合があります
#include "FsSputtering.h"

// 新しい定義
#define NEW_SPT_WIDTH 16
#define OLD_SPT_WIDTH 14
#define SPT_HEIGHT    14
#define SPT_COUNT     60

// C++用のヘッダーファイル出力
void OutputCpp() {
    std::printf("#pragma once\n");
    std::printf("#ifndef sputteringData_H\n");
    std::printf("#define sputteringData_H\n\n");
    std::printf("#include \"Fs.h\"\n");
    std::printf("#include \"../FsLibrary/FsAE.h\"\n\n");

    // 定義の更新
    std::printf("#define SPT_COUNT %d\n", SPT_COUNT);
    std::printf("#define SPT_WIDTH %d\n", NEW_SPT_WIDTH); // 16px
    std::printf("#define SPT_SIZE(IDX) (SPT_SIZE_TBL[(IDX)])\n");
    std::printf("#define SPT_VALUE(IDX,X,Y) (SPT[(IDX)][(X) + ((Y) * SPT_WIDTH)])\n\n");

    // 配列サイズは 60 * (16 * 14) になります
    std::printf("static A_u_char SPT[SPT_COUNT][SPT_WIDTH * 14] = {\n");

    for (int i = 0; i < SPT_COUNT; ++i) {
        std::printf("\t{\n");
        for (int y = 0; y < SPT_HEIGHT; ++y) {
            std::printf("\t\t");
            for (int x = 0; x < NEW_SPT_WIDTH; ++x) {
                int val = 0;
                // 横幅14番目までは元のデータを使用、15・16番目は0（パディング）
                if (x < OLD_SPT_WIDTH) {
                    val = SPT[i][x + (y * OLD_SPT_WIDTH)];
                }

                std::printf("%3d", val);

                // 最後の要素(16個目 * 14行目)以外にはカンマを付ける
                if (!(y == SPT_HEIGHT - 1 && x == NEW_SPT_WIDTH - 1)) {
                    std::printf(",");
                }
            }
            std::printf("\n");
        }
        std::printf("\t}%s\n", (i < SPT_COUNT - 1 ? "," : ""));
    }

    std::printf("};\n\n#endif\n");
}

// C++用のヘッダーファイル出力（32x32サイズ、センタリング）
void OutputCpp32x32() {
    const int OUTPUT_SIZE = 32;
    
    std::printf("#pragma once\n");
    std::printf("#ifndef sputteringData32_H\n");
    std::printf("#define sputteringData32_H\n\n");
    std::printf("#include \"Fs.h\"\n");
    std::printf("#include \"../FsLibrary/FsAE.h\"\n\n");

    // 定義の更新
    std::printf("#define SPT_COUNT %d\n", SPT_COUNT);
    std::printf("#define SPT_WIDTH %d\n", OUTPUT_SIZE);
    std::printf("#define SPT_HEIGHT %d\n", OUTPUT_SIZE);
    std::printf("#define SPT_SIZE(IDX) (SPT_SIZE_TBL[(IDX)])\n");
    std::printf("#define SPT_VALUE(IDX,X,Y) (SPT[(IDX)][(X) + ((Y) * SPT_WIDTH)])\n\n");

    // SPT_SIZE_TBL配列を出力
    std::printf("static A_u_char SPT_SIZE_TBL[SPT_COUNT] = {\n\t");
    for (int i = 0; i < SPT_COUNT; ++i) {
        std::printf("%d", SPT_SIZE_TBL[i]);
        if (i < SPT_COUNT - 1) {
            std::printf(", ");
            if ((i + 1) % 20 == 0) {
                std::printf("\n\t");
            }
        }
    }
    std::printf("\n};\n\n");

    // 配列サイズは 60 * (32 * 32) になります
    std::printf("static A_u_char SPT[SPT_COUNT][SPT_WIDTH * SPT_HEIGHT] = {\n");

    for (int i = 0; i < SPT_COUNT; ++i) {
        std::printf("\t{\n");
        
        // 有効サイズを取得
        int validSize = SPT_SIZE_TBL[i];
        int offset = (OUTPUT_SIZE - validSize) / 2;
        
        for (int y = 0; y < OUTPUT_SIZE; ++y) {
            std::printf("\t\t");
            for (int x = 0; x < OUTPUT_SIZE; ++x) {
                int val = 0;
                
                // 有効範囲内かチェック
                int srcX = x - offset;
                int srcY = y - offset;
                
                if (srcX >= 0 && srcX < validSize && srcY >= 0 && srcY < validSize &&
                    srcX < OLD_SPT_WIDTH && srcY < SPT_HEIGHT) {
                    val = SPT[i][srcX + (srcY * OLD_SPT_WIDTH)];
                }

                std::printf("%3d", val);

                // 最後の要素以外にはカンマを付ける
                if (!(y == OUTPUT_SIZE - 1 && x == OUTPUT_SIZE - 1)) {
                    std::printf(",");
                }
            }
            std::printf("\n");
        }
        std::printf("\t}%s\n", (i < SPT_COUNT - 1 ? "," : ""));
    }

    std::printf("};\n\n#endif\n");
}

// C#用のクラスファイル出力
void OutputCSharp() {
    std::printf("using System;\n\n");
    std::printf("namespace AEHelper\n");
    std::printf("{\n");
    std::printf("\tpublic static class SputteringData\n");
    std::printf("\t{\n");
    std::printf("\t\tpublic const int SPT_COUNT = %d;\n", SPT_COUNT);
    std::printf("\t\tpublic const int SPT_WIDTH = %d;\n", NEW_SPT_WIDTH);
    std::printf("\t\tpublic const int SPT_HEIGHT = %d;\n\n", SPT_HEIGHT);

    std::printf("\t\tpublic static byte GetValue(int idx, int x, int y)\n");
    std::printf("\t\t{\n");
    std::printf("\t\t\treturn SPT[idx, x + (y * SPT_WIDTH)];\n");
    std::printf("\t\t}\n\n");

    // 2次元配列として出力
    std::printf("\t\tprivate static readonly byte[,] SPT = new byte[SPT_COUNT, SPT_WIDTH * SPT_HEIGHT]\n");
    std::printf("\t\t{\n");

    for (int i = 0; i < SPT_COUNT; ++i) {
        std::printf("\t\t\t{\n");
        for (int y = 0; y < SPT_HEIGHT; ++y) {
            std::printf("\t\t\t\t");
            for (int x = 0; x < NEW_SPT_WIDTH; ++x) {
                int val = 0;
                // 横幅14番目までは元のデータを使用、15・16番目は0（パディング）
                if (x < OLD_SPT_WIDTH) {
                    val = SPT[i][x + (y * OLD_SPT_WIDTH)];
                }

                std::printf("%3d", val);

                // 最後の要素(16個目 * 14行目)以外にはカンマを付ける
                if (!(y == SPT_HEIGHT - 1 && x == NEW_SPT_WIDTH - 1)) {
                    std::printf(",");
                }
            }
            std::printf("\n");
        }
        std::printf("\t\t\t}%s\n", (i < SPT_COUNT - 1 ? "," : ""));
    }

    std::printf("\t\t};\n");
    std::printf("\t}\n");
    std::printf("}\n");
}

int main(int argc, char* argv[]) {
    // デフォルトはC++出力
    bool outputCSharp = false;
    bool output32x32 = false;

    // コマンドライン引数をチェック
    if (argc > 1) {
        if (strcmp(argv[1], "cs") == 0 || strcmp(argv[1], "csharp") == 0) {
            outputCSharp = true;
        } else if (strcmp(argv[1], "cpp32") == 0 || strcmp(argv[1], "32") == 0) {
            output32x32 = true;
        }
    }

    if (outputCSharp) {
        OutputCSharp();
    } else if (output32x32) {
        OutputCpp32x32();
    } else {
        OutputCpp();
    }

    return 0;
}
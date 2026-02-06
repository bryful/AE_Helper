#include <iostream>
#include <cstdio>

// 元のファイルをインクルードして、SPT[60][196] をロード
// ※インクルード前にマクロが衝突しないよう配慮が必要な場合があります
#include "FsSputtering.h"

// 新しい定義
#define NEW_SPT_WIDTH 16
#define OLD_SPT_WIDTH 14
#define SPT_HEIGHT    14
#define SPT_COUNT     60

int main() {
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

    return 0;
}
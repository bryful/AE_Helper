# PicTiler

NF-Sputteringで使用できるスパッタリングデータを作成できるアプリです。<br>
Targa/PNGをexportできますが、PNG推奨です。<br>

# NF-Sputteringのスッパッタリングデータ。

48x48ピクセルを基本としてずらっとタイル状に並べた1枚の画像になります。<br>
いちいちPhotoshopで並べるのは辛いので専用のアプリ(PicTiler.exe)を作りました。<br>
<br>
データとしてグレースケールを使います。<br>
基本的に24bitのグレースケールになります（RGB同じ色）<br>
<br>
NF-Sputteringでは **Green** 成分しか使っていません。<br>


# 使い方。

1. 適当なデータを作る。48x48ピクセル。違うサイズで作ってもPicTilerが勝手に縮小します。
2. リストに登録。"File:AddImage"メニューで登録。ドラッグ＆ドロップでも可能。
3. Up/Down/Remove/Clearで順番等を修正。
4. 書き出すサイズ（グリッドの数）を設定。余った場合は繰り返しで自動配置します。
5. Exportで書き出し。

です。



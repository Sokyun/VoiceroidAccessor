VoiceroidAccessor
=================

Voiceroid を.NETから喋らせるためのライブラリです。
(VoiceroidシリーズはAHS社の商品です AHS http://www.ah-soft.com/)

使い方
=================

プラグイン作成
-----------------

1. class 作る(public で)
2. net.azworks.VoiceroidAccess.voiceroids.Voiceroid を継承する
3. virtual なメソッドを一通り喋るように実装してください

多分ソリューション中の Zunko とか Yukari 見れば参考になると思う。


Use plugins
-----------------

1. voiceroids ディレクトリをアプリの下に作ります
2. 上記のプラグイン dll を voiceroids に突っ込みます
3. おもむろにVoiceroidManagerクラスのインスタンスを作る "VoiceroidManager voiceroidManager = new VoiceroidManager();"
4. メッセージを喋らせる "voiceroidManager.Talk("VOICEROID＋ 結月ゆかり", "message string");"

第三引数は、喋るコマンドを突っ込んだあと、次のメッセージを喋るまでのディレイです。
逆言うと先行して20個位メッセージ詰め込んで置くことも可能。
第三引数省略すると自動ディレイ計算。正確である保証なしです。
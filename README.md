# BW2PrmSearcher
BW2PrmSearcherはBW2専用のパラメータ特定ツールです.

Config.txtで各種パラメータを設定することで,
 - ゲーム起動時のパラメータの特定

を行うことができます.

PWTレンタルトーナメントから最低2 ~ 3匹のポケモンを入力すれば検索が行えます.

また, 6匹入力の場合6倍速で検索を行います.

## Config.txt
 - Nazo値：バージョンに対応した値を設定します.
 - VCount：バージョンに対応した値を設定します.
 - InTimer0, EnTimer0：バージョンに対応した値を設定します. In ~ En で検索したいTimer0の範囲を設定できます.

    【BW/BW2】全28バージョンパラメータ纏め (https://blog.bzl-web.com/entry/2020/09/18/235128)
 - Mac：6ケタのMacアドレスの内, 上4ケタをMac1, 下2ケタをMac2に設定します.

    [例]Mac：AA-BB-CC-DD-EE-FFのとき  Mac1：0xAABCCDD, Mac2：0xEEFF に設定します.
 - GxFra：固定値？
 - Frame：初代DSなら8、DSLiteなら6を設定します.
 - key：キー入力に対応した値を設定します. デフォルトは無入力です.
 - Prm：固定値.

 - InDateTime：検索開始時間を設定します.
 - EnDateTime：検索終了時間を設定します.
 - AddTimes：InDateTimeにインクリメントする時間を設定します.

   [例]InDateTime = 00:00:30, AddSeconds = 0にすれば起動時間を30秒に固定できます.
 - PIDCount：性格値乱数消費数の上限を設定します.
 - CountFlag：出力したい検索結果の数を設定します.

## 5genSeedUnti
BW2の初期SEED検索の簡易版です. 
Config.txtの各種パラメータを参照して性格値乱数初期SEEDの検索を行います.

ハッシュ化の処理は (https://github.com/yatsuna827/5genInitialSeedSearch) を ~~パクリ~~ 参考にしています.

## 技術的解説


## バージョン情報
 - v1.0.0 初版

## 製作者
ジラーテ(@namofure)

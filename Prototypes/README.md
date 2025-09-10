# Prototypes フォルダ

このフォルダには、新しいアプリケーション開発のための技術確認用プロトタイプが含まれています。

## LayeredWindowSample

Windows Layered Window機能を使用したサンプルアプリケーションプロジェクトです。

### 概要
Windowsの透明ウィンドウ機能（Layered Window）を活用し、PNG画像の透明背景を実際に透明として表示するサンプルアプリケーションです。.NET 8と.NET Framework 4.8の両方の実装を提供し、フレームワーク間の互換性と機能の違いを確認できます。

### 主な技術検証項目
- Windows Win32 APIの P/Invoke 呼び出し
- Layered Window機能の実装
- 透明度制御とカラーキー透明
- クリックイベントによる動的なウィンドウモード切り替え
- .NET 8 と .NET Framework 4.8 での実装の違い

### 詳細
LayeredWindowSampleフォルダ内のREADME.mdを参照してください。
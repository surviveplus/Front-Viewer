# Layered Window Sample Applications

このプロジェクトは、Windows Layered Window機能を使用したサンプルアプリケーションです。

## プロジェクト構成

### AppNet8
- .NET 8を使用したWindows Formsアプリケーション
- 最新の.NET機能とC#機能を活用

### AppDotNetFramework48
- .NET Framework 4.8を使用したWindows Formsアプリケーション
- 従来の.NET Frameworkとの互換性を確保

## 機能

### 主要機能
1. **透明背景のPNG画像表示**: Layered Window機能により、PNG画像の透明部分が実際に透明になって表示されます
2. **モード切り替え**: 画像をクリックすることで、以下の2つのモードを切り替えることができます
   - **Layered Windowモード**: 透明背景で、画像の不透明部分のみが表示される
   - **通常ウィンドウモード**: 通常のWindowsウィンドウとして表示される

### 技術的特徴
- Windows Win32 API (user32.dll) の使用
  - `GetWindowLong` / `SetWindowLong`: ウィンドウスタイルの取得・設定
  - `SetLayeredWindowAttributes`: レイヤードウィンドウの透明度設定
- P/Invoke を使用したネイティブAPI呼び出し
- カラーキー透明度 (マゼンタ色 #FF00FF を透明として扱い)

## サンプル画像

`sample.png` は 1024x1024 ピクセルのPNG画像で、以下の特徴があります：
- 透明な背景
- 中央に配置された黒い「Sample」テキスト
- フォント: Arial Bold, 120pt相当

## 実行方法

### 前提条件
- Windows 10 以降
- .NET 8 Runtime または .NET Framework 4.8
- Visual Studio 2022 または .NET CLI

### 手順

1. **リポジトリのクローン**
   ```bash
   git clone [repository-url]
   cd Front-Viewer/Prototypes/LayeredWindowSample
   ```

2. **sample.pngの配置**
   各アプリケーションの実行フォルダに `sample.png` をコピーします：
   ```bash
   cp sample.png AppNet8/bin/Debug/net8.0/
   cp sample.png AppDotNetFramework48/bin/Debug/net48/
   ```

3. **ビルドと実行**

   **.NET 8アプリケーション:**
   ```bash
   cd AppNet8
   dotnet build
   dotnet run
   ```

   **.NET Framework 4.8アプリケーション:**
   ```bash
   cd AppDotNetFramework48
   dotnet build
   dotnet run
   ```

   または、Visual Studioでソリューションファイル `LayeredWindowSample.sln` を開いて実行

## 使用方法

1. アプリケーションを起動すると、Layered Windowモードで画像が表示されます
2. ウィンドウはフレームレスで、背景が透明になっています
3. 画像をクリックすると、通常のウィンドウモードに切り替わります
4. 再度クリックすると、Layered Windowモードに戻ります

## トラブルシューティング

### よくある問題

**Q: 画像が表示されない**
A: `sample.png` ファイルが実行フォルダに存在することを確認してください。ファイルが見つからない場合、アプリケーションは自動的にフォールバック画像を生成します。

**Q: 透明度が効かない**
A: Windows 10以降でDWM (Desktop Window Manager) が有効になっていることを確認してください。古いWindowsバージョンではLayered Window機能が制限される場合があります。

**Q: クリックしても切り替わらない**
A: 画像の不透明部分をクリックしてください。透明部分はクリックイベントを受け取りません。

## ライセンス

このサンプルコードはMITライセンスの下で公開されています。
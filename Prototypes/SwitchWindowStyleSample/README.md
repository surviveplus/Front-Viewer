# SwitchWindowStyleSample

Window の枠の表示・非表示の切り替えを2つのウィンドウの切り替えで再現するサンプルアプリケーションです。

## 概要

既存の LayeredWindowSample では、WindowStyle を動的に変更する際にエラーが発生する問題がありました。
このサンプルでは、2つの独立したウィンドウを使用してこの問題を解決しています。

## 機能

- **通常ウィンドウ**: 標準的な枠付きウィンドウでサンプル画像を表示
- **透明ウィンドウ**: 枠なし・背景透明のウィンドウでサンプル画像を表示
- **ワンクリック切り替え**: 画像をクリックすることで2つのウィンドウを切り替え
- **位置の一致**: 枠のサイズを計算し、画像の表示位置が切り替え時に変わらないように調整
- **適切なリソース管理**: 一方のウィンドウが閉じられた時に、もう一方も適切に解放

## 技術仕様

- **言語**: C#
- **フレームワーク**: .NET 8
- **UI フレームワーク**: WPF
- **ターゲット環境**: Windows (Any CPU)

## 解決した問題

1. **SetLayeredWindowAttributes 不要**: WPF の `AllowsTransparency = true` を使用することで、Win32 API を直接呼び出す必要がない
2. **WindowStyle 変更エラー回避**: ウィンドウ表示後の WindowStyle 変更によるエラーを、2つの独立したウィンドウで回避
3. **画像位置の一致**: システムパラメータを使用して枠のサイズを計算し、切り替え時の画像位置を維持

## 使用方法

### Windows 環境での実行

```bash
cd Prototypes/SwitchWindowStyleSample
dotnet run
```

### ビルド

```bash
dotnet build SwitchWindowStyleSample.sln
```

### クリック操作

- **通常ウィンドウ表示中**: 画像をクリックすると透明ウィンドウに切り替わります
- **透明ウィンドウ表示中**: 画像をダブルクリックすると通常ウィンドウに切り替わります（シングルクリックではドラッグ開始）
- **透明ウィンドウ**: ドラッグで移動可能

## ファイル構成

- `SwitchWindowStyleSample.sln` - ソリューションファイル
- `SwitchWindowStyleSample.csproj` - プロジェクトファイル
- `Program.cs` - エントリーポイント
- `App.xaml` / `App.xaml.cs` - WPF アプリケーション
- `WindowManager.cs` - ウィンドウ管理クラス
- `NormalWindow.cs` - 通常ウィンドウクラス
- `TransparentWindow.cs` - 透明ウィンドウクラス
- `sample.png` - サンプル画像（LayeredWindowSample からコピー）

## 非 Windows 環境

Windows 以外の環境では、コンソールアプリとして動作し、機能説明を表示します。
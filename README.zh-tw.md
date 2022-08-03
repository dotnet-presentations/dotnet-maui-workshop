# .NET MAUI 的 Hands-on Lab 手冊

今天我們將會來建構一個 [.NET MAUI](https://docs.microsoft.com/dotnet/maui?WT.mc_id=EM-MVP-5001645) 的應用程式，它將會透過列表的方式來展示來自世界各地的猴子資料。首先我們將從建構後端服務的應用邏輯開始，此後端服務會以 RESTful API 來回應利用 json 格式表示的猴子資料。接著，我們將利用 [.NET MAUI](https://docs.microsoft.com/xamarin/essentials/index?WT.mc_id=EM-MVP-5001645) 來搜尋到距離我們最近的猴子，並透過地圖的呈現來展示猴子所在的地點。我們也將會學到更多利用不同的方式來呈現資料，並在最後開發出一個具有自動適應深色或淺色模式的佈景主題 App。

## Hands-on Lab 環境設定
操作此 Hands-on Lab 是需要自己實際動手與設定相關設備的。您將可以在 PC（推薦）或 Mac 上進行開發，您需要做的就是先安装好在 Visual Studio 2022 或 Visaul Studio for Mac 2022，並確認有設定 .NET MAUI 工作負載的安裝。

在開始著手進行此 Hands-on Lab 前，會建議先用 10 分鐘左右的時間來閱讀一下 [.NET MAUI 教學](https://docs.microsoft.com/dotnet/maui/get-started/first-app?WT.mc_id=EM-MVP-5001645)，在此教學中會指導您安裝與正確地設置好，在進行此 Hands-on Lab 所需的的開發環境。

如果您是行動應用開發的新手，會建議您直接把應用程式直接部署到 Android 實體裝置中 (通常只需幾個步驟就可以完成相關的設置) 進行測試。當然，如果您没有 Android 的實體裝置，也不用擔心，您另外透過設定 [具有硬體加速功能的 Android 模擬器](https://docs.microsoft.com/xamarin/android/get-started/installation/android-emulator?WT.mc_id=EM-MVP-5001645) 的方式來進行測試。而如果您真的没有時間提前準備好相關的設定，也是沒關係的，在此 Hands-on Lab 的進行過程中會有相關的對應提示和可能的幫助。

## Hands-on Lab 内容

有關此 Hands-on Lab 的内容介紹：

* [Hands-on Lab Part 0: 約 30 分鐘的環節](Part%200%20-%20Overview/README.md) - 有關 .NET MAUI 的基本知識與其開發環境的安裝介绍 
* [Hands-on Lab Part 1: 呈現出資料](Part%201%20-%20Displaying%20Data/README.md) - 實作單頁資料呈現列表
* [Hands-on Lab Part 2: MVVM 和資料繫結](Part%202%20-%20MVVM/README.md) - MVVM 框架設計和資料繫結
* [Hands-on Lab Part 3: 增加導覽頁面](Part%203%20-%20Navigation/README.md) - 幫 App 增加頁面巡覽功能
* [Hands-on Lab Part 4: 了解平台特性](Part%204%20-%20Platform%20Features/README.md) - 實現不同平台的特定功能
* [Hands-on Lab Part 5: 幫 CollectionView 增加下拉更新](Part%205%20-%20CollectionView/README.md) - CollectionView 多資料呈現控制項的使用技巧
* [Hands-on Lab Part 6: 設定 App 的佈景主題](Part%206%20-%20AppThemes/README.md) - 有關 App 的深淺色模式的佈景主題設置


要開始進行此 Hands-on Lab，請開啟「Part 1 - Displaying Data」資料夾並開啟 「MonkeyFinder.sln」。您可以在整個 Hands-on Lab 進行的過程中持續的使用本專案。而每個 **Hands-on Lab 的 Part** 都有一個 **README** 文件，其中包含 Hands-on Lab 在進行此 Part 過程的說明。您也可以在任何一個 Part 的資料夾當中，找到對應該個 Part 的專案程式，您也可以直接開啟該專案來確認該 Part 完成後應有的結果。

## 教學影片
James Montemagno 已有錄製大約 4 小時有關此 Hands-on Lab 的 Step by Step 教學影片，各位捧友們可以到 [James Montemagno 的 YouTube 频道](https://youtube.com/jamesmontemagno) 觀看學習

## 更多連結和資源：
- [.NET MAUI 官方網站](https://dot.net/maui)
- [Microsoft Learn 上的 .NET MAUI 的學習路徑](https://aka.ms/Learn.MAUI)
- [.NET MAUI 官方文件](https://aka.ms/Docs.MAUI)
- [GitHub 上的 .NET MAUI 專案](https://github.com/dotnet/maui)
- [.NET 初心者的入門系列影片集](https://dot.net/videos)


如果您有任何問题，也可以到 Twitter 上與 [@JamesMontemagno](https://twitter.com/jamesmontemagno) 聯繫，也可到 Facebook 的 [Xamarin Asia Developers 社群](https://www.facebook.com/groups/XamarinAsiaDevelopers/) 來提問。



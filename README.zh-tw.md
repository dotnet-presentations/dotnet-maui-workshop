# .NET MAUI 的 Hands-on Lab 手冊

今天我們將會透過 [.NET MAUI](https://docs.microsoft.com/dotnet/maui?WT.mc_id=EM-MVP-5001645) 技術建構出一個跨平台執行的應用程式，它將會透過列表的方式來展示來自世界各地的猴子資料。首先我們將從使用網際網路服務的應用邏輯開始，此服務會以 RESTful API 的方式來回應用 json 格式表示的猴子資料。接著，在此 [.NET MAUI](https://docs.microsoft.com/xamarin/essentials/index?WT.mc_id=EM-MVP-5001645) 的應用程式中，透過 [.NET MAUI](https://docs.microsoft.com/xamarin/essentials/index?WT.mc_id=EM-MVP-5001645) 所設計的 API 使用，來撰寫搜尋如何計算與搜尋到當前距離最近的猴子，並透過 API 從此應用程式轉跳到平台裝置內建的地圖應用來呈現來展示猴子所在的地點。當然也將會學到更多利用不同的方式來呈現資料，並在最後學習到讓應用程式如何自動適應系統佈景主題的深色或淺色模式的調整處理。

## Hands-on Lab 環境設定
操作此 Hands-on Lab 是需要自己實際動手與設定相關設備的。您將可以在 PC（推薦）或 Mac 上進行開發，您需要做的就是先安裝好在 Visual Studio 2022 或 Visaul Studio for Mac 2022，並確認有設定 .NET MAUI 工作負載的安裝。

在開始著手進行此 Hands-on Lab 前，會建議先用 10 分鐘左右的時間來閱讀一下 [.NET MAUI 教學](https://docs.microsoft.com/dotnet/maui/get-started/first-app?WT.mc_id=EM-MVP-5001645)，在此教學中會指導您安裝與正確地設置好，要進行此 Hands-on Lab 所需的開發環境。

如果您是行動應用開發的新手，會建議您直接把應用程式直接部署到 Android 實體裝置中 (通常只需幾個步驟就可以完成相關的設置) 進行測試。當然，如果您沒有 Android 的實體裝置可以使用，也不用擔心，可以透過設定 [具有硬體加速功能的 Android 模擬器](https://docs.microsoft.com/xamarin/android/get-started/installation/android-emulator?WT.mc_id=EM-MVP-5001645) 的方式來使用模擬器進行測試。而如果真的沒有時間能提前準備好相關的設定，也沒關係的，在此 Hands-on Lab 的進行過程中，都會盡可能的有相關的對應提示和可能幫助介紹。

## Hands-on Lab 各個 Part 內容

有關此 Hands-on Lab 的各個 Part 內容介紹：

* [Hands-on Lab Part 0: 約 30 分鐘的環節](Part%200%20-%20Overview/README.zh-tw.md) - 有關 .NET MAUI 的基本知識與其開發環境的安裝介紹 
* [Hands-on Lab Part 1: 呈現出資料](Part%201%20-%20Displaying%20Data/README.zh-tw.md) - 實作單頁資料呈現列表
* [Hands-on Lab Part 2: MVVM 和資料繫結](Part%202%20-%20MVVM/README.zh-tw.md) - MVVM 框架設計和資料繫結
* [Hands-on Lab Part 3: 增加巡覽功能](Part%203%20-%20Navigation/README.zh-tw.md) - 幫 App 增加巡覽功能進行頁面轉跳
* [Hands-on Lab Part 4: 了解平台特性](Part%204%20-%20Platform%20Features/README.zh-tw.md) - 實現不同平台的特定功能
* [Hands-on Lab Part 5: 幫 CollectionView 增加下拉更新](Part%205%20-%20CollectionView/README.zh-tw.md) - 增加下拉更新資料功能並學習 CollectionView 多資料呈現控制項的使用技巧
* [Hands-on Lab Part 6: 設定 App 的佈景主題](Part%206%20-%20AppThemes/README.zh-tw.md) - 有關 App 如何針對淺色/深色模式的處理佈景主題的設置


要開始進行此 Hands-on Lab，請開啟「Part 1 - Displaying Data」資料夾並開啟 「MonkeyFinder.sln」。可以在整個 Hands-on Lab 進行的過程中持續的使用此一開始的專案。而為確保每個階段的進行，除了在 **Hands-on Lab 的 Part** 都有一個 **README** 文件，有對 Hands-on Lab 在此 Part 的過程中進行內容的說明。同時，也在任何一個 Part 的資料夾當中，都找到對應該個 Part 的專案程式，您也可以直接開啟該 Part 資料夾中的專案，來開始進行此 Part 要完成的內容，然後在透過下一個 Part 的專案來確認完成後應有的結果。

## 教學影片
James Montemagno 已有錄製大約 4 小時有關此 Hands-on Lab 的 Step by Step 教學影片，各位捧友們可以到 [James Montemagno 的 YouTube 频道](https://youtube.com/jamesmontemagno) 觀看學習

## 更多連結和資源：
- [.NET MAUI 官方網站](https://dot.net/maui)
- [Microsoft Learn 上的 .NET MAUI 的學習路徑](https://aka.ms/Learn.MAUI)
- [.NET MAUI 官方文件](https://aka.ms/Docs.MAUI)
- [GitHub 上的 .NET MAUI 專案](https://github.com/dotnet/maui)
- [.NET 初心者的入門系列影片集](https://dot.net/videos)


如果您有任何問题，也可以到 Twitter 上與 [@JamesMontemagno](https://twitter.com/jamesmontemagno) 聯繫，也可到 Facebook 的 [Xamarin Asia Developers 社群](https://www.facebook.com/groups/XamarinAsiaDevelopers/) 來提問。



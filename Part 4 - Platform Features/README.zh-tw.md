## Hands-on Lab Part 4: 了解平台特性

在此回的 Hands-on Lab 中，將在 .NET MAUI 的應用中找到距離最近的猴子資料，並開啟包含該猴子位置的地圖呈現。

### 網際網路的檢查

透過 .NET MAUI 的 Library 內建的 `IConnectivity` 來輕鬆的檢查應用程式使否有連上網際網路。

1. 首先，在先前設計的 `MonkeysViewModel` 類別當中，新增一個型態為 `IConnectivity` 的 `connectivity` 欄位，並透過改寫 `MonkeysViewModel` 建構式，透過建構式注入的方式設定此 `connectivity` 欄位：

    ```csharp
    IConnectivity connectivity;
    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
        this.connectivity = connectivity;
    }
    ```

2. 再次到 `MauiProgram.cs` 中，繼續透過 `AddSingletion<T>()` 方法來註冊 `Connectivity.Current`。

    ```csharp
    builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
    ```

3. 同時在這邊也透過 `AddSingletion<T>()` 方法增加 `IGeolocation` 和 `IMap` 的註冊程式碼：

    ```csharp
    builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
    builder.Services.AddSingleton<IMap>(Map.Default);
    ```

4. 而回到當初設計 `GetMonkeysAsync` 的方法中檢查網際網路連線狀態，如果離線則顯示 Alert 作為提醒。


    ```csharp
    if (connectivity.NetworkAccess != NetworkAccess.Internet)
    {
        await Shell.Current.DisplayAlert("No connectivity!",
            $"Please check internet and try again.", "OK");
        return;
    }
    ```

   可以在模擬器上透過開/關飛航模式，執行應用程式來進行此功能的測試。


### 找尋最近的猴子!

透過使用設備的 GPS 功能，再搭配每隻猴子的資料都有所在的經緯度位置，所以可以針對此頁面提供更多有趣的資料呈現。

1. 在先前設計的 `MonkeysViewModel` 類別當中，新增一個型態為 `IGeolocation` 的 `geolocation` 欄位，並透過改寫 `MonkeysViewModel` 建構式，透過建構式注入的方式設定此 `geolocation` 欄位：

    ```csharp
    IConnectivity connectivity;
    IGeolocation geolocation;
    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
        this.connectivity = connectivity;
        this.geolocation = geolocation;
    }
    ```

2. 接著再 `MonkeysViewModel.cs` 中，再設計一個 `GetClosestMonkey()` 的新方法，並且在該方法上掛上 `[RelayCommand]` 的使用：

    ```csharp
    [RelayCommand]
    async Task GetClosestMonkey()
    {

    }
    ```

3. 然後，在此方法當中撰寫如下程式碼，來取得設備的位置資訊並查詢距離最近的猴子資料：

    ```csharp
    [RelayCommand]
    async Task GetClosestMonkey()
    {
        if (IsBusy || Monkeys.Count == 0)
            return;

        try
        {
            // Get cached location, else get real location.
            var location = await geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }

            // Find closest monkey to us
            var first = Monkeys.OrderBy(m => location.CalculateDistance(
                new Location(m.Latitude, m.Longitude), DistanceUnits.Miles))
                .FirstOrDefault();

            await Shell.Current.DisplayAlert("", first.Name + " " +
                first.Location, "OK");

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to query location: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }
    ```

4. 回到 `MainPage.xaml` 的畫面設計中增加一個 `Button`，並且透過此 `Command` 設定其繫結來呼叫前一個步驟所設計的找尋距離最近的猴子資料方法：

    在先前的 `Search` 按鈕的 XAML 標記下方加入下面的 XAML 標記碼。

    ```xml
    <Button Text="Find Closest" 
        Command="{Binding GetClosestMonkeyCommand}"
        IsEnabled="{Binding IsNotBusy}"
        Grid.Row="1"
        Grid.Column="1"
        Style="{StaticResource ButtonOutline}"
        Margin="8"/>
    ```

重新執行應用程式，在讀取猴子的資料完成後，即可透過操作確認相關的位置資訊。

此專案也預先設定了取得地理位置所需的權限與功能。可以透過閱讀相關文件了解更多設定的資料，而這邊有一些快速的指引:

1. .NET MAUI 已在所有 .NET MAUI 應用程式專案中進行預先規劃其執行平台環境的設定，其中也包含了權限的處理問題。
2. 針對 Android 環境，透過 **MonkeyFinder -> Platforms -> Android -> AssemblyInfo.cs** 來設定 Android 的 manifest 資訊來設置相關資訊。
3. 針對 iOS/macOS 的每個平台環境，要透過 **info.plist** 檔案來設置相關資訊。
4. 針對 Windows 環境，透過 **Package.appxmanifest** 檔案設置相關資訊。


### 開啟地圖

.NET MAUI 提供透過單一的 API 整合超過 60 多個平台的特性，來開啟其預設內建的地圖應用程式！

1. 在 `MonkeyDetailsViewModel` 中設計 `IMap` 的欄位 `map`，並修改 `MonkeyDetailsViewModel` 的建構方法，透過建構式注入設定該 `map` 欄位：

    ```csharp
    IMap map;
    public MonkeyDetailsViewModel(IMap map)
    {
        this.map = map;
    }
    ```

2. 繼續在 `MonkeyDetailsViewModel.cs` 檔案中增加一個 `OpenMap()` 的方法，並在該方法上掛上 `[RelayCommand]`。而此方法則透過 `map` 呼叫 `IMap` 所設計的 API 方法 `OpenAsync()`，並在其呼叫時帶入猴子的位置資訊：

    ```csharp
    [RelayCommand]
    async Task OpenMap()
    {
        try
        {
            await map.OpenAsync(Monkey.Latitude, Monkey.Longitude, new MapLaunchOptions
            {
                Name = Monkey.Name,
                NavigationMode = NavigationMode.None
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to launch maps: {ex.Message}");
            await Shell.Current.DisplayAlert("Error, no Maps app!", ex.Message, "OK");
        }
    }

    ```

### 調整 DetailsPage.xaml 的使用者介面

在猴子的名稱的上方，增加一個 `OpenMapCommand` 的按鈕，找到當初設計的 `<Label>` 標記，並且在其上方加入如下 `Button` 的 XAML 標記碼。

```xml
<Button Text="Show on Map" 
        Command="{Binding OpenMapCommand}"
        HorizontalOptions="Center" 
        WidthRequest="200" 
        Margin="8"
        Style="{StaticResource ButtonOutline}"/>
                
<Label Style="{StaticResource MediumLabel}" Text="{Binding Monkey.Details}" />
```

執行應用程式後，選取資料列表中的任一個猴子資料，轉跳到猴子的詳細頁面後，再點選 **Show on Map** 的按鈕，就會看到從應用程式轉跳到特定平台內建預設的地圖應用。

## iOS 安全區域 (Safe Area)

除了能使用跨各個平台設備的單一 API 進行操作外，在 .NET MAUI 的設計中還包含特定平台的程式庫。例如若使用帶有瀏海設計 (iPhone X 之後開始有的設計) 的 iOS 裝置上執行 Monkey Finder 應用程式時，可能已經發現底部的按鈕跟設備本身的 **Home Indicator** 重疊。所以在 iOS 的 UI 設計中有安全區域 (Safe Area) 的概念，正常來說畢續透過撰寫程式的方式設定。但可以透過 .NET MAUI 針對平台的特殊性設計，將可以直接在 XAML 中設定。

1. 開啟 `MainPage.xaml` 並替 iOS 平台設定一個新的命名空間引用：

    ```xml
    xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
    ```

2. 接著在 `ContentPage` 的標記中，則可以設置以下針對 iOS 平台的特殊屬性，並將其值設定為 `True`：

    ```xml
    ios:Page.UseSafeArea="True"
    ```
    
在 iOS 模擬器或設備中重新執行此應用程式，這邊可以注意到原本版面設計，其中按鈕所在的位置已稍微自動上移錯開了與 **Home Indicator** 的重疊。

緊接著在進入下一個 Hands-on Lab 的部分吧! 在 [Hands-on Lab Part 5: 幫 CollectionView 增加下拉更新](../Part%205%20-%20CollectionView/README.zh-tw.md) 中了解 CollectionView 的運用吧。

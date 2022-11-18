## 实验四: 访问平台特性

在实验四中，我们将使用 .NET MAUI 找到离我们最近的猴子，并打开一张包含猴子位置的地图。

### 网络检查

我们可以使用 .NET MAUI 的内置 `IConnectivity` 轻松检查我们的用户是否连接到互联网

1. 首先，让我们访问 .NET MAUI 中的“IConnectivity”。 让我们将 `IConnectivity` 注入到 `MonkeysViewModel` 构造函数中：

    ```csharp
    IConnectivity connectivity;
    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
        this.connectivity = connectivity;
    }
    ```

2. 在我们的 `MauiProgram.cs` 中注册 `Connectivity.Current`。

3. 在这里，让我们同时添加 `IGeolocation` 和 `IMap`，添加代码：

    ```csharp
    builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
    builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
    builder.Services.AddSingleton<IMap>(Map.Default);
    ```

4. 现在，让我们在 `GetMonkeysAsync` 方法中检查互联网，如果离线则显示警报。


    ```csharp
    if (connectivity.NetworkAccess != NetworkAccess.Internet)
    {
        await Shell.Current.DisplayAlert("No connectivity!",
            $"Please check internet and try again.", "OK");
        return;
    }
    ```

    在您的模拟器上运行应用程序并打开和关闭飞行模式进行测试。

### 查找最近的猴子!

我们可以使用设备的 GPS 向此页面添加更多功能，因为每只猴子都有与之关联的纬度和经度。

1. 首先，让我们访问 .NET MAUI 中的“IGeolocator”。 让我们将 `IGeolocator` 注入到 `MonkeysViewModel` 构造函数中：

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

2. 在我们的`MonkeysViewModel.cs`中，让我们创建另一个名为`GetClosestMonkey`的方法：

    ```csharp
    [RelayCommand]
    async Task GetClosestMonkey()
    {

    }
    ```

3. 然后，我们可以使用 .NET MAUI 来查询我们的位置以及找到离我们最近的猴子的助手：

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

4. 回到我们的`MainPage.xaml`，我们可以添加另一个`Button`来调用这个新方法：

     在“搜索”按钮下添加以下 XAML。

    ```xml
    <Button Text="Find Closest" 
        Command="{Binding GetClosestMonkeyCommand}"
        IsEnabled="{Binding IsNotBusy}"
        Grid.Row="1"
        Grid.Column="1"
        Style="{StaticResource ButtonOutline}"
        Margin="8"/>
    ```

加载猴子后，重新运行应用程序以查看地理位置的运行情况！

该项目预先配置了地理定位所需的所有必要权限和功能。 您可以阅读文档以了解有关设置的更多信息，但这里有一个快速概述。

1. .NET MAUI 已在所有 .NET MAUI 应用程序中进行预配置，包括处理权限。
2. **MonkeyFinder -> Platforms -> Android -> AssemblyInfo.cs**中预先配置了Android manifest 信息
3. **info.plist** 文件中为每个平台配置了 iOS/macOS 配置信息
4. **Package.appxmanifest**中配置了 Windows 配置信息


### 打开地图

.NET MAUI 通过单个 API 提供 60 多个平台功能，并内置打开默认地图应用程序！

1. 将 `IMap` 注入到我们的 `Monkey Details ViewModel` 中：

    ```csharp
    IMap map;
    public MonkeyDetailsViewModel(IMap map)
    {
        this.map = map;
    }
    ```

2. 打开 `MonkeyDetailsViewModel.cs` 文件并添加一个名为 `OpenMap` 的方法，该方法调用 `Map` API，将猴子的位置传递给它：

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

### 更新 DetailsPage.xaml UI

在猴子的名字上方，让我们添加一个调用 `OpenMapCommand` 的按钮。

```xml
<Button Text="Show on Map" 
        Command="{Binding OpenMapCommand}"
        HorizontalOptions="Center" 
        WidthRequest="200" 
        Margin="8"
        Style="{StaticResource ButtonOutline}"/>
                
<Label Style="{StaticResource MediumLabel}" Text="{Binding Monkey.Details}" />
```

运行应用程序，导航到一只猴子，然后按在地图上显示以在特定平台上去启动地图应用程序。

## iOS 安全区域布局

除了访问跨平台设备 API，.NET MAUI 还包括特定于平台的集成。 如果您一直在带有凹槽的 iOS 设备上运行 Monkey Finder 应用程序，您可能已经注意到底部的按钮与设备底部的栏重叠。 iOS 有安全区域的概念，您必须以编程方式设置它。 但是，由于平台的特殊性，您可以直接在 XAML 中设置它们。

1. 打开 `MainPage.xaml` 并为 iOS 细节添加一个新的命名空间：

    ```xml
    xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
    ```

2. 在 `ContentPage` 节点上，您现在可以设置以下属性：

    ```xml
    ios:Page.UseSafeArea="True"
    ```
在 iOS 模拟器或设备上重新运行应用程序，你已经注意到按钮已自动上移。

让我们进入下一个模块，在 [实验五: 为 CollectionView 添加下拉刷新](../Part%205%20-%20CollectionView/README.zh-cn.md) 中了解 Collection View

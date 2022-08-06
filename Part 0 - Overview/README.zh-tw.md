## Hands-on Lab 的準備 

讓我們首先了解 .NET MAUI 的基本概念以及整個專案的相關結構。

### 在 Visual Studio 或 Visual Studio for Mac 當中開啟此專案

1. 開啟 **Part 1 - Displaying Data/MonkeyFinder.sln**

此 MonkeyFinder 的解決方案包含 1 個專案 (MonkeyFinder.csproj)：

* MonkeyFinder 專案 - 基於 .NET MAUI 針對 Android、iOS、macOS 和 Windows 的跨平台應用開發的主要專案。它包含應用程式開發所需要的所有專案結構設計（Models 、 Views 、ViewModels 和 Services）。

![MonkeyFinder 解决方案的具體架構](../Art/Solution.PNG)

**MonkeyFinder 專案** 也都包含了我們在進行 Hands-on Lab 期間會額外使用到的空白程式碼檔案和 XAML 文件檔案，所以在進行 Hands-on Lab 的過程中，我們都會直接透過此專案中已存在的檔案去增加或修改相關的程式碼。

### 了解.NET MAUI 單一專案 (Single Project) 結構
 
.NET MAUI (.NET Multi-platform App UI) 單一專案設計，讓您在開發跨平台應用的過程中，除了能提供不同平台設計應用時有共同的開發體驗外，並針對 Android、iOS、macOS 和 Windows 各個平台抽象成一個共享的開發專案。

.NET MAUI 單一專案提供了簡化且具有一致性的跨平台應用開發體驗，.NET MAUI 單一專案提供以下功能：

- 針對 Android、iOS、macOS 和 Windows 的單一共用專案。
- 簡化在執行 .NET MAUI 應用程式的偵錯目標的選擇。
- 在單一專案中共享不同平台的有關資源檔案。
- 可以呼叫不同平台專有的 API 和相關工具。
- 單一的跨平台應用程式進入點。

.NET MAUI 單一專案允許使用多目標偵錯和引用其他支援 .NET 6 SDK 的專案。

#### 資源檔案

過去跨平台應用開發的資源管理一直存在不少的問題。每個平台對於資源檔案的使用都有自己的設置方式，必須要針對每個平台上設置。例如，每個平台都有不同的圖片需求，通常涉及不同解析度的問題，就要創立每個圖片的多個版本。因此，單張圖片通常必須在每個平台中用不同的解析度設計多次，產出的圖片也必須在每一個平台上用不同的檔案名稱或資料夾來設置。

.NET MAUI 單一專案使資源檔案可以存在一統的位置上，並且讓不同的平台系統使用。這當中包含字型、圖片、應用程式的 Icon、啟動畫面和原始資產 (Raw Assets)。

> 重要：
> 每個圖片檔案都需要設置其來源圖檔，因為在編譯時期會為每個平台產生所需用到的解析度圖片檔。

資源檔案應該要放置在 .NET MAUI 單一專案中的 _Resources_ 資料夾或 _Resources_ 資料夾中的子資料夾，並且必須正確的設定其 Build Action。下表列出了各種資源檔案型態所需設置的 Build Action：

| 資源類型 | Build Action 設置 |
| -------- | ------------ |
| 應用程式 Icon(App icon) | MauiIcon |
| 應用程式字形(Fonts) | MauiFont |
| 應用程式圖片(Images) | MauiImage |
| 應用程式啟動畫面(Splash screen) | MauiSplashScreen |
| 應用程式原始資產(Raw assets) | MauiAsset |

> 注意：
> XAML 的文件檔案也同時存在您的 .NET MAUI 應用專案中，在專案和檔案範本建立時會自動設定為 **MauiXaml** 的 Build Action。但是，XAML 文件檔案們並不會放在該應用專案的 _Resources_ 資料夹中。

將資源檔案新增到 .NET MAUI 應用專案時，會在專案檔中 (.csproj) 建立資源對應的相關標記。增加資源檔案後，可在 **Properties** 窗格當中設置其 Build Action，其下的截圖呈現了一個 _Resources_ 資料夾，並在其中包含子資料夾的圖片和字型資源：

![圖片和字型資源相關的截圖.](../Art/ResourcesSingleProject.png)

_Resources_ 資料夾的子資料夾可以透過編輯應用程式的專案檔 (.csproj) 來直接對每種資源類型的指定：

```xml
<ItemGroup>
    <!-- Images -->
    <MauiImage Include="Resources\Images\*" />

    <!-- Fonts -->
    <MauiFont Include="Resources\Fonts\*" />

    <!-- Raw Assets (also remove the "Resources\Raw" prefix) -->
    <MauiAsset Include="Resources\Raw\**" LogicalName="%(RecursiveDir)%(Filename)%(Extension)" />
</ItemGroup>
```

透過星字符號 (`*`) 的設定，表示此資料夾中的所有檔案都將被指定為該類型的資源檔。此外，要包含子資料夾當中的所有檔案的類型指定也是可以做到的：

```xml
<ItemGroup>
    <!-- Images -->
    <MauiImage Include="Resources\Images\**\*" />
</ItemGroup>
```

在上述範例中，兩個星字符號 ('**') 則是指定 _Images_ 資料夾中可以包含子資料夾。因此， `<MauiImage Include="Resources\Images\**\*" />` 指定 _Resources\Images_ 資料夾中的任何檔案或 _Images_ 資料夾中的任何子資料夾將設定為來源圖檔，替每個平台產生出所需的指定解析度的圖片檔案。

專屬指定平台的資源檔將覆蓋掉其共用資源的對應檔案。例如，如果您有一個放在 _Platforms\Android\Resources\drawable-xhdpi\logo.png_ 的 Android 平台中的特定解析規格中指定特定的圖片檔案，而同時也放置了一個位於 _Resources\Images\logo.svg_ 的共用圖片檔，則該 SVG 檔案將產生其他解析所需的 Android 圖片，但對於 XHDPI 解析度的該 logo 圖片則用已指定的特定解析的圖片檔案。

### 應用程式 Icon - App icons

可以直接將 Icon 的圖片檔案拖曳到 Visual Studio 的方案總管窗格當中，建在該單一專案底下的 _Resources\Images_ 資料夾，並到 **Properties** 窗格中將 Icon 圖片檔案的 Build Action 設定為 **MauiIcon** 來把應用程式的 Icon 加入到此應用程式的專案當中。這會在專案檔案 (.csproj) 當中增加對應該 Icon 圖片檔案應有的對應標記：

```xml
<MauiIcon Include="Resources\Images\appicon.png" />
```

應用程式的 Icon 圖片檔會在編譯建置應用的過程當中，自動調整為目標平台與設備所對應的解析度，然後也會將調整完成的應用程式 Icon 加入到應用程式編譯完成的封裝檔中。應用程式 Icon 會被調整成多種解析度的檔案，以便它們能在對應到設備在呈現 Icon 與應用程式發佈到市集展示 Icon 時的使用。

#### 應用程式圖片 - Images

可以直接將應用程式圖片檔案拖曳到 Visual Studio 的方案總管窗格當中，建在該單一專案底下的 _Resources\Images_ 資料夾，並到 **Properties** 窗格中將應用程式圖片檔案的 Build Action 設定為 **MauiImage**，來把應用程式圖片加入到應用程式的裝案當中。這會在專案檔案 (.csproj) 當中增加對應每個圖片檔案應有的對應標記：

```xml
<MauiImage Include="Resources\Images\logo.jpg" />
```

應用程式圖片檔會在編譯建置應用的過程當中，自動調整為目標平台與設備的對應的解析度，然後也會將調整大小完成的應用程式圖片檔加入到應用程式編譯完成的封裝檔中。

#### 應用程式字型 - Fonts

True type format (TTF) 和 open type font (OTF) 字型檔案可以增加到您的應用專案當中，一樣透過將該檔案拖曳到 Visual Studio 的方案總管窗格當中，建在該單一專案底下的 _Resources\Fonts_ 資料夾中，並到 **Properties** 窗格中將應用程式字型檔案的 Build Action 設定為 **MauiFont** 。這會在專案檔案 (.csproj) 當中增加對應每個字型檔案應有的對應標記：

```xml
<MauiFont Include="Resources\Fonts\OpenSans-Regular.ttf" />
```

應用程式字型檔會在編譯建置應用的過程當中，把字型檔複製到該應用程式封裝檔中。

#### 應用程式啟動畫面 - Splash screen

透過將應用程式啟動畫面的圖片檔拖曳到 Visual Studio 的方案總管窗格當中，建在該單一專案底下的 _Resources\Images_ 資料夾，並到 **Properties** 窗格中將應用程式圖片檔案的 Build Action 設定為 **MauiSplashScreen** ，就可以將應用程式啟動畫面增加到應用程式專案當中。這會在專案檔案 (.csproj) 當中增加應用程式啟動畫面應有的對應標記：

```xml
<MauiSplashScreen Include="Resources\Images\splashscreen.svg" />
```

應用程式啟動畫面的圖片檔案會在編譯建置應用的過程當中，自動調整為目標平台與設備對應的應用程式啟動畫面的大小。然後也會將調整大小完成的應用程式啟動畫面加入到應用程式的封裝檔中。

#### 應用程式原始資產檔 - Raw assets

應用程式原始資產檔 (Raw assets) 包括 HTML、JSON 和影片，可以透過將它們拖曳到專案當中的 _Resources_ 資料夾（或子資料夾，例如 _Resources\Assets_）並到 **Properties** 窗格中將其 Build Action 設置為 **MauiAsset**，就可以將應用程式原始資產檔增加到應用程式專案當中。這會在專案檔案 (.csproj) 當中增加應用程式原始資產檔應有的對應標記：

```xml
<MauiAsset Include="Resources\Assets\index.html" />
```

如果控制元件有屬性的值需要設定應用程式原始資產檔 (Raw assets) 時，即可透過如下作法來設定:

```xaml
<WebView Source="index.html" />
```

應用程式原始資產檔 (Raw assets) 會在編譯建置應用的過程當中，被複製到該應用程式的封裝檔中。

### 了解 .NET MAUI 應用程式的啟動作業

.NET MAUI 應用程式使用 .NET 泛型主機的模型進行啟動。這使應用程式能夠從單一程式位置來做初始化作業，並在此提供設定字型、服務和引用第三方類別庫的動作。

每個平台進入點都呼叫 `MauiProgram` 這個靜態類別中的 `CreateMauiApp` 靜態方法，該方法建立一個 `MauiApp` 的執行個體並且回傳後，並成為應用程式的執行進入點。

`MauiProgram` 類別最少必須提供一個應用程式才能執行：

```csharp
namespace MyMauiApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>();

        return builder.Build();
    }
}  
```

`App` 類別繼承自 `Application` 類別：

```csharp
namespace MyMauiApp;

public class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }
}
```

#### 註冊字型

字型可以增加到應用程式並透過檔案名稱或設置別名來引用。這是透過 `MauiAppBuilder` 的這個執行個體上來呼叫 `ConfigureFonts` 方法來完成的。然後，再透過符合 `IFontCollection` 實作的執行個體中，呼叫 `AddFont` 方法來增加所需使用的字型：

```csharp

namespace MyMauiApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        return builder.Build();
    }
}
```

在上面的例子中，`AddFont` 方法的第一個參數是該字型的檔案名稱，而第二個參數代表的是可選填的別名名稱，若有設置字型的別名名稱時，則可透過引用該別名來使用此字型。

應用程式中要使用任何自行定義的字型檔時，都必須把應有的對應標記撰寫至專案的專案檔 (.csproj) 中，可透過設定字型的檔案名稱或是使用星字符號 ('*') 來完成：

```xml
<ItemGroup>
   <MauiFont Include="Resources\Fonts\*" />
</ItemGroup>
```

> 注意：
> 若是直接透過 Visual Studio 中的方案總管 (Solution Explorer) 增加到專案的字型檔案，則將會自動設定上述的對應標記到專案的專案檔 (.csproj) 中。

然後可透過引用其名稱來使用其字型，且不需要檔案的 **副檔名** ：

```xaml
<!-- Use font name -->
<Label Text="Hello .NET MAUI"
       FontFamily="OpenSans-Regular" />
```

或是可透過引用其别名來使用該字型：

```xaml
<!-- Use font alias -->
<Label Text="Hello .NET MAUI"
       FontFamily="OpenSansRegular" />
```

到這邊的介紹就已經對 .NET MAUI 專案結構有了基本的了解了，就讓我們繼續來建立應用程式吧! 前往 [Hands-on Lab Part 1: 呈現出資料](../Part%201%20-%20Displaying%20Data/README.zh-tw.md)。

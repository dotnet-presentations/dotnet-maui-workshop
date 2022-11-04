## 实验准备 

让我们首先了解 .NET MAUI 的基本概念以及项目的相关结构。

### 在 Visual Studio 中打开解决方案

1. 打开 **Part 1 - Displaying Data/MonkeyFinder.sln**

此 MonkeyFinder 解决方案包含 1 个项目 (MonkeyFinder.csproj)：

* MonkeyFinder 项目 - 基于 .NET MAUI 针对 Android、iOS、macOS 和 Windows 的跨平台应用开发的主要项目。 它包括应用程序开发所需要的所有部分（ Models 、 Views 、ViewModels 和 Services ）。

![MonkeyFinder 解决方案的具体架构](../Art/Solution.PNG)

**MonkeyFinder 项目**也包括了我们将在动手实验期间使用的空白代码文件和 XAML 页面文件。 实验过程中，我们都通过该项目去添加和修改相关代码。

### 了解.NET MAUI 单项目(Single Project) 结构
 
.NET MAUI ( .NET .NET Multi-platform App UI )单项目为您在开发跨平台应用过程中提供针对不同平台的开发体验，并将它们抽象为一个可以针对 Android、iOS、macOS 和 Windows 的共享项目。

.NET MAUI 单项目提供了简化且一致的跨平台应用开发体验。 .NET MAUI 单项目提供以下功能：

- 针对 Android、iOS、macOS 和 Windows 的单共享项目。
- 用于运行 .NET MAUI 应用程序的简化调试目标选择。
- 在单项目中共享不同平台的相关资源文件。
- 可以访问不同平台特有的 API 和相关工具。
- 单个跨平台应用程序接入。

.NET MAUI 单项目允许使用多目标调试和使用 .NET 6 中的 SDK 风格的项目。

#### 资源文件

过去跨平台应用开发的资源管理一直存在问题。每个平台都有自己的资源管理方法，必须在每个平台上实施。例如，每个平台都有不同的图像要求，通常涉及以不同的分辨率创建每个图像的多个版本。因此，单个图像通常必须在每个平台以不同的分辨率复制多次，生成的图像必须在每个平台上使用不同的文件名和文件夹约定。

.NET MAUI 单项目使资源文件可以存储在统一位置上，可以让不同系统平台使用。这包括字体、图像、应用程序图标、屏幕启动和  Raw assets。

> 重点：
> 每个图像资源文件都用作源图像，在创建时为每个平台生成所需要图像的分辨率。

资源文件应该放置在 .NET MAUI 应用项目的 _Resources_ 文件夹或 _Resources_ 文件夹的子文件夹中，并且必须正确设置其 Build Action。下表显示了每种资源文件类型的 Build Action：

| 资源类型 | Build Action 设置 |
| -------- | ------------ |
| 应用程序图标(App icon) | MauiIcon |
| 应用程序字体(Fonts) | MauiFont |
| 应用程序图片(Images) | MauiImage |
| 应用程序启动画面(Splash screen) | MauiSplashScreen |
| 应用程序 Raw assets(Raw assets) | MauiAsset |

> 注意：
> XAML 文件也存储在您的 .NET MAUI 应用项目中，在项目和项模板创建时自动设置为 **MauiXaml** 的 Build Action 。 但是，XAML 文件通常不会位于应用项目的 _Resources_ 文件夹中。

将资源文件添加到 .NET MAUI 应用项目时，会在项目 (.csproj) 文件中创建资源的相应条目。 添加资源文件后，可以在 **Properties** 窗口中设置其Build Action。 以下截图显示了一个 _Resources_ 文件夹，其中包含子文件夹中的图像和字体资源：

![图片和字体资源的相关截屏.](../Art/ResourcesSingleProject.png)

_Resources_ 文件夹的子文件夹可以通过编辑应用程序的项目 (.csproj) 文件来为每种资源类型指定：

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

通配符 (`*`) 表示文件夹中的所有文件都将被视为指定资源类型。 此外，还可以包含子文件夹中的所有文件：

```xml
<ItemGroup>
    <!-- Images -->
    <MauiImage Include="Resources\Images\**\*" />
</ItemGroup>
```

在此示例中，双通配符 ('**') 指定 _Images_ 文件夹可以包含子文件夹。 因此，`<MauiImage Include="Resources\Images\**\*" />` 指定 _Resources\Images_ 文件夹中的任何文件或 _Images_ 文件夹的任何子文件夹将用作源图像， 为每个平台生成所需的分辨率。

特定于平台的资源将覆盖其共享资源对应项。 例如，如果您有一个位于 _Platforms\Android\Resources\drawable-xhdpi\logo.png_ 的 Android 特定图像，并且您还提供了一个共享的 _Resources\Images\logo.svg_ 图像，则 SVG 文件 将用于生成所需的 Android 图像，但 XHDPI 图像已作为特定于平台的图像存在。

### 应用程序图标 - App icons

可以通过将图像拖到项目的_Resources\Images_ 文件夹中，并在**Properties** 窗口中将图标的 Build Action 设置为**MauiIcon** 来将应用程序图标添加到您的应用程序项目中。 这会在您的项目文件中创建一个相应的条目：

```xml
<MauiIcon Include="Resources\Images\appicon.png" />
```

在构建时，应用程序图标的大小会调整为目标平台和设备的正确大小。 然后将调整大小的应用程序图标添加到您的应用程序包中。 应用图标被调整为多种分辨率，因为它们有多种用途，包括用于在设备上和应用商店中展示应用。

#### 应用程序图片 - Images

通过将图像拖到项目的_Resources\Images_ 文件夹中，并在**Properties** 窗口中将它们的 Build Action 设置为**MauiImage**，可以将图像添加到您的应用程序项目中。 这会在项目文件中为每个图像创建一个相应的条目：

```xml
<MauiImage Include="Resources\Images\logo.jpg" />
```

在构建时，图像被调整为目标平台和设备的正确分辨率。 然后将调整大小的图像添加到您的应用程序包中。

#### 应用程序字体 - Fonts

True type format (TTF) 和 open type font (OTF) 字体可以添加到您的应用项目中，方法是将它们拖到项目的_Resources\Fonts_ 文件夹中，并在 **Properties** 窗口中将它们的 Build Action 设置为**MauiFont** 。 这会在项目文件中为每个字体创建一个相应的条目：

```xml
<MauiFont Include="Resources\Fonts\OpenSans-Regular.ttf" />
```

在构建时，字体会复制到您的应用程序包中。

#### 应用程序启动画面 - Splash screen

通过将图像拖到项目的_Resources\Images_ 文件夹中，并在**Properties** 窗口中将图像的 Build Action 设置为**MauiSplashScreen**，可以将应用程序启动画面添加到您的应用程序项目中。 这会在您的项目文件中创建一个相应的条目：

```xml
<MauiSplashScreen Include="Resources\Images\splashscreen.svg" />
```

在构建时，初始屏幕图像的大小将调整为目标平台和设备的正确大小。 然后将调整大小的初始屏幕添加到您的应用程序包中。

#### 应用程序 - Raw assets

Raw assets 文件包括 HTML、JSON 和视频，可以通过将它们拖到项目的 _Resources_ 文件夹（或子文件夹，例如 _Resources\Assets_）并将其 Build Action 设置为 **Properties** 窗口中的`MauiAsset`。 这会在您的项目文件中为每个资产创建一个相应的条目：

```xml
<MauiAsset Include="Resources\Assets\index.html" />
```

然后控件就可以根据需要使用Raw assets:

```xaml
<WebView Source="index.html" />
```

在构建时，Raw assets 将被复制到您的应用程序包中。

### 了解 .NET MAUI 应用程序启动

.NET MAUI 应用程序使用 .NET 通用主机模型进行引导。 这使应用程序能够从单个位置初始化，并提供配置字体、服务和第三方库的能力。

每个平台入口点调用静态“MauiProgram”类的“CreateMauiApp”方法，该方法创建并返回一个“MauiApp”，即应用程序的入口点。

`MauiProgram` 类必须至少提供一个应用程序才能运行：

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

`App` 类派生自 `Application` 类：

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

#### 注册字体

字体可以添加到您的应用程序并通过文件名或别名引用。 这是通过在 `MauiAppBuilder` 对象上调用 `ConfigureFonts` 方法来完成的。 然后，在 `IFontCollection` 对象上，调用 `AddFont` 方法来添加所需的字体：

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

在上面的例子中，`AddFont` 方法的第一个参数是字体文件名，而第二个参数代表一个可选的别名，在使用字体时可以通过该别名引用它。

应用使用的任何自定义字体都必须包含在您的 .csproj 文件中。 这可以通过引用它们的文件名或使用通配符来完成：

```xml
<ItemGroup>
   <MauiFont Include="Resources\Fonts\*" />
</ItemGroup>
```

> 注意：
> 通过 Visual Studio 中的解决方案资源管理器(Solution Explorer)添加到项目的字体将自动包含在 .csproj 文件中。

然后可以通过引用其名称来使用该字体，而无需文件扩展名：

```xaml
<!-- Use font name -->
<Label Text="Hello .NET MAUI"
       FontFamily="OpenSans-Regular" />
```

或者，可以通过引用其别名来使用它：

```xaml
<!-- Use font alias -->
<Label Text="Hello .NET MAUI"
       FontFamily="OpenSansRegular" />
```

现在您已经对 .NET MAUI 项目结构有了基本的了解，让我们开始创建应用程序吧！ 前往 [实验一: 显示数据](../Part%201%20-%20Displaying%20Data/README.zh-cn.md)。

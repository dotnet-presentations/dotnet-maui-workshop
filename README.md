# .NET MAUI - 动手实验手册

今天我们将会构建一个 [.NET MAUI](https://docs.microsoft.com/dotnet/maui?WT.mc_id=friends-mauiworkshop-jamont) 的应用程序，它将显示来自世界各地的猴子列表。 我们将从构建业务逻辑后端开始，该后端从 RESTful 端点提取 json 编码的数据。 然后，我们将利用 [.NET MAUI](https://docs.microsoft.com/xamarin/essentials/index?WT.mc_id=friends-mauiworkshop-jamont) 找到离我们最近的猴子，并将猴子展示在地图上。 我们还将学习到如何用多种不同的方式显示数据，最后开发一个完整的主题化应用程序。

## 动手实验环境需知
本次实验将是一个需要动手和自备设备的实验。您可以在 PC（推荐）或 Mac 上进行开发，您需要做的就是安装带有 .NET MAUI workload 的 Visual Studio 2022 或 Visual Studio for Mac 2022。

在开始本实验系列之前，我建议先用10分钟快速阅读 [.NET MAUI 教程](https://docs.microsoft.com/dotnet/maui/get-started/first-app?WT.mc_id=friends-mauiworkshop-jamont)，它将指导您完成安装和正确配置好所需要的实验环境。

如果您是移动应用开发的新手，我们建议您把应用直接部署到 Android 设备(只需几个步骤即可完成有关设置)。如果您没有设备，请不用担心，因为您可以设置 [具有硬件加速功能的 Android 模拟器](https://docs.microsoft.com/xamarin/android/get-started/installation/android-emulator?WT.mc_id=friends-mauiworkshop-jamont)。如果您没有时间提前准备好相关设置，也不要担心，因为我们会在实验期间提供有关的提示和帮助。

## 实验内容

这是本次实验的有关内容：

* [实验准备](Part%200%20-%20Overview/README.zh-cn.md) - 介绍 .NET MAUI 基本知识以及环境安装 
* [实验一: 显示数据](Part%201%20-%20Displaying%20Data/README.zh-cn.md) - 实现单页数据列表
* [实验二: MVVM 和数据绑定](Part%202%20-%20MVVM/README.zh-cn.md) - MVVM 设计模式和数据绑定
* [实验三: 添加导航页面](Part%203%20-%20Navigation/README.zh-cn.md) - 为应用添加导航
* [实验四: 访问平台特性](Part%204%20-%20Platform%20Features/README.zh-cn.md) - 实现不同平台特定功能
* [实验五: 为 CollectionView 添加下拉刷新](Part%205%20-%20CollectionView/README.md) - CollectionView 使用技巧
* [实验六: 应用程序主题设置](Part%206%20-%20AppThemes/README.zh-cn.md) - 主题化应用


要开始使用，请打开“Part 1 - Displaying Data”文件夹并打开“MonkeyFinder.sln”。 您可以在整个实验过程中使用该项目。 每个**实验**都有一个 **README** 文件，其中包含该步骤实验过程的说明。 您也可以打开任何步骤的文件夹，里面都有一个和步骤对应的项目，您也可以打开来查看每一步的实现。

## 教学视频
James 已经录制了4个小时的完整有关本次实验的手把手教学视频，各位小伙伴请到 [James 的 YouTube 频道](https://youtube.com/jamesmontemagno) 观看

## 更多链接和资源：
- [.NET MAUI 官方网站](https://dot.net/maui)
- [Microsoft Learn 上的 .NET MAUI 的学习路径](https://aka.ms/Learn.MAUI)
- [.NET MAUI 官方文档](https://aka.ms/Docs.MAUI)
- [GitHub 上的 .NET MAUI 项目](https://github.com/dotnet/maui)
- [.NET 初学者入门系列视频](https://dot.net/videos)


如果您有任何问题，请在 Twitter 上与[@JamesMontemagno](https://twitter.com/jamesmontemagno) 联系。



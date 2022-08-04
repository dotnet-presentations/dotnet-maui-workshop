## Hands-on Lab Part 1: 顯示出資料

在此部分的 Hands-on Lab 要完成的，是要讓大家在對於 .NET MAUI 專案結構有初步了解後，開始進入撰寫程式的部分，並且看看如何在透過 CollectionView 的列表控制項中顯示出資料。

### 在 Visual Studio 中開啟解決方案

1. 開啟 **Part 1 - Displaying Data/MonkeyFinder.sln**

此 MonkeyFinder 包含 1 個專案：

* MonkeyFinder 專案 - 基於 .NET MAUI 針對 Android、iOS、macOS 和 Windows 的跨平台應用程式的主要專案。它包括應用程式開發所需要的所有部分（Models、Views、ViewModels 和 Services ）。

![MonkeyFinder 解決方案的專案結構](../Art/Solution.PNG)

**MonkeyFinder 專案** 也包括了在 Hands-on Lab 的過程當中將使用到的空白程式碼檔與 XAML 的頁面文件檔。在進行 Hands-on Lab 的時候，都將會透過專案當中的這些檔案直接修改有關的程式碼。

### 進行 NuGet 套件還原

Hands-on Lab 的專案已經都設定好所需引用的 Nuget 套件，因此在 Hands-on Lab 的過程中無須再安裝其他 Nuget 套件。唯一要做的一件事情就是在第一次使用此 Hands-on Lab 的專案時需要還原 Nuget 套件的引用(需透過網際網路連到 nuget.org 進行下載)。

1. 在 MonkeyFinder 的解決方案上的 **右鍵選單** 選項中選擇 **Restore NuGet packages** 。

![還原 NuGets](../Art/RestoreNuGets.PNG)

### 建立 Model 紀錄資料描述

此 Hands-on Lab 當中將下載有關猴子的資料，所以在應用程式當中需要設計一個類別來記錄它的資料描述。

![將 json 轉換成 c# 類別](../Art/Convert.PNG)

可以透過使用 [json2csharp.com](https://json2csharp.com) 來轉換位於 [montemagno.com/monkeys.json](https://montemagno.com/monkeys.json) 的原始 json 資料，並將此原始的 json 資料貼入左邊的輸入框後，也確定上方介面中轉換的選項有選成 **json to C#** 之後，即可點選 **Convert** 來進行轉換。  

而所產生出 C# 的類別，除了要確認此類別的名稱有被正確設定成 `Monkey` 之外，也要確認此類別的命名空間是設定 `MonkeyFinder.Model` 之後，再進行該類別檔案的下載。  

但如同前面所述，此 Hands-on Lab 當中所準備的專案中，都已經有先建立好所需的空白類別檔，所以只需要圈選 json2csharp 所協助轉換成的 C# 類別中 `Auto 屬性` 這個部分的程式碼後複製，即可直接到專案當中進行下面動作。

1. 開啟 `Model/Monkey.cs` 檔案。
2. 在 `Monkey.cs` 類別當中, 貼上剛剛在 json2csharp 服務當中所複製的程式碼。  
  
完成後如下結果:

```csharp
public class Monkey
{        
    public string Name { get; set; } 
    public string Location { get; set; } 
    public string Details { get; set; } 
    public string Image { get; set; } 
    public int Population { get; set; } 
    public double Latitude { get; set; } 
    public double Longitude { get; set; } 
}
```

### 顯示出資料

接著將可以在 `MainPage.xaml` 的 `CollectionView` 當中顯示出經過特殊定義而對應類型的任何資料。所以只要透過設定 `ItemTemplate` 就能設置基本的控制項(如: Image、Label...等)，來建立使用者介面呈現資料。

首先，找到在 `MainPage.xaml` 檔案一開始的 ContentPage 標記，並在該標記中多增加一個新的命名空間引用的撰寫：

```xml
xmlns:model="clr-namespace:MonkeyFinder.Model"
```

以便在後續的 XAML 撰寫可以使用到前一段所做好的 Monkey 類別，並且在也可以透過 Binding 來完成資料繫結。

接著，將下面的 XAML 內容加到 MainPage.xaml 的 `ContentPage` 成對標記當中：

```xml
<CollectionView>
    <CollectionView.ItemsSource>
        <x:Array Type="{x:Type model:Monkey}">
            <model:Monkey
                Name="Baboon"
                Image="https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/baboon.jpg"
                Location="Africa and Asia" />
            <model:Monkey
                Name="Capuchin Monkey"
                Image="https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/capuchin.jpg"
                Location="Central and South America" />
            <model:Monkey
                Name="Red-shanked douc"
                Image="https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/douc.jpg"
                Location="Vietnam" />
        </x:Array>
    </CollectionView.ItemsSource>
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Monkey">
            <HorizontalStackLayout Padding="10">
                <Image
                    Aspect="AspectFill"
                    HeightRequest="100"
                    Source="{Binding Image}"
                    WidthRequest="100" />
                <Label VerticalOptions="Center" TextColor="Gray">
                    <Label.Text>
                        <MultiBinding StringFormat="{}{0} | {1}">
                            <Binding Path="Name" />
                            <Binding Path="Location" />
                        </MultiBinding>
                    </Label.Text>
                </Label>
            </HorizontalStackLayout>
        </DataTemplate>
    </CollectionView.ItemTemplate>
</CollectionView>
```

如果想將兩個串文字垂直呈現，可以將兩個 `Label` 控制項包在由 `VerticalStackLayout` 組成的成對標記當中，並在兩個 `Label` 控制項當中，來透過設定字體大小 (FontSize) 的屬性來讓文字在顯示有差異化的效果：

```xml
 <HorizontalStackLayout Padding="10">
    <Image
        Aspect="AspectFill"
        HeightRequest="100"
        Source="{Binding Image}"
        WidthRequest="100" />
    <VerticalStackLayout VerticalOptions="Center">
        <Label Text="{Binding Name}" FontSize="24" TextColor="Gray"/>
        <Label Text="{Binding Location}" FontSize="18" TextColor="Gray"/>
    </VerticalStackLayout>
</HorizontalStackLayout>
```

### 測試運作應用程式

確認機器的環境都可以在不同平台與目標當中進行測試：

* [有關 Android 模擬器的設定步驟](https://docs.microsoft.com/dotnet/maui/android/emulator/device-manager)
* [在 Windows 中使用 .NET MAUI 開發的設定](https://docs.microsoft.com/dotnet/maui/windows/setup)

1. 在 Visual Studio 中，透過選擇偵錯選單當中的下拉選單來變更 `架構` (Framework)，將 Android 或 Windows 應用程式設置為啟動專案

![Visual Studio 偵錯選單當中顯示多個 架構 (Framework) ](../Art/SelectFramework.png)

2. 在 Visual Studio 中點選 `偵錯` 按鈕或工具 -> 開始偵錯
     - 如果有遇到任何問题，請參考執行時的設定指引

執行的應用程式後呈現出三隻猴子的資料在列表當中的結果：

![在 Android 上執行的應用程式並顯示三隻猴子的結果](../Art/CodedMonkeys.png)

接著，繼續到 [Hands-on Lab Part 2 : MVVM 和資料繫結](../Part%202%20-%20MVVM/README.zh-tw.md) 中，學習如何使用資料繫結與 MVVM 框架。

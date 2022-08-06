## Hands-on Lab Part 6: 設定 App 的佈景主題

到目前為止在應用程式所設定 App 的佈景主題都使採用預設的淺色主題。.NET MAUI 的設計是存在可重用資源 (Resources) 設計的觀念，以及透過自動適應設備或系統中，配合使用者正在使用的佈景主題，來設定 App 應該採用的對應佈景主題。

## 可重用的資源 (Resources)

開啟 `App.xaml` 檔案，在這裡面有幾個 `Color` 和 `Styles` 的設定。這些是提前為整個應用程式所套用的一些基本顏色與樣式所設定的。例如，主要背景顏色定義了一種淺色的色彩：

```xml
<Color x:Key="LightBackground">#FAF9F8</Color>
```

在這之後應用程式所設置的任何 UI 元件或要重用的共用樣式中都可以直接使用它。例如，透過 `ButtonOutline` 設置樣式後並套用到 `Button` 控制項上，而且在這個 `ButtonOutline` 設置的樣式還調整了圆角、文字、邊框和背景(使用前述的背景色彩資源)...等屬性設定顏色：


```xml
<Style x:Key="ButtonOutline" TargetType="Button">
    <Setter Property="Background" Value="{StaticResource LightBackground}" />
    <Setter Property="TextColor" Value="{StaticResource Primary}" />
    <Setter Property="BorderColor" Value="{StaticResource Primary}" />
    <Setter Property="BorderWidth" Value="2" />
    <Setter Property="HeightRequest" Value="40" />
    <Setter Property="CornerRadius" Value="20" />
</Style>
```

建立 `Color` 和 `Styles` 這些設定可以被稱為可重用的資源 (Resources)，是一種要針對整個應用程式來共享並套用元件格式的好方式。

## 佈景主題變更 - 淺色/深色模式

若想要讓應用程式根據使用者當前設備的佈景主題設置自動變更的話，例如深色模式時應用程式該有如何的應對效果？這就透過 .NET MAUI 設計能用於 `AppThemeBinding` 的概念來完成吧。讓在這邊使用 `Label` 的 `TextColor` 屬性設定來解釋，透過定義兩種新顏色來選用：

```xml
<Color x:Key="LabelText">Black</Color>
<Color x:Key="LabelTextDark">White</Color>
```

由於文字內容在背景顏色為淺色模式時是預設為黑色，而在背景顏色為深色模式時要變成白色。但通常，為了簡便就會將文字顏色直接設定為固定的顏色，例如：

```xml
<Label Text="Hello, world!" TextColor="{StaticResource LabelText}"/>
```

但這並不會自動對佈景主題的變化而改變顏色。在這邊可以將其設定為 `DynamicResource` 用以檢查佈景主題的變化時能更新 `LabelText` 的設定值，又或者是可以使用 `AppThemeBinding` 的設置：

```xml
<Label Text="Hello, world!" 
       TextColor="{AppThemeBinding Light={StaticResource LabelText}, Dark={StaticResource LabelTextDark}}"/>
```

現在可以選擇建立一個透過名稱 (x:Key) 來使用可重用資源 (Resources) 並針對指定的 UI 控制項設置其專屬的樣式：

```xml
<Style TargetType="Label" x:Key="DefaultLabel">
    <Setter Property="TextColor" Value="{AppThemeBinding Light={StaticResource LabelText}, Dark={StaticResource LabelTextDark}}" />
</Style>
```

```xml
<Label Text="Hello, world!" 
       Style="{StaticResource DefaultLabel}"/>
```

如果在設定 `Style` 的時候省略 `x:Key` 的名稱設定，那這將會直接套用到整個應用程式當中的每個 `Label` 控制項。

```xml
<Style TargetType="Label">
    <Setter Property="TextColor" Value="{AppThemeBinding Light={StaticResource LabelText}, Dark={StaticResource LabelTextDark}}" />
</Style>
```

## 更新可重用的資源 (Resources)

接著在整個應用程式中設定支援佈景主題的淺色/深色模式。

1. 在 `ResourceDictionary` 當中增加一些要使用的一些新顏色以配合佈景主題的淺色/深色模式：

    ```xml
    <Color x:Key="CardBackground">White</Color>
    <Color x:Key="CardBackgroundDark">#1C1C1E</Color>

    <Color x:Key="LabelText">#1F1F1F</Color>
    <Color x:Key="LabelTextDark">White</Color>
    ```

2. 接著從以下幾個地方更新頁面的背景顏色：

    原本的：

    ```xml
    <Style ApplyToDerivedTypes="True" TargetType="Page">
        <Setter Property="BackgroundColor" Value="{StaticResource LightBackground}" />
    </Style>
    ```

    改變成:

    ```xml
    <Style ApplyToDerivedTypes="True" TargetType="Page">
        <Setter Property="BackgroundColor" Value="{AppThemeBinding Light={StaticResource LightBackground}, Dark={StaticResource DarkBackground}}" />
    </Style>
    ```

3. 變更 `BaseLabel` 的 `TextColor` 設定值：

    ```xml
    <Setter Property="TextColor" Value="{AppThemeBinding Light={StaticResource LabelText}, Dark={StaticResource LabelTextDark}}" />
    ```

4. 幫先前的 Hands-on Lab Part 5 中所設計的 `RefreshView` 控制項來增加 `Background` 的屬性值。

    ```xml
    <Style ApplyToDerivedTypes="True" TargetType="RefreshView">
        <Setter Property="RefreshColor" Value="{StaticResource Primary}" />
        <!--Add this-->
        <Setter Property="Background" Value="{AppThemeBinding Light={StaticResource LightBackground}, Dark={StaticResource DarkBackground}}" />
    </Style>
    ```

5. 繼續調整 `ButtonOutline` 上的 `Background` 設定值。

    ```xml
    <Setter Property="Background" Value="{AppThemeBinding Light={StaticResource LightBackground}, Dark={StaticResource DarkBackground}}" />
    ```

6. 調整 `CardView` 上的 `Background` 設定值。

    ```xml
    <Setter Property="Background" Value="{AppThemeBinding Light={StaticResource CardBackground}, Dark={StaticResource CardBackgroundDark}}" />
    ```

現在執行應用程式，並透過設定變更佈景主題的淺色/深色模式：

![變更佈景主題的設定](../Art/Themes.gif)


恭喜！  

到這邊，已經完整地建立了第一個 .NET MAUI 應用程式。這當中包含了從網際網路下載資料後，呈現到應用程式的使用者介面中後，更是完成了巡覽轉跳頁面的功能，也增加了針對平台的特定功能配置，最後還針對應用程式設定適應設備的佈景主題！

再進一步到 [官方的 .NET MAUI 教學文件](https://docs.microsoft.com/dotnet/maui/get-started/first-app?WT.mc_id=EM-MVP-5001645) 了解更多吧~~~

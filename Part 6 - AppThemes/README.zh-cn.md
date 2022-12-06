## 实验六: 应用程序主题设置

到目前为止，我们已经在应用程序上使用了标准的浅色主题。 .NET MAUI 具有可重用应用程序资源的概念，以及可以自动适应设备主题的资源。

## 可重用资源

打开 `App.xaml` 文件，注意有几个 `Color` 条目和 `Styles`。 这些是为我们在整个应用程序中使用的一些基本颜色和样式提前配置的。 例如，我们为主背景色定义了一种浅色：

```xml
<Color x:Key="LightBackground">#FAF9F8</Color>
```

以后任何 UI 元素或可重用的共享样式都可以引用它。 例如，我们的 `ButtonOutline` 样式应用于 `Button` 控件并为其赋予圆角，为文本、边框和背景设置颜色：


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
这是在整个应用程序中共享代码的好方法。

## 主题更改 - 浅色/深色主题

当您想要响应用户将设备更改为使用黑色主题模式时会发生什么？ 好吧，.NET MAUI 具有用于值的“AppThemeBinding”的概念。 让我们使用 `Label` 的 `TextColor` 属性。 我们可以定义两种新的颜色来使用：

```xml
<Color x:Key="LabelText">Black</Color>
<Color x:Key="LabelTextDark">White</Color>
```

我们希望文本在背景颜色较浅时为黑色，而在背景颜色较深时为白色。 通常，我们会将颜色设置为单一颜色，例如：

```xml
<Label Text="Hello, world!" TextColor="{StaticResource LabelText}"/>
```

但是，这不会适应应用程序主题的变化。 我们可以将其设为 `DynamicResource`，监听应用主题的变化，并更新 `LabelText` 的值，或者我们可以使用 `AppThemeBinding`：

```xml
<Label Text="Hello, world!" 
       TextColor="{AppThemeBinding Light={StaticResource LabelText}, Dark={StaticResource LabelTextDark}}"/>
```

我们现在可以选择创建一个我们通过名称引用的可重用样式或适用于特定类型的每个元素的样式：

```xml
<Style TargetType="Label" x:Key="DefaultLabel">
    <Setter Property="TextColor" Value="{AppThemeBinding Light={StaticResource LabelText}, Dark={StaticResource LabelTextDark}}" />
</Style>
```

```xml
<Label Text="Hello, world!" 
       Style="{StaticResource DefaultLabel}"/>
```

如果我们省略 `x:Key`，那么它将自动应用于我们应用程序中的每个 `Label`。

```xml
<Style TargetType="Label">
    <Setter Property="TextColor" Value="{AppThemeBinding Light={StaticResource LabelText}, Dark={StaticResource LabelTextDark}}" />
</Style>
```

## 更新资源

现在，让我们在整个应用程序中添加浅色/深色主题支持。

1. 让我们在 `ResourceDictionary` 中添加一些我们将使用的新颜色：

    ```xml
    <Color x:Key="CardBackground">White</Color>
    <Color x:Key="CardBackgroundDark">#1C1C1E</Color>

    <Color x:Key="LabelText">#1F1F1F</Color>
    <Color x:Key="LabelTextDark">White</Color>
    ```

2. 让我们从以下位置更新页面的背景颜色：

    从：

    ```xml
    <Style ApplyToDerivedTypes="True" TargetType="Page">
        <Setter Property="BackgroundColor" Value="{StaticResource LightBackground}" />
    </Style>
    ```

    转变为:

    ```xml
    <Style ApplyToDerivedTypes="True" TargetType="Page">
        <Setter Property="BackgroundColor" Value="{AppThemeBinding Light={StaticResource LightBackground}, Dark={StaticResource DarkBackground}}" />
    </Style>
    ```

3. 更新 `BaseLabel` 的 `TextColor` 值：

    ```xml
    <Setter Property="TextColor" Value="{AppThemeBinding Light={StaticResource LabelText}, Dark={StaticResource LabelTextDark}}" />
    ```

4. 在我们的 `RefreshView` 上添加`Background`

    ```xml
    <Style ApplyToDerivedTypes="True" TargetType="RefreshView">
        <Setter Property="RefreshColor" Value="{StaticResource Primary}" />
        <!--Add this-->
        <Setter Property="Background" Value="{AppThemeBinding Light={StaticResource LightBackground}, Dark={StaticResource DarkBackground}}" />
    </Style>
    ```

5. 更新 `ButtonOutline` 上的 `Background`

    ```xml
    <Setter Property="Background" Value="{AppThemeBinding Light={StaticResource LightBackground}, Dark={StaticResource DarkBackground}}" />
    ```

6. 更新 `CardView` 上的 `Background`

    ```xml
    <Setter Property="Background" Value="{AppThemeBinding Light={StaticResource CardBackground}, Dark={StaticResource CardBackgroundDark}}" />
    ```

现在，让我们运行应用程序并更改主题：

![改变主题](../Art/Themes.gif)


恭喜！ 您构建了属于您的第一个 .NET MAUI 应用程序、从互联网加载数据、实现了导航、添加了平台特定功能并为应用程序设置了主题！


       




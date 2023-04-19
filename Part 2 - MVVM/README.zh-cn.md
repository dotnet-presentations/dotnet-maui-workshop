## 实验二: MVVM 和数据绑定

在实验二中，我们将介绍与 MVVM 的完整数据绑定，并从互联网数据源中检索猴子。

### INotifyPropertyChanged 实现

*INotifyPropertyChanged* 对于 MVVM 框架中的数据绑定很重要。 这是一个接口，当实现时，让我们的视图知道模型的变化。 我们将在 `BaseViewModel` 中实现一次，以便我们创建的所有其他视图模型都可以继承它。

1. 在 Visual Studio 中，打开 `ViewModel/BaseViewModel.cs`
   
2. 在 `BaseViewModel.cs` 中，通过改变这个来实现 INotifyPropertyChanged

```csharp
public class BaseViewModel
{

}
```

转换为

```csharp
public class BaseViewModel : INotifyPropertyChanged
{

}
```

3. 在`BaseViewModel.cs`中，右键单击`INotifyPropertyChanged`
   
4. 实现`INotifyPropertyChanged`接口
    - (Visual Studio for Mac) 在右键菜单中，选择 Quick Fix -> Implement Interface
    - (Visual Studio PC) 在右键菜单中，选择 Quick Actions and Refactorings -> Implement Interface
  
5. 在 `BaseViewModel.cs` 中，确保现在出现这行代码：

```csharp
public event PropertyChangedEventHandler PropertyChanged;
```

6. 在 `BaseViewModel.cs` 中，创建一个名为 `OnPropertyChanged` 的新方法
     - 注意：每当属性更新时，我们都会调用 `OnPropertyChanged`

```csharp
public void OnPropertyChanged([CallerMemberName] string name = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
```
### 实现 Title、IsBusy 和 IsNotBusy

我们将为一些属性创建一个支持字段和访问器。 这些属性将允许我们在页面上设置标题，并让我们的视图知道我们的视图模型处于 busy 状态，因此我们不会执行重复操作（例如允许用户多次刷新数据）。 它们在 `BaseViewModel` 中，因为它们对于每个页面都是通用的。

1. 在 `BaseViewModel.cs` 中，创建支持字段：
   
```csharp
public class BaseViewModel : INotifyPropertyChanged
{
    bool isBusy;
    string title;
    //...
}
```
2. 创建相关属性:

```csharp
public class BaseViewModel : INotifyPropertyChanged
{
    //...
    public bool IsBusy
    {
        get => isBusy;
        set
        {
            if (isBusy == value)
                return;
            isBusy = value;
            OnPropertyChanged();
        }
    }

    public string Title
    {
        get => title;
        set
        {
            if (title == value)
                return;
            title = value;
            OnPropertyChanged();
        }
    }
    //...
}
```

请注意，当值更改时，我们会调用 `OnPropertyChanged`。 .NET MAUI 绑定基础结构将订阅我们的 **PropertyChanged** 事件，因此 UI 将收到更改通知。

我们还可以通过创建另一个名为 `IsNotBusy` 的属性来创建 `IsBusy` 的反转，该属性返回 `IsBusy` 的相反值，然后在设置 `IsBusy` 时触发 `OnPropertyChanged` 事件

```csharp
public class BaseViewModel : INotifyPropertyChanged
{
    //...
    public bool IsBusy
    {
        get => isBusy;
        set
        {
            if (isBusy == value)
                return;
            isBusy = value;
            OnPropertyChanged();
            // Also raise the IsNotBusy property changed
            OnPropertyChanged(nameof(IsNotBusy));
        }
    } 

    public bool IsNotBusy => !IsBusy;
    //...
}
```

### 使用 .NET Community Toolkit 简化 MVVM 模式

现在您已经了解了 MVVM 的工作原理，让我们看看一种简化开发的方法。 随着应用程序变得越来越复杂，将添加更多属性和事件。 这会导致添加更多样板代码。 .NET Community Toolkit 旨在通过源生成器来简化 MVVM，以自动处理我们过去必须手动编写的代码。 `CommunityToolkit.Mvvm` 库已添加到项目中，我们可以即可使用它。

删除 `BaseViewModel.cs` 中的所有代码并添加如下代码:

```csharp
namespace MonkeyFinder.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    string title;

    public bool IsNotBusy => !IsBusy;
}
```

在这里，我们可以看到我们的代码已经大大简化成了一个 `ObservableObject` 基类，它实现了 `INotifyPropertyChanged` 以及绑定相关属性。

请注意，isBusy 和 title 都附加了 `[ObservableProperty]` 属性。 生成的代码看起来几乎与我们手动编写的相同。 另外，isBusy 属性有 `[NotifyPropertyChangedFor(nameof(IsNotBusy))]`，当值改变时也会通知 `IsNotBusy`。 要查看生成的代码，请转到项目，然后展开 **Dependencies -> net7.0-android -> Analyzers -> CommunityToolkit.Mvvm.SourceGenerators -> CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator** 并打开`MonkeyFinder.ViewModel。 BaseViewModel.cs`：

这是我们的 `IsBusy` 属性:

```csharp
[global::System.CodeDom.Compiler.GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator", "8.0.0.0")]
[global::System.Diagnostics.DebuggerNonUserCode]
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public bool IsBusy
{
    get => isBusy;
    set
    {
        if (!global::System.Collections.Generic.EqualityComparer<bool>.Default.Equals(isBusy, value))
        {
            OnPropertyChanging(global::CommunityToolkit.Mvvm.ComponentModel.__Internals.__KnownINotifyPropertyChangingArgs.IsBusy);
            isBusy = value;
            OnPropertyChanged(global::CommunityToolkit.Mvvm.ComponentModel.__Internals.__KnownINotifyPropertyChangedArgs.IsBusy);
            OnPropertyChanged(global::CommunityToolkit.Mvvm.ComponentModel.__Internals.__KnownINotifyPropertyChangedArgs.IsNotBusy);
        }
    }
}
```

这段代码可能看起来有点吓人，但由于它是自动生成的，它添加了额外的属性以避免冲突。 它还通过缓存进行了高度优化。

将来，相同的库还将帮助我们处理点击事件，即“Commands”。

> 请注意，我们将此类更改为“partial”类，以便生成的代码可以在该类中共享。

### 创建 Monkey Service

们已经准备好创建一个从互联网上检索猴子数据的方法。 我们将首先使用 HttpClient 通过一个简单的 HTTP 请求来实现这一点。 我们将在位于 `Services` 文件夹中的 `MonkeyService.cs` 文件中执行此操作。

1. 在 `MonkeyService.cs` 中，让我们添加一个新方法来获取所有猴子：

    ```csharp
    List<Monkey> monkeyList = new ();
    public async Task<List<Monkey>> GetMonkeys()
    {
        return monkeyList;
    }
    ```

    现在，该方法只是创建一个新的 Monkeys 列表并返回它。 我们现在可以填写方法使用 `HttpClient` 来拉取一个 json 文件(包括解析/缓存/返回)。

2. 让我们通过添加到`MonkeyService`的构造函数中来访问`HttpClient`。

    ```csharp
    var response = await httpClient.GetAsync("https://www.montemagno.com/monkeys.json");

    if (response.IsSuccessStatusCode)
    {
        monkeyList = await response.Content.ReadFromJsonAsync(MonkeyContext.Default.ListMonkey);
    }
    
    return monkeyList;
    ```

    .NET MAUI 包括类似于 ASP.NET Core 的依赖注入。 我们将很快注册此服务和依赖项。

3. 让我们检查一下列表中是否有猴子，如果有，则通过填写 `GetMonkeys` 方法返回：
   
    ```csharp
    if (monkeyList?.Count > 0)
        return monkeyList;
    ```

4. 我们可以使用 `HttpClient` 发出 Web 请求，并使用内置的 `System.Text.Json` 反序列化对其进行解析。
   
    ```csharp
    var response = await httpClient.GetAsync("https://www.montemagno.com/monkeys.json");

    if (response.IsSuccessStatusCode)
    {
        monkeyList = await response.Content.ReadFromJsonAsync(MonkeyContext.Default.ListMonkey);
    }
    
    return monkeyList;
    ```

5. 别忘记在文件顶部添加以下 using 指令以访问 `ReadFromJsonAsync` 扩展方法：
   
    ```csharp
    using System.Net.Http.Json;
    ```

#### 如果没有网络

如果您在当前设置中遇到互联网访问问题，请不要担心，因为我们已将猴子列表嵌入到项目中。 您可以读取文件并返回它，而不是使用 `HttpClient`：

```csharp
using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
using var reader = new StreamReader(stream);
var contents = await reader.ReadToEndAsync();
monkeyList = JsonSerializer.Deserialize(contents, MonkeyContext.Default.ListMonkey);
```

### 从 ViewModel 调用 MonkeyService

我们现在可以更新我们的`MonkeysViewModel`来调用我们的新猴子服务并将猴子列表暴露给我们的用户界面。

我们将使用 `ObservableCollection<Monkey>` 将被清除然后加载 **Monkey** 对象。 我们使用 `ObservableCollection` 是因为它内置支持在我们从集合中添加或删除项目时引发 `CollectionChanged` 事件。 这意味着我们在更新集合时不会调用 `OnPropertyChanged`。

1. 在`MonkeysViewModel.cs`中声明一个我们将初始化为空集合的属性。 此外，我们可以将 Title 设置为“Monkey Finder”。


    ```csharp
    public partial class MonkeysViewModel : BaseViewModel
    {
        public ObservableCollection<Monkey> Monkeys { get; } = new();
        public MonkeysViewModel()
        {
            Title = "Monkey Finder";
        }
    }
    ```

2. 我们将要访问我们新的`MonkeyService`。 因此，让我们将以下 using 指令添加到文件顶部：
   
    ```csharp
    using MonkeyFinder.Services;
    ```

3. 我们还需要访问我们的`MonkeyService`，我们将通过构造函数注入它：
   
    ```csharp
    public ObservableCollection<Monkey> Monkeys { get; } = new();
    MonkeyService monkeyService;
    public MonkeysViewModel(MonkeyService monkeyService)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
    }
    ```

4. 在 `MonkeysViewModel.cs` 中，创建一个名为 `GetMonkeysAsync` 的方法，该方法返回 `async Task`：
   
    ```csharp
    public class MonkeysViewModel : BaseViewModel
    {
        //...
        async Task GetMonkeysAsync()
        {
        }
        //...
    }
    ```

5. 在 `GetMonkeysAsync` 中，首先确保 `IsBusy` 为 false。 如果是真的，`return`
   
    ```csharp
    async Task GetMonkeysAsync()
    {
        if (IsBusy)
            return;
    }
    ```

6. 在 `GetMonkeysAsync` 中，为 try/catch/finally 块添加内容
     - 请注意，当我们开始调用服务器和完成时，我们将 *IsBusy* 切换为 true，然后切换为 false。
  

    ```csharp
    async Task GetMonkeysAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;

        }
        catch (Exception ex)
        {

        }
        finally
        {
           IsBusy = false;
        }

    }
    ```

7. 在 `GetMonkeysAsync` 的 `try` 块中，我们可以从 `MonkeyService` 中获取猴子。
   
    ```csharp
    async Task GetMonkeysAsync()
    {
        //...
        try
        {
            IsBusy = true;

            var monkeys = await monkeyService.GetMonkeys();
        }
        //... 
    }
    ```

8. 仍然在 `try` 块内，清除 `Monkeys` 属性，然后添加新的猴子数据：

    ```csharp
    async Task GetMonkeysAsync()
    {
        //...
        try
        {
            IsBusy = true;

            var monkeys = await monkeyService.GetMonkeys();

            if(Monkeys.Count != 0)
                Monkeys.Clear();

            foreach (var monkey in monkeys)
                Monkeys.Add(monkey);
        }
        //...
    }
    ```

9. 在 `GetMonkeysAsync` 中，将此代码添加到 `catch` 块以在数据检索失败时显示弹出窗口：

    ```csharp
    async Task GetMonkeysAsync()
    {
        //...
        catch(Exception ex)
        {
            Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        //...
    }
    ```

10. 确保完成的代码如下所示：

    ```csharp
    async Task GetMonkeysAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var monkeys = await monkeyService.GetMonkeys();

            if(Monkeys.Count != 0)
                Monkeys.Clear();

            foreach(var monkey in monkeys)
                Monkeys.Add(monkey);

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }

    }
    ```

12. 最后，让我们通过可以数据绑定到的 `ICommand` 公开这个方法。 通常，我们必须创建一个支持字段，例如：

    ```csharp
    public Command GetMonkeysCommand { get; }
    public MonkeysViewModel()
    {
        //...
        GetMonkeysCommand = new Command(async () => await GetMonkeysAsync());
    }
    ```

    但是，使用 .NET Community Toolkit，我们可以简单地将 `[RelayCommand]` 属性添加到我们的方法中：

    ```csharp
     [RelayCommand]
    async Task GetMonkeysAsync()
    {
        //..
    }
    ```

    这将自动创建我们需要的所有代码：

    ```csharp
	// <auto-generated/>
	#pragma warning disable
	#nullable enable
	namespace MonkeyFinder.ViewModel
	{
		partial class MonkeysViewModel
		{
			/// <summary>The backing field for <see cref="GetMonkeysASyncCommand"/>.</summary>
			[global::System.CodeDom.Compiler.GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.0.0.0")]
			private global::CommunityToolkit.Mvvm.Input.AsyncRelayCommand? getMonkeysASyncCommand;
			/// <summary>Gets an <see cref="global::CommunityToolkit.Mvvm.Input.IAsyncRelayCommand"/> instance wrapping <see cref="GetMonkeysASync"/>.</summary>
			[global::System.CodeDom.Compiler.GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.RelayCommandGenerator", "8.0.0.0")]
			[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
			public global::CommunityToolkit.Mvvm.Input.IAsyncRelayCommand GetMonkeysASyncCommand => getMonkeysASyncCommand ??= new global::CommunityToolkit.Mvvm.Input.AsyncRelayCommand(new global::System.Func<global::System.Threading.Tasks.Task>(GetMonkeysASync));
		}
	}
    ```

我们获取数据的方法终于完成了!

### 注册 Services

在我们可以运行应用程序之前，我们必须注册我们所有的依赖项。 打开“MauiProgram.cs”文件。

1. 添加以下 using 指令来访问我们的 `MonkeyService`：

	```csharp
	using MonkeyFinder.Services;
	```

2. 找到我们在 `builder.Services` 中注册您的 `Main Page` 的位置，并在其上方添加以下内容：
   
   	```csharp
	builder.Services.AddSingleton<MonkeyService>();
	builder.Services.AddSingleton<MonkeysViewModel>();
	```

我们将 `MonkeyService` 和 `MonkeysViewModel` 注册为单例。 这意味着它们只会被创建一次，如果我们希望每个请求都创建一个唯一的实例，我们会将它们注册为“瞬态”。

3. 在项目后面的代码中，我们将把我们的`MonkeysViewModel`注入到我们的 MainPage 中：

    ```csharp
    public MainPage(MonkeysViewModel viewModel)
    {
	InitializeComponent();
	BindingContext = viewModel;
    }
    ```


## 创建用户界面 

现在是时候在 `View/MainPage.xaml` 中构建 .NET MAUI 用户界面了。 我们的最终结果是构建一个如下所示的页面：

![](../Art/FinalUI.PNG)

1. 在 `MainPage.xaml` 中，在 `ContentPage` 标记的顶部添加一个 `x:DataType`，这将使我们能够获得绑定智能感知：

    ```xml
    <ContentPage
        xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" 
        x:Class="MonkeyFinder.View.MainPage"
        xmlns:model="clr-namespace:MonkeyFinder.Model"
        xmlns:viewmodel="clr-namespace:MonkeyFinder.ViewModel"
        x:DataType="viewmodel:MonkeysViewModel">

    </ContentPage>
    ```

    这称为编译绑定。 我们指定我们将直接绑定到`MonkeysViewModel`。 这将进行错误检查并增强性能。

2. 我们可以通过添加 `Title` 属性在 `ContentPage` 上创建我们的第一个绑定：

```xml
<ContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="MonkeyFinder.View.MainPage"
    xmlns:model="clr-namespace:MonkeyFinder.Model"
    xmlns:viewmodel="clr-namespace:MonkeyFinder.ViewModel"
    x:DataType="viewmodel:MonkeysViewModel"
    Title="{Binding Title}">

</ContentPage>
```

3. 在 `MainPage.xaml` 中，我们可以在 `ContentPage` 标记之间添加一个 2 行 2 列的 `Grid`。 我们还将设置 `RowSpacing` 和 `ColumnSpacing` 

```xml
<ContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="MonkeyFinder.View.MainPage"
    xmlns:model="clr-namespace:MonkeyFinder.Model"
    xmlns:viewmodel="clr-namespace:MonkeyFinder.ViewModel"
    x:DataType="viewmodel:MonkeysViewModel"
    Title="{Binding Title}">

    <!-- Add this -->
    <Grid
        ColumnDefinitions="*,*"
        ColumnSpacing="5"
        RowDefinitions="*,Auto"
        RowSpacing="0">
    </Grid>
</ContentPage>
```

4. 在 `MainPage.xaml` 中，我们可以在 `Grid` 标记之间添加一个 `CollectionView`，跨越 2 列。 我们还将设置 `ItemsSource`，它将绑定到我们的 `Monkeys` ObservableCollection，并另外设置一些属性来优化列表。

```xml
<ContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="MonkeyFinder.View.MainPage"
    xmlns:model="clr-namespace:MonkeyFinder.Model"
    xmlns:viewmodel="clr-namespace:MonkeyFinder.ViewModel"
    x:DataType="viewmodel:MonkeysViewModel"
    Title="{Binding Title}">

    <!-- Add this -->
    <Grid
        ColumnDefinitions="*,*"
        ColumnSpacing="5"
        RowDefinitions="*,Auto"
        RowSpacing="0">
         <CollectionView ItemsSource="{Binding Monkeys}"
                         SelectionMode="None"
                         Grid.ColumnSpan="2">

        </CollectionView>
    </Grid>
</ContentPage>
```

5. 在 `MainPage.xaml` 中，我们可以向 `CollectionView` 添加一个 `ItemTemplate` 来表示列表中每个项目显示的内容：
   
```xml
<ContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="MonkeyFinder.View.MainPage"
    xmlns:model="clr-namespace:MonkeyFinder.Model"
    xmlns:viewmodel="clr-namespace:MonkeyFinder.ViewModel"
    x:DataType="viewmodel:MonkeysViewModel"
    Title="{Binding Title}">

   <Grid
        ColumnDefinitions="*,*"
        ColumnSpacing="5"
        RowDefinitions="*,Auto"
        RowSpacing="0">
        <CollectionView ItemsSource="{Binding Monkeys}"
                         SelectionMode="None"
                         Grid.ColumnSpan="2">
            <CollectionView.ItemTemplate>
                <DataTemplate x:DataType="model:Monkey">
                    <Grid Padding="10">
                        <Frame HeightRequest="125" Style="{StaticResource CardView}">
                            <Grid Padding="0" ColumnDefinitions="125,*">
                                <Image Aspect="AspectFill" Source="{Binding Image}"
                                       WidthRequest="125"
                                       HeightRequest="125"/>
                                <VerticalStackLayout
                                    Grid.Column="1"
                                    VerticalOptions="Center"
                                    Padding="10">
                                    <Label Style="{StaticResource LargeLabel}" Text="{Binding Name}" />
                                    <Label Style="{StaticResource MediumLabel}" Text="{Binding Location}" />
                                </VerticalStackLayout>
                            </Grid>
                        </Frame>
                    </Grid>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>
    </Grid>
</ContentPage>
```

6. 在 `MainPage.xaml` 中，我们可以在 `CollectionView` 下添加一个 `Button`，这将使我们能够单击它并从服务器获取猴子：

```xml
<ContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="MonkeyFinder.View.MainPage"
    xmlns:model="clr-namespace:MonkeyFinder.Model"
    xmlns:viewmodel="clr-namespace:MonkeyFinder.ViewModel"
    x:DataType="viewmodel:MonkeysViewModel"
    Title="{Binding Title}">

   <Grid
        ColumnDefinitions="*,*"
        ColumnSpacing="5"
        RowDefinitions="*,Auto"
        RowSpacing="0">
        <CollectionView ItemsSource="{Binding Monkeys}"
                         SelectionMode="None"
                         Grid.ColumnSpan="2">
            <CollectionView.ItemTemplate>
                <DataTemplate x:DataType="model:Monkey">
                    <Grid Padding="10">
                        <Frame HeightRequest="125" Style="{StaticResource CardView}">
                            <Grid Padding="0" ColumnDefinitions="125,*">
                                <Image Aspect="AspectFill" Source="{Binding Image}"
                                       WidthRequest="125"
                                       HeightRequest="125"/>
                                <VerticalStackLayout
                                    Grid.Column="1"
                                    VerticalOptions="Center"
                                    Padding="10">
                                    <Label Style="{StaticResource LargeLabel}" Text="{Binding Name}" />
                                    <Label Style="{StaticResource MediumLabel}" Text="{Binding Location}" />
                                </VerticalStackLayout>
                            </Grid>
                        </Frame>
                    </Grid>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>

        <!-- Add this -->
        <Button Text="Get Monkeys" 
                Command="{Binding GetMonkeysCommand}"
                IsEnabled="{Binding IsNotBusy}"
                Grid.Row="1"
                Grid.Column="0"
                Style="{StaticResource ButtonOutline}"
                Margin="8"/>
    </Grid>
</ContentPage>
```

7. 最后，在 `MainPage.xaml` 中，我们可以在最底部的所有控件或 `Grid` 上方添加一个 `ActivityIndicator`，当我们按下 `Get Monkeys` 按钮时，它将显示正在发生的事情的指示。



```xml
<ContentPage
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="MonkeyFinder.View.MainPage"
    xmlns:model="clr-namespace:MonkeyFinder.Model"
    xmlns:viewmodel="clr-namespace:MonkeyFinder.ViewModel"
    x:DataType="viewmodel:MonkeysViewModel"
    Title="{Binding Title}">

   <Grid
        ColumnDefinitions="*,*"
        ColumnSpacing="5"
        RowDefinitions="*,Auto"
        RowSpacing="0">
        <CollectionView ItemsSource="{Binding Monkeys}"
                         SelectionMode="None"
                         Grid.ColumnSpan="2">
            <CollectionView.ItemTemplate>
                <DataTemplate x:DataType="model:Monkey">
                    <Grid Padding="10">
                        <Frame HeightRequest="125" Style="{StaticResource CardView}">
                            <Grid Padding="0" ColumnDefinitions="125,*">
                                <Image Aspect="AspectFill" Source="{Binding Image}"
                                       WidthRequest="125"
                                       HeightRequest="125"/>
                                <VerticalStackLayout
                                    Grid.Column="1"
                                    VerticalOptions="Center"
                                    Padding="10">
                                    <Label Style="{StaticResource LargeLabel}" Text="{Binding Name}" />
                                    <Label Style="{StaticResource MediumLabel}" Text="{Binding Location}" />
                                </VerticalStackLayout>
                            </Grid>
                        </Frame>
                    </Grid>
                </DataTemplate>
            </CollectionView.ItemTemplate>
        </CollectionView>

        <Button Text="Get Monkeys" 
                Command="{Binding GetMonkeysCommand}"
                IsEnabled="{Binding IsNotBusy}"
                Grid.Row="1"
                Grid.Column="0"
                Style="{StaticResource ButtonOutline}"
                Margin="8"/>

        <!-- Add this -->
        <ActivityIndicator IsVisible="{Binding IsBusy}"
                           IsRunning="{Binding IsBusy}"
                           HorizontalOptions="Fill"
                           VerticalOptions="Center"
			   Color="{StaticResource Primary}"
                           Grid.RowSpan="2"
                           Grid.ColumnSpan="2"/>
    </Grid>
</ContentPage>
```

### 运行应用程序

1、在Visual Studio中，将 iOS、Android、macOS 或 Windows项目 设置为启动项目

2. 在 Visual Studio 中，单击“开始调试”。 当应用程序启动时，您将看到一个 **Get Monkeys** 按钮，按下该按钮将从互联网中加载猴子数据！

让我们继续我们的动手实验旅程，在 [实验三: 添加导航页面](../Part%203%20-%20Navigation/README.zh-cn.md) 中了解 Navigation

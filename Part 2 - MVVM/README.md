## MVVM & Data Binding

In Part 2 we will introduce full data binding with MVVM and retrieve the monkeys from an internet data source.

This module is also available in [Chinese (Simplified)](README.zh-cn.md) & [Chinese (Traditional)](README.zh-tw.md).

### Implementing INotifyPropertyChanged

*INotifyPropertyChanged* is important for data binding in MVVM Frameworks. This is an interface that when implemented, lets our view know about changes to the model. We will implement it once in our `BaseViewModel` so all other view models that we create can inherit from it.

1. In Visual Studio, open `ViewModel/BaseViewModel.cs`
2. In `BaseViewModel.cs`, implement INotifyPropertyChanged by changing this

```csharp
public class BaseViewModel
{

}
```

to this

```csharp
public class BaseViewModel : INotifyPropertyChanged
{

}
```

3. In `BaseViewModel.cs`, right click on `INotifyPropertyChanged`
4. Implement the `INotifyPropertyChanged` Interface
   - (Visual Studio Mac) In the right-click menu, select Quick Fix -> Implement Interface
   - (Visual Studio PC) In the right-click menu, select Quick Actions and Refactorings -> Implement Interface
5. In `BaseViewModel.cs`, ensure this line of code now appears:

```csharp
public event PropertyChangedEventHandler PropertyChanged;
```

6. In `BaseViewModel.cs`, create a new method called `OnPropertyChanged`
    - Note: We will call `OnPropertyChanged` whenever a property updates

```csharp
public void OnPropertyChanged([CallerMemberName] string name = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
```

### Implementing Title, IsBusy, and IsNotBusy

We will create a backing field and accessors for a few properties. These properties will allow us to set the title on our pages and also let our view know that our view model is busy so we don't perform duplicate operations (like allowing the user to refresh the data multiple times). They are in the `BaseViewModel` because they are common for every page.

1. In `BaseViewModel.cs`, create the backing field:

```csharp
public class BaseViewModel : INotifyPropertyChanged
{
    bool isBusy;
    string title;
    //...
}
```

2. Create the properties:

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

Notice that we call `OnPropertyChanged` when the value changes. The .NET MAUI binding infrastructure will subscribe to our **PropertyChanged** event so the UI will be notified of the change.

We can also create the inverse of `IsBusy` by creating another property called `IsNotBusy` that returns the opposite of `IsBusy` and then raising the event of `OnPropertyChanged` when we set `IsBusy`

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


### Simplifying MVVM with .NET Community Toolkit

Now that you have an understanding of how MVVM works, let's look at a way to simplify development. As applications get more complex, more properties and events will be added. This leads to more boilerplate code being added. The .NET Community Toolkit seeks to simplify MVVM with source generators to automatically handle the code that we used to manually had to write. The `CommunityToolkit.Mvvm` library has been added to the project and we can start using it right away.

Delete all contents in `BaseViewModel.cs` and replace it with the following:

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

Here, we can see that our code has been greatly simplified with an `ObservableObject` base class that implements `INotifyPropertyChanged` and also attributes to expose our properties.

Note that both isBusy and title have the `[ObservableProperty]` attribute attached to it. The code that is generated looks nearly identical to what we manually wrote. Additionally, the isBusy property has `[NotifyPropertyChangedFor(nameof(IsNotBusy))]`, which will also notify `IsNotBusy` when the value changes. To see the generated code head to the project and then expand **Dependencies -> net6.0-android -> Analyzers -> CommunityToolkit.Mvvm.SourceGenerators -> CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator** and open `MonkeyFinder.ViewModel.BaseViewModel.cs`:


Here is what our `IsBusy` looks like:

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

This code may look a bit scary, but since it is auto-generated it adds additional attributes to avoid conflicts. It is also highly optimized with caching as well.

The same library will also help us handle click events aka `Commands` in the future.

> Note that we changed this class to a `partial` class so the generated code can be shared in the class.


### Create a Monkey Service

We are ready to create a method that will retrieve the monkey data from the internet. We will first implement this with a simple HTTP request using HttpClient. We will do this inside of our `MonkeyService.cs` file that is located in the `Services` folder.

1. Inside of the `MonkeyService.cs`, let's add a new method to get all Monkeys:

    ```csharp
    List<Monkey> monkeyList = new ();
    public async Task<List<Monkey>> GetMonkeys()
    {
        return monkeyList;
    }
    ```

    Right now, the method simply creates a new list of Monkeys and returns it. We can now fill in the method use `HttpClient` to pull down a json file, parse it, cache it, and return it.

1. Let's get access to an `HttpClient` by added into the contructor for the `MonkeyService`.

    ```csharp
     HttpClient httpClient;
    public MonkeyService()
    {
        this.httpClient = new HttpClient();
    }
    ```

    .NET MAUI includes dependency injection similar to ASP.NET Core. We will register this service and dependencies soon.

1. Let's check to see if we have any monkeys in the list and return it if so by filling in the `GetMonkeys` method:

    ```csharp
    if (monkeyList?.Count > 0)
        return monkeyList;
    ```

1. We can use the `HttpClient` to make a web request and parse it using the built in `System.Text.Json` deserialization.

    ```csharp
    var response = await httpClient.GetAsync("https://www.montemagno.com/monkeys.json");

    if (response.IsSuccessStatusCode)
    {
        monkeyList = await response.Content.ReadFromJsonAsync<List<Monkey>>();
    }
    
    return monkeyList;
    ```

1. Add the following using directive at the top of the file to access the `ReadFromJsonAsync` extension method:

    ```csharp
    using System.Net.Http.Json;
    ```

#### No Internet? No Problem!

If you have internet issues in your current setup don't worry as we have embedded a list of monkeys into the project. Instead of using `HttpClient`, you can read the file and return it:

```csharp
using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
using var reader = new StreamReader(stream);
var contents = await reader.ReadToEndAsync();
monkeyList = JsonSerializer.Deserialize<List<Monkey>>(contents);
```


### Call MonkeyService from ViewModel

We now can update our `MonkeysViewModel` to call our new monkey service and expose the list of monkeys to our user interface. 

We will use an `ObservableCollection<Monkey>` that will be cleared and then loaded with **Monkey** objects. We use an `ObservableCollection` because it has built-in support to raise `CollectionChanged` events when we Add or Remove items from the collection. This means we don't call `OnPropertyChanged` when updating the collection.

1. In `MonkeysViewModel.cs` declare a property which we will initialize to an empty collection. Also, we can set our Title to `Monkey Finder`.

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
    
1. We will want to access our new `MonkeyService`. So let's add the following using directive to the top of the file:

    ```csharp
    using MonkeyFinder.Services;
    ```

1. We also need access to our `MonkeyService`, which we will inject throught he constructor:

    ```csharp
    public ObservableCollection<Monkey> Monkeys { get; } = new();
    MonkeyService monkeyService;
    public MonkeysViewModel(MonkeyService monkeyService)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
    }
    ```

1. In `MonkeysViewModel.cs`, create a method named `GetMonkeysAsync` that returns `async Task`:

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

1. In `GetMonkeysAsync`, first ensure `IsBusy` is false. If it is true, `return`

    ```csharp
    async Task GetMonkeysAsync()
    {
        if (IsBusy)
            return;
    }
    ```

1. In `GetMonkeysAsync`, add some scaffolding for try/catch/finally blocks
    - Notice, that we toggle *IsBusy* to true and then false when we start to call to the server and when we finish.

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

1. In the `try` block of `GetMonkeysAsync`, we can get the monkeys from our `MonkeyService`.

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

1. Still inside of the `try` block, clear the `Monkeys` property and then add the new monkey data:

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

1. In `GetMonkeysAsync`, add this code to the `catch` block to display a popup if the data retrieval fails:

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

1. Ensure the completed code looks like this:

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

1.  Finally, let's expose this method via an `ICommand` that we can data bind to. Normally, we would have to create a backing field such as:

    ```csharp
    public Command GetMonkeysCommand { get; }
    public MonkeysViewModel()
    {
        //...
        GetMonkeysCommand = new Command(async () => await GetMonkeysAsync());
    }
    ```

    However, with the .NET Community Toolkit we simply can add the `[RelayCommand]` attribute to our method:

    ```csharp
     [RelayCommand]
    async Task GetMonkeysAsync()
    {
        //..
    }
    ```

    This will automatically create all of the code we need:

    ```csharp
    partial class MonkeysViewModel
    {
        /// <summary>The backing field for <see cref="GetMonkeysCommand"/>.</summary>
        [global::System.CodeDom.Compiler.GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ICommandGenerator", "8.0.0.0")]
        private global::CommunityToolkit.Mvvm.Input.AsyncRelayCommand? getMonkeysCommand;
        /// <summary>Gets an <see cref="global::CommunityToolkit.Mvvm.Input.IAsyncRelayCommand"/> instance wrapping <see cref="GetMonkeysAsync"/>.</summary>
        [global::System.CodeDom.Compiler.GeneratedCode("CommunityToolkit.Mvvm.SourceGenerators.ICommandGenerator", "8.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCode]
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public global::CommunityToolkit.Mvvm.Input.IAsyncRelayCommand GetMonkeysCommand => getMonkeysCommand ??= new global::CommunityToolkit.Mvvm.Input.AsyncRelayCommand(new global::System.Func<global::System.Threading.Tasks.Task>(GetMonkeysAsync));
    }
    ```

    MAGIC!

Our main method for getting data is now complete!

### Register Services

Before we can run the app, we must register all of our dependencies. Open the `MauiProgram.cs` file. 

1. Add the following using directive to access our `MonkeyService`:

	```csharp
	using MonkeyFinder.Services;
	```

1. Find where we are  registering our `MainPage` with `builder.Services` and add the following above it:
	```csharp
	builder.Services.AddSingleton<MonkeyService>();
	builder.Services.AddSingleton<MonkeysViewModel>();
	```

We are registering the `MonkeyService` and `MonkeysViewModel` as singletons. This means they will only be created once, if we wanted a unique instance to be created each request we would register them as `Transient`.


1. In the code behind for the project we will inject our `MonkeysViewModel` into our MainPage:

    ```csharp
    public MainPage(MonkeysViewModel viewModel)
    {
	InitializeComponent();
	BindingContext = viewModel;
    }
    ```

## Build The Monkeys User Interface
It is now time to build the .NET MAUI user interface in `View/MainPage.xaml`. Our end result is to build a page that looks like this:

![](../Art/FinalUI.PNG)

1. In `MainPage.xaml`, add a `x:DataType` at the top of the `ContentPage` tag, which will enable us to get binding intellisense:

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

    This is called a compiled binding. We are specifying that we will be binding directly to the `MonkeysViewModel`. This will do error checking and has performance enhancements.

1. We can create our first binding on the `ContentPage` by adding the `Title` Property:

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

1. In the `MainPage.xaml`, we can add a `Grid` between the `ContentPage` tags with 2 rows and 2 columns. We will also set the `RowSpacing` and `ColumnSpacing` to

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

1. In the `MainPage.xaml`, we can add a `CollectionView` between the `Grid` tags that spans 2 Columns. We will also set the `ItemsSource` which will bind to our `Monkeys` ObservableCollection and additionally set a few properties for optimizing the list.

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

1. In the `MainPage.xaml`, we can add a `ItemTemplate` to our `CollectionView` that will represent what each item in the list displays:

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

1. In the `MainPage.xaml`, we can add a `Button` under our `CollectionView` that will enable us to click it and get the monkeys from the server:

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
                                    Padding="10">
                                    <Label Style="{StaticResource LargeLabel}" Text="{Binding Name}" />
                                    <Label Style="{StaticResource MediumLabel}" Text="{Binding Location}" />
                                </StackLayout>
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


1. Finally, In the `MainPage.xaml`, we can add a `ActivityIndicator` above all of our controls at the very bottom or `Grid` that will show an indication that something is happening when we press the `Get Monkeys` button.

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
                           HorizontalOptions="FillAndExpand"
                           VerticalOptions="CenterAndExpand"
                           Grid.RowSpan="2"
                           Grid.ColumnSpan="2"/>
    </Grid>
</ContentPage>
```

### Run the App

1. In Visual Studio, set the iOS, Android, macOS, or Windows project as the startup project 

2. In Visual Studio, click "Start Debugging". When the application starts you will see a **Get Monkeys** button that when pressed will load monkey data from the internet!

Let's continue our journey and learn about Navigation in [Part 3](../Part%203%20-%20Navigation/README.md)

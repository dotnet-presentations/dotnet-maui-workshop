## Hands-on Lab Part 2: MVVM 和資料繫結

在 Hands-on Lab Part 2 當中，將來介紹如何透過 MVVM 框架進行完整的資料繫結處理，並從先前從網路當中載入的資料搜尋猴子。

### INotifyPropertyChanged 實作

*INotifyPropertyChanged* 對於應用 MVVM 框架中的資料繫結很重要。透過實作 INotifyPropertyChanged 這個介面，能夠讓畫面元件得知所繫結到的資料在 Model 中發生了改變，進而更新畫面上的資料。在後面的步驟中將在 `BaseViewModel` 中實作一次，以便後續要設計的所有的 ViewModel 都可以直接繼承此實作 INotifyPropertyChanged 介面的類別。

1. 在 Visual Studio 中，開啟 `ViewModel/BaseViewModel.cs`
   
2. 在 `BaseViewModel.cs` 中，透過如下改寫來實作 INotifyPropertyChanged 介面

```csharp
public class BaseViewModel
{

}
```

改寫為

```csharp
public class BaseViewModel : INotifyPropertyChanged
{

}
```

3. 在 `BaseViewModel.cs` 中，右鍵點擊 `INotifyPropertyChanged`
   
4. 實作 `INotifyPropertyChanged` 介面
    - (Visual Studio for Mac) 在右鍵選單當中，選擇 快速修正 (Quick Fix) -> 實作介面 (Implement Interface)
    - (Visual Studio PC) 在右鍵選單當中，選擇 快速修正與重構 (Quick Actions and Refactorings) -> 實作介面 (Implement Interface)
  
5. 在 `BaseViewModel.cs` 中，確認現在出現下列這行的程式碼：

```csharp
public event PropertyChangedEventHandler PropertyChanged;
```

6. 在 `BaseViewModel.cs` 中，建立一個名稱為 `OnPropertyChanged` 的方法，並且緊接著透過 Lamda Expression 來完成方法的撰寫。
     - 注意：每當屬性的資料有更新時，都會呼叫 `OnPropertyChanged` 這個方法

```csharp
public void OnPropertyChanged([CallerMemberName] string name = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
```
### 實作 Title、IsBusy 和 IsNotBusy

接著將在 ViewModel 中建立一些欄位資料與屬性設計，除了設計 Title 能讓畫面設定 Title 外，並且透過 IsBusy 讓所繫結 ViewModel 的畫面知道是否處於 busy 的狀態，透過這樣的手法就不會發生重複更新的問題（例如允許使用者多次更新資料）。而由於它們設計在 `BaseViewModel` 中，只要畫面所繫結到的 ViewModel 有繼承此 BaseViewModel 類別做為基底類別，那便可以知道有關的處理是否仍處於 busy 的狀態。

1. 在 `BaseViewModel.cs` 中，建立欄位資料：
   
```csharp
public class BaseViewModel : INotifyPropertyChanged
{
    bool isBusy;
    string title;
    //...
}
```
2. 建立對應到該欄位資料的屬性設計:

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

請注意，當資料值有變更時，將需要呼叫 `OnPropertyChanged`。 .NET MAUI 的繫結引擎 (Binding Engine) 將引發其中註冊的 **PropertyChanged** 事件，因此 UI 將會收到更新畫面的通知。

在這邊另外在建立一個名稱為 `IsNotBusy` 的屬性來設定為 `IsBusy` 反向其 bool 資料後並回傳結果，並在 `IsBusy` 的 Setter 當中增加呼叫 `OnPropertyChanged` 的方法，注意此次所增加的呼叫必須傳入 `nameof(IsNotBusy)` 作為該方法所設計的參數值。

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

### 使用 .NET Community Toolkit 簡化 MVVM 框架

現在經過前面的介紹已經了解 MVVM 的工作原理，接著來看看一種簡化開發的方式。隨著應用程式的開發越來越複雜，將會增加更多的屬性與方法。這將會使得程式碼當中增加許多不同的數。 .NET Community Toolkit 主要是透過 AOP 的處理來簡化 MVVM 框架的設計，以自動完成前面需手動撰寫的程式碼。 `CommunityToolkit.Mvvm` 套件引用已經有增加到專案中，可以直接使用。

刪除 `BaseViewModel.cs` 中所有的程式碼後，增加以下程式碼:

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

在這邊，可以看到目前的程式碼已變成透過繼承 `ObservableObject` 類別為基底類別的設計，由於它已經完成實作 `INotifyPropertyChanged` 及其相關繫結屬性的設計，並提供了 AOP 的模式供以掛載到在 `BaseViewModel` 中所設計的欄位。

所以可以注意到 isBusy 和 title 欄位都附掛了 `[ObservableProperty]` 的設計。所編譯後的程式碼看起來幾乎和先前設計撰寫的程式碼相同。另外，isBusy 欄位有額外掛上 `[NotifyPropertyChangedFor(nameof(IsNotBusy))]`，當欄位值有改變時也會通知 `IsNotBusy` 屬性。要查看編譯後的程式碼，請到透過方案總管的專案當中，並展開 **Dependencies -> net7.0-android -> Analyzers -> CommunityToolkit.Mvvm.SourceGenerators -> CommunityToolkit.Mvvm.SourceGenerators.ObservablePropertyGenerator** 並開啟 `MonkeyFinder.ViewModel. BaseViewModel.cs`：

以下是在編譯時期自動產生的 `IsBusy` 屬性:

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

這段程式碼可能看起來有點驚人，但由於它是編譯時期自動產生的，也必須增加額外的命名資訊以避免名稱衝突，也透過部分手法進行記憶體暫存的最佳化處理。

後面此 `CommunityToolkit.Mvvm` 也將幫忙處理畫面元件的部分命令 `Commands` 控制。

> 請注意，此類別設計有增加一個關鍵字 **partial** 的使用，將此類別改為 `partial` 類別的設計，以利自動產生的程式碼可以在此類別當中使用。

### 建立 Monkey Service

建立一個取得已經有的建立在的網路中 json 格式的猴子資料並搜尋猴子的方法。在這邊將使用 `HttpClient` 這個類別來建立一個簡單的 HTTP 請求，來取得已經建立在網際網路中的用 json 格式表示的猴子資料。打開位在 `Services` 資料夾中的 `MonkeyService.cs` 檔案，並且進行下列的步驟:

1. 在 `MonkeyService.cs` 中，我們設計一個方法來表示取所有猴子資料：

    ```csharp
    List<Monkey> monkeyList = new ();
    public async Task<List<Monkey>> GetMonkeys()
    {
        return monkeyList;
    }
    ```

    目前撰寫的只是建立一個新的 Monkeys 集合 (用 `List<T>` 類別) 回傳。

2. 在 `MonkeyService` 類別當中，來增加一個型態為 `HttpClient` 的欄位並將此欄位命名為 httpClient，接著在設計建構方法時，將該 **httpClient** 欄位設定一個 `HttpClient` 的物件。

    ```csharp
    HttpClient httpClient;
    public MonkeyService()
    {
        this.httpClient = new HttpClient();
    }
    ```

    .NET MAUI 的設計中有著類似於 ASP.NET Core 的 DI 設計，這將會在後面將很快地介紹到。

3. 在投過 httpClient 發出請求前，檢查一下 monkeyList 的集合當中是否已有資料。如果有，則無需再發出網路請求，直接回傳 `return monkeyList` 即可：
   
    ```csharp
    if (monkeyList?.Count > 0)
        return monkeyList;
    ```

4. 接著要繼續在 `GetMonkeys()` 方法當中使用 **httpClient** 發出請求，來取得網際網路中以 json 格式表示的猴子資料，再透過 `HttpClient` 這個類別所設計的 `GetAsync()` 方法發出網路請求後，接著使用在繼續使用 .NET 6 Library 內建的 `System.Text.Json` 來進行反序列化的處理。請將下列程式碼插入至 `return monkeyList` 之前。

    ```csharp
    var response = await httpClient.GetAsync("https://www.montemagno.com/monkeys.json");

    if (response.IsSuccessStatusCode)
    {
        monkeyList = await response.Content.ReadFromJsonAsync(MonkeyContext.Default.ListMonkey);
    }
    ```

5. 别忘記在文件一開始增加 using 命名空間以利使用到 `ReadFromJsonAsync` 這個擴展方法：
   
    ```csharp
    using System.Net.Http.Json;
    ```

#### 如果沒有網際網路

如果在目前的配置遇到連線到網際網路的問題，可以透過讀取已經放到專案當中用 json 格式儲存的猴子資料檔案。在這邊就是直接透過 `StreamReader` 類別來讀取該 .json 檔案的資料，而不是使用 `HttpClient`，讀取資料完成後，再透過 JsonSerializer 進行 Json 資料格式的轉換，為應用程式提供可操作的集合 (List<T>) 資料：

```csharp
using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
using var reader = new StreamReader(stream);
var contents = await reader.ReadToEndAsync();
monkeyList = JsonSerializer.Deserialize(contents, MonkeyContext.Default.ListMonkey);
```

### 從 ViewModel 使用 MonkeyService

現在可以在 `MonkeysViewModel` 中撰寫程式，來使用前面所設計 MonkeyService 並將猴子的列表資料展示到使用者介面中。

接下來將使用 `ObservableCollection<Monkey>` 這個集合來在類別當中設計一個 `Monkeys` 屬性，並在確認清除集合資料後能載入從 json 資料轉換來的 **Monkey** 物件集合。使用 `ObservableCollection` 的原因是這個集合類別在設計的時候，就已經有支援集合資料異動置時就引發 `CollectionChanged` 的事件，這代表著使用此類別的集合資料在更新時不需要自行呼叫 `OnPropertyChanged`。

1. 在 `MonkeysViewModel.cs` 的撰寫中，先將 Monkeys 屬性透過初始化設定為空集合。此時，也透過 MonkeyViewModel 建構式順便將 Title 的字串值設定為 `Monkey Finder`。

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

2. 由於要使用到前述設計的 `MonkeyService` 類別，因此在此 `MonkeysViewModel.cs` 當中加入 using 引用命名空間的撰寫：
   
    ```csharp
    using MonkeyFinder.Services;
    ```

3. 接著使用 `MonkeyService` 類別來設計一個 `monkeyService` 欄位，並透過建構方法的參數設定其資料物件(建構式注入)：
   
    ```csharp
    public ObservableCollection<Monkey> Monkeys { get; } = new();
    MonkeyService monkeyService;
    public MonkeysViewModel(MonkeyService monkeyService)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
    }
    ```

4. 繼續在 `MonkeysViewModel.cs` 中來設計一個回傳 `async Task` 且名稱為 `GetMonkeysAsync()` 的方法：
   
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

5. 在 `GetMonkeysAsync()` 方法當中，先確認 `IsBusy` 的資料值是否為 false，如果是 true 則直接透過 `return` 離開方法:
   
    ```csharp
    async Task GetMonkeysAsync()
    {
        if (IsBusy)
            return;
    }
    ```

6. 繼續在 `GetMonkeysAsync()` 方法中，撰寫 try/catch/finally 的敘述陳述式。
     - 注意，當使用 MonkeyService 開始運作時，要將 *IsBusy* 值設定為 true，當運作完畢後則要將 *IsBusy* 設定為 false。
  
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

7. 在 `GetMonkeysAsync()` 方法的 `try` 區塊中，就透過 `MonkeyService` 所設計的 `GetMonkeys()` 方法來取得猴子的資料，並且準備一個暫時的 monkeys 區域變數來暫存取得的資料。
   
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

8. 仍繼續在 `try` 區塊中，清除 `Monkeys` 属性中所記錄的所有猴子資料，然後再將 monkeys 的猴子資料加入到 Monkeys 這個屬性集合當中：

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

9. 在 `GetMonkeysAsync()` 方法中，將下列 `catch` 區塊中的程式碼以便確認資料取得失敗時，並在 UI 中顯示出 Alert：

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

10. 確認所完成 `GetMonkeysAsync()` 方法的整段程式碼如下所示：

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

12. 最後，通常可以透過 `ICommand` 來設計一個可繫結的屬性來公開這個 `GetMonkeysAsync()` 方法，則會撰寫一段類似如下的程式碼：

    ```csharp
    public Command GetMonkeysCommand { get; }
    public MonkeysViewModel()
    {
        //...
        GetMonkeysCommand = new Command(async () => await GetMonkeysAsync());
    }
    ```

    但透過使用 **.NET Community Toolkit** 套件，就可以簡單的使用 `[RelayCommand]` 掛到方法的設計上：

    ```csharp
    [RelayCommand]
    async Task GetMonkeysAsync()
    {
        //..
    }
    ```

    而把 `[RelayCommand]` 掛上後，該方法則會自動編譯出對應的程式碼，大致如下：

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

到這邊 MonkeysViewModel 類別要用來取得猴子資料的方法終於完成了!  

### 註冊 Services

在執行此應用程式之前，必須註冊所設計的 Service，所以來開啟 `MauiProgram.cs` 檔案。

1. 增加以下 using 命名空間來準備使用 `MonkeyService`：

	```csharp
	using MonkeyFinder.Services;
	```

2. 找到在 `builder.Services` 中先前註冊 `MainPage` 的地方，並在其上方增加下列程式碼：
   
   	```csharp
	builder.Services.AddSingleton<MonkeyService>();
	builder.Services.AddSingleton<MonkeysViewModel>();
	```

在這邊是將 `MonkeyService` 和 `MonkeysViewModel` 註冊為 **Singleton**，這將意味著他們都在整個應用程式中都只會被建立一次執行個體來執行。如果希望每次請求會變成獨立的執行個體來執行，是可以透過 AddTransient 的方式將他們註冊為 **Transient**。

3. 在 MainPage 對應的後置程式碼當中，則將 `MonkeysViewModel` 透過建構式注入的方式，把此 MonkeysViewModel 的物件設定到 MainPage 的 BindingContext 屬性當中：

    ```csharp
    public MainPage(MonkeysViewModel viewModel)
    {
	InitializeComponent();
	BindingContext = viewModel;
    }
    ```


## 建立使用者介面

到這邊要進入 `View/MainPage.xaml` 中來撰寫 .NET MAUI 使用者介面了。最終的結果會是建立出如下所示的畫面效果：

![](../Art/FinalUI.PNG)

1. 開啟 `MainPage.xaml` 後，找到 `ContentPage` 標記後在其中加入 `x:DataType` 並設定其值為 `viewmodel:MonkeysViewModel`，這將能取得資料繫結的自動通知能力：

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

    這邊將 x:DataType 設定為 `MonkeysViewModel`，這個動作稱之為編譯繫結，將能進行編譯時期檢查型別錯誤並加速執行時的效能。

2. 接著在 `ContentPage` 標記當中，透過 `Title` 屬性來撰寫資料繫結 `{Binding Title}` 完成設定 `ContentPage` 中 `Title` 的資料值：

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

3. 繼續在 `MainPage.xaml` 中，在 `ContentPage` 標記當中撰寫一個將版面規劃成 2 欄 2 列的 `Grid` 版型設計控制項，並設定其 `RowSpacing` 為 0 和 `ColumnSpacing` 為 5

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

4. 持續在 `MainPage.xaml` 當中，並在 `Grid` 標記當中增加一個 `CollectionView` 的資料集合呈現控制項，將其設定為跨 `Grid` 的 2 欄，並設置其 `ItemsSource` 為绑定到 `Monkeys` 這個 ObservableCollection 集合物件，另外再設定 `SelectionMode` 屬性值為 `None`，將 CollectionView 的項目被點選時無反應的效果。

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

5. 繼續在 `MainPage.xaml` 當中，幫 `CollectionView` 增加一個 `ItemTemplate` 的設計，以呈現這個 CollectionView 當中每個項目要展示的內容：
   
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

6. 維持在 `MainPage.xaml` 當中，在 `CollectionView` 的標記下方增加一個 `Button`，再設定一些配合 `Grid` 的版型設定的屬性值，透過這個 `Button` 的點選操作，就可以從前面所設計的 `MonkeyService` 取得猴子資料：

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

7. 最後在 `MainPage.xaml` 中，在 `Button` 標記的下方再增加一個 `ActivityIndicator` 標記(仍在 `Grid` 標記中)，並且設定一些配合 `Grid` 的版型設定的屬性值，而透過點選 **Get Monkeys** 的按鈕時，這個 `ActivityIndicator` 這個 UI 控制項將會發生運作中的指示讓使用者知道。

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

### 偵錯執行此應用程式

1. 在 Visual Studio 中，選擇要執行的 iOS、Android、macOS 或 Windows 的架構 (framework)。

2. 在 Visual Studio 中，點選 `開始偵錯`。而當應用程式啟動後將看到畫面上有一個 **Get Monkeys** 按鈕。點選該按鈕後，則會看到從網際網路當中讀取的猴子資料並呈現到畫面中！

再繼續後面的 Hands-on Lab 的部分吧！前往 [Hands-on Lab Part 3: 增加巡覽功能](../Part%203%20-%20Navigation/README.zh-tw.md) 中了解巡覽 (Navigation) 的使用。

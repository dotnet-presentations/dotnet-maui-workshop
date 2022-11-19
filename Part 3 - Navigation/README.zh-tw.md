## Hands-on Lab Part 3: 增加巡覽功能

在此 Hands-on Lab Part 3 中，將增加簡單的巡覽功能來進行頁面的轉跳，而呈現的新頁面將用來呈現猴子的完整詳細資訊。

而在這邊將使用 .NET MAUI 的內建的 Shell 進行巡覽，透過基於 URI 模式的巡覽系統，能夠更簡易的完成頁面轉跳，並且可以透過 QueryString 的方式來傳遞參數（例如: 字串或物件...等資料）到新頁面當中。

例如，假設我們想要巡覽到新頁面呈現猴子的詳細資訊時，可以在 URI 直接帶入一個 QueryString 的字串值。

```csharp
await Shell.Current.GoToAsync("DetailsPage?name=james");
```

接著在呈現猴子的詳細資訊頁面 (Page) 或是其對應的 ViewModel 設計當中，可以在其類別掛上 `[QueryProperty]` 並設計其對應的属性動作 (其下以 Page 的類別設計為例)：

```csharp
[QueryProperty(nameof(Name), "name")]
public partial class DetailsPage : ContentPage
{
    string name;
    public string Name
    {
        get => name;
        set => name = value;
    }
}
```

在進行頁面轉跳時，QueryString 的值 `james` 將會自動傳遞到 `DetailPage` 所設計的 `Name` 屬性當中。也可以使用類似的手法(呼叫 'GoToAsync()' 方法的多載)來傳遞一個完整的物件資料：

```csharp
var person = new Person { Name="James" };
await Shell.Current.GoToAsync("DetailsPage", new Dictionary<string, object>
{
    { "person", person }
});
```

接著在轉跳到的詳細資訊頁面 (Page) 或是其對應的 ViewModel 設計當中，可以在其類別掛上 `[QueryProperty]` 並設計其對應的屬性動作 (其下以 Page 的類別設計為例)：

```csharp
[QueryProperty(nameof(Person), "person")]
public partial class DetailsPage : ContentPage
{
    Person person;
    public Person Person
    {
        get => person;
        set => person = value;
    }
}
```

當進行頁面轉跳時 `Person` 物件將會自動的序列化或反序列化來傳遞資料。

接著透過 ViewModel 來增加一個點選後會透過巡覽轉跳到詳細頁面並傳遞猴子資料的處理。

### 增加點選動作的處理

準備透過 `Shell` 所設計的 `GoToAsync()` 方法轉跳到呈現猴子的詳細訊息頁面！

1. 在 `MonkeysViewModel.cs` 當中，建立一個 `async Task GoToDetailsAsync(Monkey monkey)` 的方法，並透過 `[RelayCommand]` 的掛載，形成可公開的操作 `Command`：

    ```csharp
    [RelayCommand]
    async Task GoToDetails(Monkey monkey)
    {
        if (monkey == null)
	    return;

        await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
        {
            {"Monkey", monkey }
        });
    }
    ```

    - 此方法中一開始先檢查參數所得到的資料是否為空值，若不是空值才透過 Shell 內建用以 `Navigation` 的 `GoToAsync()` 方法，並將帶入的猴子資料為呼叫方法的引數資料，來轉跳進入詳細頁面。

2. 在 `MainPage.xaml` 當中，找到 `CollectionView.ItemTemplate` 有關設置 `Frame` 的部分，並增加中 `TapGestureRecognizer` 的事件：

    增加前:  

    ```xml
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Monkey">
            <Grid Padding="10">
                <Frame HeightRequest="125" Style="{StaticResource CardView}">
                    <Grid Padding="0" ColumnDefinitions="125,*">
                        <Image
                            Aspect="AspectFill"
                            HeightRequest="125"
                            Source="{Binding Image}"
                            WidthRequest="125" />
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
    ```

    增加後:  
    
    ```xml
    <CollectionView.ItemTemplate>
        <DataTemplate x:DataType="model:Monkey">
            <Grid Padding="10">
                <Frame HeightRequest="125" Style="{StaticResource CardView}">
                    <!-- Add the Gesture Recognizer-->
                    <Frame.GestureRecognizers>
                        <TapGestureRecognizer 
                                Command="{Binding Source={RelativeSource AncestorType={x:Type viewmodel:MonkeysViewModel}}, Path=GoToDetailsCommand}"
                                CommandParameter="{Binding .}"/>
                    </Frame.GestureRecognizers>
                    <Grid Padding="0" ColumnDefinitions="125,*">
                        <Image
                            Aspect="AspectFill"
                            HeightRequest="125"
                            Source="{Binding Image}"
                            WidthRequest="125" />
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
    ```

    這邊使用了 `RelativeSource` 的設定，使得其資料繫結的資料來源為透過 `AncestorType` 來指定繫結階層中型態為 `MonkeysViewModel` 的資料，透過這樣的設計則可以讓 Frame 控制項的 `BindingContext` 不再繫結到原本透過 `DataTemplate` 而對應繫結到的 `Monkey` 物件，而當然也允許指定繫結到更多階層的型態指定。

### 為 DetailPage 設計其 ViewModel 

1. 開啟 `ViewModel/MonkeyDetailsViewModel.cs` 檔案，來設計將猴子資料指定給 ViewModel 的程式。在這邊先來幫 `Monkey` 屬性掛上 [ObservableProperty] 來建立成一個可繫結的屬性：

    ```csharp
    public partial class MonkeyDetailsViewModel : BaseViewModel
    {
        public MonkeyDetailsViewModel()
        {
        }

        [ObservableProperty]
        Monkey monkey;    
    }

2. 接著在此 MonkeyDetailsViewModel 在掛上 `[QueryProperty]` 來處理傳遞進來的猴子資料：

    ```csharp
    //Add QueryProperty
    [QueryProperty(nameof(Monkey), "Monkey")]
    public partial class MonkeyDetailsViewModel : BaseViewModel
    {
        public MonkeyDetailsViewModel()
        {
        }

        [ObservableProperty]
        Monkey monkey;
    }
    ```

## 註冊頁面的路由

現在已經有設計了 DetailPage 以及其 ViewModel，在 `Shell` 的運作當中需要註冊其路由，這就透過 Shell 的註冊和 .NET MAUI 服務註冊完成的。

1. 開啟 `AppShell.xaml.cs` 這個檔案，找到在其 AppShell 類別的建構方法當中的 `InitializeComponent();` 程式碼，並在其下加入下列程式碼：

    ```csharp
    Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
    ```
    這將能讓 `DetailsPage` 這個頁面註冊為 **DetailPage** 的路由資訊。

2. 開啟 `MauiProgram.cs` 並將 DetailPage 與其 ViewModel 透過 `AddTransient<T>()` 加入服務中，這將會讓每次巡覽到 DetailPage 時會建立一個新的 Page 與 ViewModel 物件：

    ```csharp
    builder.Services.AddTransient<MonkeyDetailsViewModel>();
    builder.Services.AddTransient<DetailsPage>();
    ```

3. 最後，開啟 `DetailsPage.xaml.cs` 這個 DetailPage.xaml 所對應的後置程式碼檔案，並將 DetailPage 的建構方法改為如下內容：

    ```csharp
	public DetailsPage(MonkeyDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    ```
    這將 ViewModel 透過建構式注入到 `DetailsPage` 中。  
    
### 幫 DetailsPage.xaml 建立使用者介面

將下列變更加入到 DetailsPage 中。最終希望呈現出如下的精美介面來展示猴子的詳細資料：

![](../Art/Details.PNG)

1. 首先透過 ViewModel 的命名空間的引用並設置 x:DataType 為期對應的 ViewModel 後，即可將 ContentPage 的 Title 設定資料繫結：

    ```xml
    <ContentPage
        xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
        x:Class="MonkeyFinder.DetailsPage"
        xmlns:viewmodel="clr-namespace:MonkeyFinder.ViewModel"
        x:DataType="viewmodel:MonkeyDetailsViewModel"
        Title="{Binding Monkey.Name}">

        <!-- Add Content Here -->
    </ContentPage>
    ```

2. 而在 ContentPage 的標記當中增加 `ScrollView` 和 `Grid` 的相關標記，這些搭配能呈現出不錯的畫面配置對應效果：

    ```xml
    <ScrollView>
        <Grid RowDefinitions="Auto,Auto,*">

            <!-- Background, Image of Monkey, Name -->
        
            <!-- Details of Monkey -->

        </Grid>
    </ScrollView>
    ```

3. 接著在 `Grid` 標記當中放置其下的 XAML 標記碼：

    ```xml
    <BoxView
        BackgroundColor="{StaticResource Primary}"
        Grid.RowSpan="2"
        HorizontalOptions="Fill"
        VerticalOptions="Fill"/>

    <Border StrokeShape="RoundRectangle 80"
            Stroke="White"
            StrokeThickness="6"
            HeightRequest="160"
            WidthRequest="160"
            Margin="0,8,0,0"
            HorizontalOptions="Center"
            VerticalOptions="Center">
            <Image Aspect="AspectFill"
                HeightRequest="160"
                HorizontalOptions="Center"
                VerticalOptions="Center"
                Source="{Binding Monkey.Image}"
                WidthRequest="160"/>
    </Border>

    <Label Style="{StaticResource LargeLabel}" 
            Grid.Row="1"
            TextColor="White"
            FontAttributes="Bold"
            Text="{Binding Monkey.Name}" 
            HorizontalOptions="Center"
            Margin="0,0,0,8"/>
    ```
    
    此為放置一個 BoxView 來設定背景色為黃色，並將猴子的影像調整成圓形呈現。

4. 在 `Grid` 標記的下方 (但仍在 `VerticalStackLayout` 標記的內部)，將透過 Label 來呈現有關猴子的詳細資料。
   

```xml
<VerticalStackLayout Grid.Row="2" Padding="10" Spacing="10">
    <Label Style="{StaticResource MediumLabel}" Text="{Binding Monkey.Details}" />
    <Label Style="{StaticResource SmallLabel}" Text="{Binding Monkey.Location, StringFormat='Location: {0}'}" />
    <Label Style="{StaticResource SmallLabel}" Text="{Binding Monkey.Population, StringFormat='Population: {0}'}" />
</VerticalStackLayout>
```

5. 就如同前面的 Hands-on Lab 的 Part，在要測試的架構當中來選定執行目標後，來執行應用程式。並點選 CollectionView 當中的一個項目進行頁面的巡覽轉跳！

平台特性是下一個 Hands-on Lab 的 Part 要進行的部分，前進到 [Hands-on Lab Part 4: 了解平台特性](../Part%204%20-%20Platform%20Features/README.zh-tw.md) 以開始下一個階段的 Hands-on Lab。
   

## Navigation

In Part 3 we will add simple navigation to push a new page onto the stack to display details about the monkey.

We will use the build in Shell navigation of .NET MAUI. This powerfull navigation system is based on URIs. You can pass additional information while navigating query paramater such as a string, or a full object.

For example, let's say we wanted to navigate to a details page and pass in an identifier. 

```csharp
await Shell.Current.GoToAsync("DetailsPage?name=james");
```

Then in our details page or view model we sould define this property:

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

When we navigate, the name "james" would be passed along automatically. We can also pass a full object as well using the same mechanism:

```csharp
var person = new Person { Name="James" };
await Shell.Current.GoToAsync("DetailsPage", new Dictionary<string, object)
{
    { "person", person }
});
```

Then on our page or view model we would create the property.

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

Here, the `Person` is automatically serialized and deserialized for us when we navigate.

Now, let's add a click handler to the collection view and pass the monkey to the details page.

### Add Selected Event

Now, let's add navigation to a second page that displays monkey details!

1. In `MainPage.xaml` we can add an `SelectionChanged` event to the `CollectionView`:

    Before:

    ```xml
    <CollectionView
        Grid.ColumnSpan="2"
        ItemsSource="{Binding Monkeys}"
        SelectionMode="Single">
    ```

    After:
    ```xml
    <CollectionView
        Grid.ColumnSpan="2"
        ItemsSource="{Binding Monkeys}"
        SelectionChanged="CollectionView_SelectionChanged"
        SelectionMode="Single">
    ```


1. In `MainPage.xaml.cs`, create a method called `CollectionView_SelectionChanged`:


    ```csharp
    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var monkey = e.CurrentSelection.FirstOrDefault() as Monkey;
        if (monkey == null)
            return;

        await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
		{
			{"Monkey", monkey }
		});

        ((CollectionView)sender).SelectedItem = null;
    }
    ```

    - This code checks to see if the selected item is non-null and then uses the built in Shell `Navigation` API to push a new page with the monkey as a parameter and then deselects the item. 

### ViewModel for Details

1. Inside of our `ViewModel/MonkeyDetailsViewModel.cs`, we will house our logic for assigning the monkey to the view model. Let's first create a bindable property for the `Monkey`:

    ```csharp
    public partial class MonkeyDetailsViewModel : BaseViewModel
    {
        public MonkeyDetailsViewModel()
        {
        }

        [ObservableProperty]
        Monkey monkey;    
    }
    ```

1. Next, we will add a `QueryProperty` to handle passing the monkey data:

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

### Create DetailsPage.xaml UI

Let's add UI to the DetailsPage. Our end goal is to get a fancy profile screen like this:

![](../Art/Details.PNG)


1. Let's first start by definig our DataType by defining the viewmodel namespace and also setting the title:

    ```xml
    <ContentPage
        x:Class="MonkeyFinder.DetailsPage"
        xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
        xmlns:viewmodel="clr-namespace:MonkeyFinder.ViewModel"
        x:DataType="viewmodel:MonkeyDetailsViewModel"
        Title="{Binding Monkey.Name}">

        <!-- Add Content Here -->
    </ContentPage>
    ```

1. At the core is a `ScrollView`, `VerticalStackLayout`, and `Grid` to layout all of the controls nicely on the screen:

    ```xml
    <ScrollView>
        <VerticalStackLayout>
            <Grid ColumnDefinitions="*,Auto,*" RowDefinitions="100, Auto">

                <!-- Background and Image of Monkey -->
           
            </Grid>
        
            <!-- Details of Monkey -->

        </VerticalStackLayout>
    </ScrollView>
    ```

1. We can now fill in our `Grid` with the following code to place a box as the background color of yellow, and then our monkey image cut out in the shape of a circle:

    ```xml
    <BoxView
        Grid.ColumnSpan="3"
        BackgroundColor="{StaticResource Primary}"
        HeightRequest="160"
        HorizontalOptions="FillAndExpand" />
    <Frame
        Grid.RowSpan="2"
        Grid.Column="1"
        Margin="0,80,0,0"
        HorizontalOptions="Center" 
        Padding="0"
        CornerRadius="80">
        <Image
            Aspect="AspectFill"
            HeightRequest="160"
            HorizontalOptions="Center"
            VerticalOptions="Center"
            Source="{Binding Monkey.Image}"
            WidthRequest="160"/>
    </Frame>
    ```

1. Finally, under the `Grid`, but inside of the `VerticalStackLayout` we will add details about the monkey.

```xml
<VerticalStackLayout Margin="0,-50,0,0" Padding="10" Spacing="10">
    <Label Text="{Binding Monkey.Details}" />
    <Label FontSize="Micro" Text="{Binding Monkey.Location, StringFormat='Location: {0}'}" />
    <Label FontSize="Micro" Text="{Binding Monkey.Population, StringFormat='Population: {0}'}" />
</VerticalStackLayout>
```

## Registering Routing

Now that we have our details page in place we need to register it for routing. This is done in both the Shell routing system and with the .NET MAUI dependency service.

1. Open `AppShell.xaml.cs` code behind and add the following code into the constructor under the `InitializeComponent();` invoke:

    ```csharp
    Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
    ```

    This will register the details page with the route of "DetailsPage", which we used earlier.

1. Open `MauiProgram.cs` and add  both the view model and the page as `Transient` so a new page and viewmodel is created each time it is navigated to:

    ```csharp
    builder.Services.AddTransient<MonkeyDetailsViewModel>();
    builder.Services.AddTransient<DetailsPage>();
    ```

1. Finally, we must inject the viewmodel into our `DetailsPage`. Open the code behind for the page in `DetailsPage.xaml.cs` and change the constructor to the following:

    ```csharp
	public DetailsPage(MonkeyDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
    ```

1. Run the application on the desired platform and tap on a monkey to navigate!

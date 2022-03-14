## Navigation

In Part 3 we will add simple navigation to push a new page onto the stack to display details about the monkey.

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


2. In `MainPage.xaml.cs`, create a method called `CollectionView_SelectionChanged`:
    - This code checks to see if the selected item is non-null and then uses the built in `Navigation` API to push a new page and deselect the item.

```csharp
private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    var monkey = e.CurrentSelection.FirstOrDefault() as Monkey;
    if (monkey == null)
        return;

    await Navigation.PushAsync(new DetailsPage(monkey));

    ((CollectionView)sender).SelectedItem = null;
}
```

### ViewModel for Details

1. Inside of our `ViewModel/MonkeyDetailsViewModel.cs`, we will house our logic for assigning the monkey to the view model.

Let's first create a bindable property for the `Monkey`:

```csharp
public class MonkeyDetailsViewModel : BaseViewModel
{
    public MonkeyDetailsViewModel()
    {
    }

    public MonkeyDetailsViewModel(Monkey monkey)
        : this()
    {
        Monkey = monkey;
        Title = $"{Monkey.Name} Details";
    }
    Monkey monkey;
    public Monkey Monkey
    {
        get => monkey;
        set
        {
            if (monkey == value)
                return;

            monkey = value;
            OnPropertyChanged();
        }
    }
}
```

### Create DetailsPage.xaml UI

Let's add UI to the DetailsPage. Our end goal is to get a fancy profile screen like this:

![](Art/Details.PNG)

At the core is a `ScrollView`, `StackLayout`, and `Grid` to layout all of the controls nicely on the screen:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage 
             Title="{Binding Title}"> <!-- Add this-->
    <d:ContentPage.BindingContext>
        <viewmodel:MonkeyDetailsViewModel/>
    </d:ContentPage.BindingContext>
    <ScrollView>
        <StackLayout>
            <Grid ColumnDefinitions="*,Auto,*" RowDefinitions="100, Auto">
                <!-- Monkey image and background -->
            </Grid>   
            <!-- Name and details -->
        </StackLayout>
    </ScrollView>
</ContentPage>
```

We can now fill in our `Grid` with the following code:

```xml
<Rectangle
    Grid.ColumnSpan="3"
    BackgroundColor="{StaticResource Primary}"
    HeightRequest="100"
    HorizontalOptions="FillAndExpand" />
<StackLayout
    Grid.RowSpan="2"
    Grid.Column="1"
    Margin="0,50,0,0">

    <Image
        Aspect="AspectFill"
        HeightRequest="100"
        Source="{Binding Monkey.Image}"
        VerticalOptions="Center"
        WidthRequest="100">
        <Image.Clip>
            <EllipseGeometry
                Center="50,50"
                RadiusX="50"
                RadiusY="50" />
        </Image.Clip>
    </Image>
</StackLayout>

<Label
    Grid.Row="1"
    Margin="10"
    FontSize="Micro"
    HorizontalOptions="Center"
    Text="{Binding Monkey.Location}" />
<Label
    Grid.Row="1"
    Grid.Column="2"
    Margin="10"
    FontSize="Micro"
    HorizontalOptions="Center"
    Text="{Binding Monkey.Population}" />
```

Finally, under the `Grid`, but inside of the `StackLayout` we will add details about the monkey.

```xml
<Label
    FontAttributes="Bold"
    FontSize="Medium"
    HorizontalOptions="Center"
    Text="{Binding Monkey.Name}" />

<Label Margin="10" Text="{Binding Monkey.Details}" />
```
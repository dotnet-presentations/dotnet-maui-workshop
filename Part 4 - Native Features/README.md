## Accessing Native Features

In Part 4, we will use Xamarin.Essentials to find the closest monkey to us and also open a map with the Monkeys location.

### Find Closest Monkey!

We can add more functionality to this page using the GPS of the device since each monkey has a latitude and longitude associated with it.

1. In our `MonkeysViewModel.cs`, let's create another method called `GetClosestAsync`:

```csharp
async Task GetClosestAsync()
{

}
```

We can then fill it in by using Xamarin.Essentials to query for our location and helpers that find the closest monkey to us:

```csharp
async Task GetClosestAsync()
{
    if (IsBusy || Monkeys.Count == 0)
        return;

    try
    {
        // Get cached location, else get real location.
        var location = await Geolocation.GetLastKnownLocationAsync();
        if (location == null)
        {
            location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Medium,
                Timeout = TimeSpan.FromSeconds(30)
            });
        }

        // Find closest monkey to us
        var first = Monkeys.OrderBy(m => location.CalculateDistance(
            new Location(m.Latitude, m.Longitude), DistanceUnits.Miles))
            .FirstOrDefault();

        await Application.Current.MainPage.DisplayAlert("", first.Name + " " +
            first.Location, "OK");

    }
    catch (Exception ex)
    {
        Debug.WriteLine($"Unable to query location: {ex.Message}");
        await Application.Current.MainPage.DisplayAlert("Error!", ex.Message, "OK");
    }
}
```

2. We can now create a new `Command` that we can bind to:

```csharp
// ..
public Command GetClosestCommand { get; }
public MonkeysViewModel()
{
    // ..
    GetClosestCommand = new Command(async () => await GetClosestAsync());
}
```

3. Back in our `MainPage.xaml` we can add another `Button` that will call this new method:


```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage 
             Title="{Binding Title}">

    <d:ContentPage.BindingContext>
        <viewmodel:MonkeysViewModel/>
    </d:ContentPage.BindingContext>


    <Grid RowSpacing="0" ColumnSpacing="5">
        <!--... -->
         <ListView ItemsSource="{Binding Monkeys}"
                  CachingStrategy="RecycleElement"
                  HasUnevenRows="True"
                  SeparatorVisibility="None"
                  Grid.ColumnSpan="2">
            <!--... -->
        </ListView>
        <Button Text="Search" 
                Command="{Binding GetMonkeysCommand}"
                IsEnabled="{Binding IsNotBusy}"
                Grid.Row="1"
                Grid.Column="0"
                Style="{StaticResource ButtonOutline}"
                Margin="8"/>

        <!-- Add this -->
        <Button Text="Find Closest" 
                Command="{Binding GetClosestCommand}"
                IsEnabled="{Binding IsNotBusy}"
                Grid.Row="1"
                Grid.Column="1"
                Style="{StaticResource ButtonOutline}"
                Margin="8"/>

        <ActivityIndicator IsVisible="{Binding IsBusy}"
                           IsRunning="{Binding IsBusy}"
                           HorizontalOptions="FillAndExpand"
                           VerticalOptions="CenterAndExpand"
                           Grid.RowSpan="2"
                           Grid.ColumnSpan="2"/>
    </Grid>
</ContentPage>
```

Re-run the app to see geolocation in action!

### Opening Maps

Xamarin.Essentials provides over 60 native features from a single API and opening the default map application is built in!

1. Open the `MonkeyDetailsViewModel.cs` file

2. Now we can create an `OpenMapCommand` and method `OpenMapAsync` to open the map to the monkey's location:

```csharp
public class MonkeyDetailsViewModel : BaseViewModel
{
    public Command OpenMapCommand { get; }
    
    public MonkeyDetailsViewModel()
    {
        OpenMapCommand = new Command(async () => await OpenMapAsync()); 
    }

    //..

    async Task OpenMapAsync()
    {
        try
        {
            await Map.OpenAsync(Monkey.Latitude, Monkey.Longitude);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to launch maps: {ex.Message}");
            await Application.Current.MainPage.DisplayAlert("Error, no Maps app!", ex.Message, "OK");
        }
    }
}
```

### Update DetailsPage.xaml UI


Under the monkey's name, let's add a button that calls the `OpenMapCommand`.

```xml
<Label Text="{Binding Monkey.Name}" HorizontalOptions="Center" FontSize="Medium" FontAttributes="Bold"/>

<!-- Add this -->
<Button Text="Show on Map" 
        Command="{Binding OpenMapCommand}"
        HorizontalOptions="Center" 
        WidthRequest="200" 
        Margin="8"
        Style="{StaticResource ButtonOutline}"/>

<Label Text="{Binding Monkey.Details}" Margin="10"/>
```

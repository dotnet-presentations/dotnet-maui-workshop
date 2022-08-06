## Accessing Platform Features

In Part 4, we will use .NET MAUI to find the closest monkey to us and also open a map with the Monkeys location.

This module is also available in [Chinese (Simplified)](README.zh-cn.md) & [Chinese (Traditional)](README.zh-tw.md).

### Check for internet

We can easily check to see if our user is connected to the internet with the built in `IConnectivity` of .NET MAUI

1. First, let's get access to the `IConnectivity` found inside of .NET MAUI. Let's inject `IConnectivity` into our `MonkeysViewModel` constructor:

    ```csharp
    IConnectivity connectivity;
    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
        this.connectivity = connectivity;
    }
    ```

1. Register the `Connectivity.Current` in our `MauiProgram.cs`.


1. While we are here let's add both `IGeolocation` and `IMap`, add the code:

    ```csharp
    builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
    builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
    builder.Services.AddSingleton<IMap>(Map.Default);
    ```

1. Now, let's check for internet inside of the `GetMonkeysAsync` method and display an alert if offline.

    ```csharp
    if (connectivity.NetworkAccess != NetworkAccess.Internet)
    {
        await Shell.Current.DisplayAlert("No connectivity!",
            $"Please check internet and try again.", "OK");
        return;
    }
    ```

    Run the app on your emulator and toggle on and off airplane mode to check your implementation.


### Find Closest Monkey!

We can add more functionality to this page using the GPS of the device since each monkey has a latitude and longitude associated with it.

1. First, let's get access to the `IGeolocator` found inside of .NET MAUI. Let's inject `IGeolocator` into our `MonkeysViewModel` constructor:

    ```csharp
    IConnectivity connectivity;
    IGeolocation geolocation;
    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;
        this.connectivity = connectivity;
        this.geolocation = geolocation;
    }
    ```


1. In our `MonkeysViewModel.cs`, let's create another method called `GetClosestMonkey`:

    ```csharp
    [RelayCommand]
    async Task GetClosestMonkey()
    {

    }
    ```

1. We can then fill it in by using .NET MAUI  to query for our location and helpers that find the closest monkey to us:

    ```csharp
    [RelayCommand]
    async Task GetClosestMonkey()
    {
        if (IsBusy || Monkeys.Count == 0)
            return;

        try
        {
            // Get cached location, else get real location.
            var location = await geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                location = await geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                });
            }

            // Find closest monkey to us
            var first = Monkeys.OrderBy(m => location.CalculateDistance(
                new Location(m.Latitude, m.Longitude), DistanceUnits.Miles))
                .FirstOrDefault();

            await Shell.Current.DisplayAlert("", first.Name + " " +
                first.Location, "OK");

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to query location: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }
    ```


1. Back in our `MainPage.xaml` we can add another `Button` that will call this new method:

    Add the following XAML under the Search button.

    ```xml
    <Button Text="Find Closest" 
        Command="{Binding GetClosestMonkeyCommand}"
        IsEnabled="{Binding IsNotBusy}"
        Grid.Row="1"
        Grid.Column="1"
        Style="{StaticResource ButtonOutline}"
        Margin="8"/>
    ```

Re-run the app to see geolocation in action after you load monkeys!

This project is pre-configured with all required permissions and features needed for Geolocation. You can read the documentation to find out more about setup, but here is a quick overview.

1. .NET MAUI is pre-configured in all .NET MAUI applications including handling permissions.
1. Android manifest information was pre-configured in **MonkeyFinder -> Platforms -> Android -> AssemblyInfo.cs**
1. iOS/macOS manifest information was configured in the **info.plist** file for each platform
1. Windows manifest information was configured in the **Package.appxmanifest**

### Opening Maps

.NET MAUI provides over 60 platform features from a single API and opening the default map application is built in!

1. Inject `IMap` into our `MonkeyDetailsViewModel`:

    ```csharp
    IMap map;
    public MonkeyDetailsViewModel(IMap map)
    {
        this.map = map;
    }
    ```

1. Open the `MonkeyDetailsViewModel.cs` file and add a method called `OpenMap` that calls into the `Map` API passing it the monkey's location:

    ```csharp
    [RelayCommand]
    async Task OpenMap()
    {
        try
        {
            await map.OpenAsync(Monkey.Latitude, Monkey.Longitude, new MapLaunchOptions
            {
                Name = Monkey.Name,
                NavigationMode = NavigationMode.None
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to launch maps: {ex.Message}");
            await Shell.Current.DisplayAlert("Error, no Maps app!", ex.Message, "OK");
        }
    }

    ```

### Update DetailsPage.xaml UI


Above the monkey's name, let's add a button that calls the `OpenMapCommand`.

```xml
<Button Text="Show on Map" 
        Command="{Binding OpenMapCommand}"
        HorizontalOptions="Center" 
        WidthRequest="200" 
        Margin="8"
        Style="{StaticResource ButtonOutline}"/>
                
<Label Style="{StaticResource MediumLabel}" Text="{Binding Monkey.Details}" />
```

Run the application, navigate to a monkey, and then press Show on Map to launch the map app on the specific platform.


## iOS Safe Area Layouts

In addition to accessing cross-platform device APIs, .NET MAUI also includes platform specific integrations. If you have been running the Monkey Finder app on an iOS device with a notch, you may have noticed that the buttons on the bottom overlap the bar on the bottom of the device. iOS has the concept of Safe Areas and you must progmatically set this. However, thanks to platform specifics, you can set them directly in the XAML.

1. Open `MainPage.xaml` and add a new namespace for iOS specifics:

    ```xml
    xmlns:ios="clr-namespace:Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;assembly=Microsoft.Maui.Controls"
    ```

1. On the `ContentPage` node, you can now set the following property:

    ```xml
    ios:Page.UseSafeArea="True"
    ```

Re-run the application on an iOS simulator or device and notice the buttons have automatically been shifted up.

Let's move forward to the next module and learn about the Collection View in [Part 5](../Part%205%20-%20CollectionView/README.md)

# Part 3 - "Magic Values"

In this part, we will examine the use of "magic strings" and "magic numbers" (collectively referred to as "magic values") in XAML files. Magic values can make code harder to understand and often mean changes need to be made in more than one place. Because of this, it's wise to avoid them whenever possible, and we'll look at ways of doing this.

The code examples in this part will continue on from the previous part. If you have the solution open, you can carry on from there. Alternatively, open the [solution in the Finish directory from the last part](../Part%202%20-%20Responsibility/Finish/).

## 3.0 - Outline

Look at the XAML files in the project.
You'll see lots of numbers and string values repeated many times.
Are these the same because they represent the same underlying value, or is it a coincidence? How can you be sure? How would someone modifying the code in the future know which should all be changed together and which shouldn't?
What if there's a typo in a string or a number? Would you know? When would you know?

If we need to modify a value and it's used "everywhere," we really only want to have to change that value in a single place. We don't want to have to reason over the entire codebase and try to work out where all the identical changes should be made.

These are just some of the issues with using "magic values" and how they make the code harder to modify.

In this part, we'll look at:

- Defining things once, in a base class.
- Defining numbers once as resources.
- Doing simple math in a XAML file.
- Making the most of implicit styles.
- String resources do more than enable localization.

Yes, this is a lot to look at, but we want to eliminate all the "magic" so we can clearly and easily follow and understand the code.

## 3.1 - Defining things once, in a base class

No one likes doing the same mundane thing more than once. Similarly, you don't want to write the same code multiple times.

The two pages in our app contain duplicate functionality we can encapsulate and simplify. Technically, this isn't a "magic value", but the principles of streamlining it apply in the same way as "magic values".

The commonality we'll address is the background of these two pages. If you look at the two files, you may notice that the background isn't specified on the `ContentPage`. Instead, it's defined on the child/content element of each `Page`. Not only does this make it unclear that the colors apply at the page level, it also makes it less obvious that these are the same thing and could be centralized.

- Let's start by creating a new folder at the root of our app. Call this `Controls`. The name could be anything, but this name is a common convention.

- Inside this new folder, create a new class called `StandardPage.cs`. This name hopefully communicates that this page is used as a standard throughout the app. You could use another name if it makes more sense to you. If you needed to add a page that was very different from the others, you may inherit from this or create something afresh and give it a name that makes it clear where and when it should be used. There's more on this in the next part.
- Have this class inherit from `ContentPage`.
- Add a constructor to the class that sets the `BackgroundColor` based on the App Theme, like this:

```diff
+namespace MonkeyFinder.Controls;
+
+public class StandardPage : ContentPage
+{
+    public StandardPage()
+    {
+        App.Current.Resources.TryGetValue("LightBackground", out object lightColor);
+        App.Current.Resources.TryGetValue("DarkBackground", out object darkColor);
+
+        this.SetAppThemeColor(ContentPage.BackgroundColorProperty, lightColor as Color, darkColor as Color);
+    }
+}
```

Note that we need to use the `TryGetValue` method to access the application resources in C#, rather than accessing them via a string index. This behavior is by design, even though it is different from what you may be familiar with in other platforms that use XAML.

It is also possible to use a XAML file to configure the binding to the App Theme. I've used a C# file here because it means only adding to a single new file and it highlights that it's perfectly acceptable and even expected to mix the use of C# and XAML in your code. It is left as an exercise for you to explore how you might do this with XAML if that's your preference.

**But, wait.** Our `StandardPage` includes strings that need to match what's specified in `Colors.xaml`. This is the classic example of a "magic string". If we had a typo or difference in one of these strings, we'd end up with functionality that wouldn't work as expected, and we might only find out at runtime.

Fortunately, I have a simple solution.

- Add a NuGet package reference to `RapidXaml.CodeGen.Maui`.
- Rebuild the project.
- If you look in the `Resources/Styles` folder, you'll notice the new file `Colors.cs`. (We'll look at the other added file, `Styles.cs`, later.)
- Inside this file, which is based on `Colors.xaml`, you'll find constant values we can reference.
- We can now update `StandardPage.cs` to reference these constants.

```diff
-        App.Current.Resources.TryGetValue("LightBackground", out object lightColor);
-        App.Current.Resources.TryGetValue("DarkBackground", out object darkColor);
+        App.Current.Resources.TryGetValue(AppColors.LightBackground, out object lightColor);
+        App.Current.Resources.TryGetValue(AppColors.DarkBackground, out object darkColor);
```

With these constants now being used, if the keys in `Colors.xaml` were ever changed, we'd get a compile-time error that there was an inconsistency. This is another benefit we get from not using "magic values".

With our new Page class defied and not using any magic strings, we can start using it in other parts of the app.

- In `GlobalUsings.cs`, add a global using declaration for the namespace containing our new class.

```diff
+global using MonkeyFinder.Controls;
```

- Update `MainPage.xaml.cs` so that it inherits from our new class

```diff
-public partial class MainPage : ContentPage
+public partial class MainPage : StandardPage
```

- Update `MainPage.xaml`:
  - So it uses our new class.
  - Set the XMLNamespace and alias so the new class can be found.
  - Remove the BackgroundColor from the Grid.

```diff
-<ContentPage
+<c:StandardPage

...

+    xmlns:c="clr-namespace:MonkeyFinder.Controls"

...

-    <Grid BackgroundColor="{AppThemeBinding Light={StaticResource LightBackground}, Dark={StaticResource DarkBackground}}" RowDefinitions="*,Auto">
+    <Grid RowDefinitions="*,Auto">

...

-</ContentPage>
+</c:StandardPage>
```

- Update `DetailsPage.xaml.cs` so that it inherits from our new class

```diff
-public partial class DetailsPage : ContentPage
+public partial class DetailsPage : StandardPage
```

- Update `DetailsPage.xaml`:
  - So it uses our new class.
  - Set the XMLNamespace and alias so the new class can be found.
  - Remove the BackgroundColor from the ScrollView.

```diff
-<ContentPage
+<c:StandardPage

...

+    xmlns:c="clr-namespace:MonkeyFinder.Controls"

...

-    <ScrollView BackgroundColor="{AppThemeBinding Light={StaticResource LightBackground}, Dark={StaticResource DarkBackground}}">
+    <ScrollView>

...

-</ContentPage>
+</c:StandardPage>
```

We now have a custom`Page` class we'd use for all pages in the app, and that has the BackgroundColor defined in only one place. If we added another page, we'd use this new page unless there was a good reason not to.

> **Note**:  
> Creating a base class isn't the only way approach that would be appropriate here, but I've walked you through this approach as creating a page to use as the standard for an app is common for apps with more than a handful of different pages.

Creating a base class isn't the only way to simplify our XAML. Let's look at removing duplication by redefining some values as resources we can reuse in multiple places.

## 3.2 - Defining numbers once, as resources

Numbers (integers, doubles, etc.) are widely used when defining a UI. If you were using C# to create your UI, you'd quickly spot that you were typing the same values in multiple places and create a variable (or a constant) to store them, give them meaning, and make future changes to those values easier. Just because we're defining the UI in XAML doesn't mean we should avoid what would be considered good practice in any other programming language.

Let's start by creating a place to put the resources we will create.

- In the `Resources\Styles` folder, create a new `ResourceDictionary` called `Sizes.xaml`. (Of course, you could put the new file somewhere else if you prefer.)

We'll use this file to store the constant values for the sizes of things we define in the UI. As with elsewhere in this workshop, we're only defining a few things, but other apps you make will likely be larger and contain much more code. You could put everything in one big file, but as we've looked at previously, this isn't conducive to creating code that's easy to understand and maintain.

In `App.xaml`, add a reference to this new file so that the compiler knows to make the resources available to the rest of the application's code.

```diff
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="Resources/Styles/Colors.xaml" />
+               <ResourceDictionary Source="Resources/Styles/Sizes.xaml" />
                <ResourceDictionary Source="Resources/Styles/Styles.xaml" />
            </ResourceDictionary.MergedDictionaries>
```

> **Note**:  
> Define this before the reference to `Styles.xaml` means that you will be able to reference the "Sizes" in the "Styles" file. We're not doing that here, but you're likely to want to do it in your own apps. If you find that your resources aren't available where you expect, check the order they are loaded.

We're now ready to extract our repeated size values into reusable resources.

Let's start by looking at `MainPage.xaml`.

The `EmptyView` of the `CollectionView` contains a square `Image`. The `HeightRequest` and `WidthRequest` are both the same. Let's create a new resource for this.

In `Sizes.xaml`, add `<x:Double x:Key="LargeSquareImageSize">160</x:Double>`.

We can then update the `Image` to use this new resource.

```diff
    <Image
-       HeightRequest="160"
+       HeightRequest="{StaticResource LargeSquareImageSize}"
        HorizontalOptions="Center"
        Source="nodata.png"
        VerticalOptions="Center"
-       WidthRequest="160" />
+       WidthRequest="{StaticResource LargeSquareImageSize}" />
```

Yes, it looks like we've made the XAML code longer. While that's the case, we've made it easier to modify in the future. When you're working with a large code base with lots of images, and you need to change the size of all your large, square images, this will make your life easier. There are actually other ways of making that situation easier to manage and work with, but we'll get to them in Part 5.

This image is in a `StackLayout` with a `Padding` around it. If you want you can also create a resource for this. However, as this is the only location that this padding is used, I'll leave this to you to implement if you so desire.

Instead, let's look at another `Padding` used in this file.

The `Grid` used for each item in the `CollectionView` has a `Padding` of **10**, as does the `VerticalStackLayout` it contains. There's also a `VerticalStackLayout` on the details page which also uses a `Padding` with the same value. In fact, you could argue that **10** is the "Standard" padding size used throughout this app. Let's create a resource that defines this.

In `Sizes.xaml` create a `Thickness` for the `StandardItemPadding`.

```diff
+    <Thickness x:Key="StandardItemPadding">10</Thickness>
```

We can then use this value in the two pages:

```diff
-    Padding="10"
+    Padding="{StaticResource StandardItemPadding}"
```

Notice that the `VerticalStackLayout` on the DetailsPage also uses a `Spacing` value of **10**. You cannot use the "StandardItemPadding" value here though, as `Spacing` of type `double`, whereas `Padding` is a `Thickness`.

We can create a separate resource for this:

```diff
+    <x:Double x:Key="InternalSpacing">10</x:Double>
```

and used in `DetailsPage.xaml` like this:

```diff
-    Spacing="10">
+    Spacing="{StaticResource InternalSpacing}">
```

We've created a resource that we're only using in one place in this app, but it is common for most designs to have standardization of the spacing around and between related UI elements. Clearly defining these values within the code of an app makes it obvious when something is inconsistent.

> **Note**:  
> Both `StandardItemPadding` and `InternalSpacing` use the same number "10". This duplication of a numeric value may be something you want to avoid. Sadly, XAML does not make it easy to remove this duplication. There are solutions but they are not currently covered in this workshop.

`MainPage.xaml` also includes a smaller image within the `ItemTemplate`. The size of this is **125**, but you will notice that the same value is also used for the `Frame` and one of the `ColumnDefinitions`. Is it a coincidence that this value appears in multiple places?  It's not, but we can make it clearer to someone looking at this code in the future by creating a single resource and using that everywhere.

As this is for a smaller image than the other one, let's call this the "SmallSquareImageSize". You create it in `Sizes.xaml` like this:

```diff
+    <x:Double x:Key="SmallSquareImageSize">125</x:Double>
```

We can now use this in `MainPage.xaml`:

```diff
-       <Frame HeightRequest="125" Style="{StaticResource CardView}">
+       <Frame HeightRequest="{StaticResource SmallSquareImageSize}" Style="{StaticResource CardView}">
            <Frame.GestureRecognizers>
                <TapGestureRecognizer Command="{Binding Source={RelativeSource AncestorType={x:Type viewmodel:MonkeysViewModel}}, x:DataType=viewmodel:MonkeysViewModel, Path=GoToDetailsCommand}" CommandParameter="{Binding .}" />
            </Frame.GestureRecognizers>
-           <Grid Padding="0" ColumnDefinitions="125,*">
+           <Grid Padding="0">
+               <Grid.ColumnDefinitions>
+                   <ColumnDefinition Width="{StaticResource SmallSquareImageSize}" />
+                   <ColumnDefinition Width="*" />
+               </Grid.ColumnDefinitions>
               <Image
                    Aspect="AspectFill"
-                   HeightRequest="125"
+                   HeightRequest="{StaticResource SmallSquareImageSize}"
                    Source="{Binding Image}"
-                   WidthRequest="125" />
+                   WidthRequest="{StaticResource SmallSquareImageSize}" />
```

Notice that we've had to change the format used to define the `ColumnDefinitions` to do this.

Finally, for this section, let's look at the image on the Details Page.

This is the same size as the large image on the Main Page, so we can reuse the resource we created earlier. Note that the image is also inside a Frame that is the same size. This means we can use the resource in 4 places:

```diff
    <Border
-       HeightRequest="172"
+       HeightRequest="{StaticResource LargeSquareImageSize}"
        Stroke="White"
        StrokeShape="RoundRectangle 80"
        StrokeThickness="6"
-       WidthRequest="172">
+       WidthRequest="{StaticResource LargeSquareImageSize}">
        <Image
            Aspect="AspectFill"
-           HeightRequest="160"
+           HeightRequest="{StaticResource LargeSquareImageSize}"
            Source="{Binding Monkey.Image}"
-           WidthRequest="160" />
+           WidthRequest="{StaticResource LargeSquareImageSize}" />
    </Border>
```

The result is that if we ever wished to change the size of the image, we'd only have to change one value, not four.

We've now removed all the repeated values that are semantically the same and replaced them with constant resources we can use in all the places it makes semantic sense to do so.

Having removed many of the repeated numeric values, you can now see some similar values used in similar places.

The Padding of the `HorizontalStackLayout` on `MainPage.xaml` and the topmost `VerticalStackLayout` on `DetailsPage.xaml` is 8, while the other StackLayouts used 10 (which is now the "StandardItemPadding".) Such inconsistency is unusual for such similar parts of a UI. Let's assume this inconsistency wasn't deliberate and use the same value everywhere. If there were an intentional reason for these values being different, using different named resource values would clarify this.

In `MainPage.xaml`, make this change:

```diff
    <HorizontalStackLayout
        Grid.Row="1"
-         Padding="8"
+         Padding="{StaticResource StandardItemPadding}"
          HorizontalOptions="Center"
          Spacing="21">
```

In `DetailsPage.xaml`, make this change:

```diff
-  <VerticalStackLayout Padding="8" BackgroundColor="{StaticResource Primary}">
+  <VerticalStackLayout Padding="{StaticResource StandardItemPadding}" BackgroundColor="{StaticResource Primary}">
```

The `Border` on `DetailsPage.xaml` includes a number (80) in the `StrokeShape` definition that is exactly half the value used for the Height and Width of the border. If the height and width of the border changed, this value would need to be changed. We could extract this to a resource and give it a name or add a comment to help ensure that any future changes are applied correctly, or we could do it another way. We'll look at that next.

## 3.3 - Doing simple math in a XAML file

We currently have two related numbers that we have specified in the code. If one is changed, the other must be changed proportionally as well. Rather than rely on people (other developers) to ensure that the two numbers are updated together, let's have the machine the code is running on calculate one of the numbers. _(Computers are very good at doing math.)_

In `DetailsPage.xaml` we have a large square image with a `Border` around it to crop it to appear as a circle. To do this, we specify the size of the image and the radius of the circle. As the size of the image (height and width as it's a square) is the same as the circle's diameter, we can divide the image size by 2 to get the radius.

Hopefully, that sounds simple enough. However, XAML has no built-in mathematics functions, so we must write our own. Don't fear—it's surprisingly simple.

We'll first change the `StrokeShape` of the Border to use the Tag syntax rather than the attribute version, which implicitly converts a string to a tag.

```diff
  <Border
      HeightRequest="{StaticResource LargeSquareImageSize}"
      Stroke="White"
-     StrokeShape="RoundRectangle 80"
      StrokeThickness="6"
      WidthRequest="{StaticResource LargeSquareImageSize}">
+     <Border.StrokeShape>
+       <RoundRectangle CornerRadius="80" />
+     </Border.StrokeShape>
      <Image
```

Again, this is an example of making the code longer than it was previously, but this is so that it's easier to see what we're doing and why.

With this change, it's clear what the value of "80" in the `StrokeShape` represents.

We now want to replace the value of "80" with something calculated based on the Resource that contains the "LargeSquareImageSize". We do this by creating a **Markup Extension**.

One thing that XAML has that XML does not is "Markup expressions." You'll typically see them as the attributes in XAML files surrounded by curly braces. The "markup expression" is identified by the curly braces. It starts with the name of the "markup extension" and is followed by zero or more other parameters.

> **Important**
> Never forget that the 'X' in XAML stands for **eXtensible**. It was always intended that you'd use more than the built-in capabilities. A "Markup Extension" is only one of the ways that XAML can be extended.

We'll create a new folder to store our markup extensions. Call it `Extensions`.

In this new folder, create a new C# class called `GetRadius.cs`. This simple class will take a `Diameter` and return the `Radius` as a `double`.

This new class inherits from `BindableObject`. This is so that we can add properties that we can bind to. It also implements the `IMarkupExtension<double>` interface to show that it can be used as a markup extension for properties of type `double`. (Which is the type of `CornerRadius`.)

```csharp
public class GetRadius : BindableObject, IMarkupExtension<double>
{
}
```

Now add the `BindableProperty` called `Diameter` and of type `double`.

```csharp
    public double Diameter
    {
        get => (double)GetValue(DiameterProperty);
        set => SetValue(DiameterProperty, value);
    }

    public static readonly BindableProperty DiameterProperty =
        BindableProperty.Create(nameof(Diameter), typeof(double), typeof(GetRadius), defaultValue: 0.0);
```

Now implement the methods required by the `IMarkupExtension<double>` interface declaration:

```csharp
    public double ProvideValue(IServiceProvider serviceProvider)
    {
        return Diameter / 2;
    }

    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return (this as IMarkupExtension<double>).ProvideValue(serviceProvider);
    }
```

Yes, it really is that simple. 
All the class is doing is allowing a Diameter to be specified and then dividing it by 2 when called (via `ProvideValue`) as a markup extension.

We can now update `DetailsPage.xaml` to use our new extension.

Add an XML namespace alias for the namespace containing the markup extension:

```diff
+   xmlns:e="clr-namespace:MonkeyFinder.Extensions"
```

Now replace the hard-coded value (of "80") with the new markup extension and specify that the `Diameter` should be the resource of "LargeSquareImageSize".

```diff
-   <RoundRectangle CornerRadius="80" />
+   <RoundRectangle CornerRadius="{e:GetRadius Diameter={StaticResource LargeSquareImageSize}}" />
```

Run the app, and again, you should see no significant difference in the UI. It's only if you know some of the Padding values have been changed by 2 DIP that you'd see any difference.

Yes, replacing a single hard-coded value with something that calculates that value was arguably a lot of work. Hopefully, though, you can appreciate the potential power this technique has to make the connection between different values more obvious, reduce the need to keep multiple related values in sync manually, and incorporate logic into your XAML files.

While that was adding to the overall amount of code in our files, let's now look at another way of improving our XAML files that will reduce the amount of code.

## 3.4 - Making the most of implicit styles

An "implicit Style" is applied to a type without specifying its name (or key). When defined, we can consider them the "default" style when no other "explicit" style is specified.

In this section, we'll set some new implicit styles to simplify the XAML on each page.

Across the two pages of the app, there are three images. Of these, two set the `Aspect` to the value of `AspectFill`. The other leaves the default value of `AspectFit` but produces the same output when set to `AspectFill` because the correct Height and Width of that image are specified.  
With this knowledge, we can say that if the default value for `Image.Aspect` was changed to `AspectFill`; we wouldn't have to specify this property anywhere in the app. So, let's change the app so it works that way.

In `Styles.xaml`, add a new implicit style.  
It should be for the `TargetType` of `Image`.  
It should not specify an `x:Key`. (This is what makes it an implicit style.)  
It should set the `Aspect` property to the value of `AspectFill`.  

```diff
+    <Style TargetType="Image">
+        <Setter Property="Aspect" Value="AspectFill" />
+    </Style>
```

We can now remove the need to specify this property (`Aspect`) from the other pages.

In `DetailsPage.xaml` remove this line/property.

```diff
    <Image
-       Aspect="AspectFill"
        HeightRequest="{StaticResource LargeSquareImageSize}"
        Source="{Binding Monkey.Image}"
        WidthRequest="{StaticResource LargeSquareImageSize}" />
```

In `MainPage.xaml` remove this line/property.

```diff
    <Image
-       Aspect="AspectFill"
        HeightRequest="{StaticResource SmallSquareImageSize}"
        Source="{Binding Image}"
        WidthRequest="{StaticResource SmallSquareImageSize}" />
```

Remaining in `MainPage.xaml`, we use an `ActivityIndicator` to show when the app is busy.  
This is a good UX practice, but we're using more XAML than is necessary. Specifically, we're configuring it to display in the center of its container. If we didn't do this it would expand to fill all the available space.  
It's rare to position an activity indicator anywhere other than the center of its container. By making this the default behavior in our app for this control, we can simplify usage and future code maintenance.

Principles:

- Apply settings or properties globally if you want them applied to all (or most) instances of that control.
- Only add properties to an instance of an element in XAML when it is specific to that instance and it will contribute helpful information to anyone reading the code in the future.

While this app only has a single instance of this control, it's common for larger apps to want to indicate activity in more than one place, so it is helpful to see how to do this. It also follows on from the lesson of the last part as the `ActivityIndicator` is no longer responsible for its position. We have moved the responsibility for the position of all ActivityIndicators to a central location.

In addition to being positioned in the same way, multiple instances of the `ActivityIndicator` are likely to want to be styled in the same way. For this app, the styling applied is the color and so we shouldn't require that each instance set this.

In `MainPage.xaml` remove the attributes related to position and color.

```diff
    <ActivityIndicator
-       HorizontalOptions="Center"
        IsRunning="{Binding IsBusy}"
        IsVisible="{Binding IsBusy}"
-       VerticalOptions="Center"
-       Color="{StaticResource Primary}" />
```

In `Styles.xaml`, add a new implicit style.  
It should be for the `TargetType` of `ActivityIndicator`.  
It should not specify an `x:Key`. (This is what makes it an implicit style.)  
It should set the `HorizontalOptions` and `VerticalOptions` properties to the value of `Center`.  

```diff
+    <Style TargetType="ActivityIndicator">
+       <Setter Property="HorizontalOptions" Value="Center" />
+       <Setter Property="VerticalOptions" Value="Center" />
+       <Setter Property="Color" Value="{StaticResource Primary}" />
+    </Style>
```

Earlier, we standardized the `Padding` used in all StackLayouts. Rather than specify this on every instance, we can add implicit styles for this. This simplifies the contents of each page that uses a StackLayout

In `Styles.xaml`, add new implicit styles for `HorizontalStackLayout` and `VerticalStackLayout`.  
For both styles, set the `Padding` to the `StandardItemPadding` resource value.

```diff
+    <Style TargetType="HorizontalStackLayout">
+        <Setter Property="Padding" Value="{StaticResource StandardItemPadding}" />
+    </Style>

+    <Style TargetType="VerticalStackLayout">
+        <Setter Property="Padding" Value="{StaticResource StandardItemPadding}" />
+    </Style>
```

Now remove the specifying of `Padding` in the three places it's used. (One in `MainPage.xaml` and two in `DetailsPage.xaml`.)

```diff
-   Padding="{StaticResource StandardItemPadding}"
```

With these changes now complete, the XAML on our two pages is now more consistent, is without duplication, and will be easier to modify in the future.

For the final section in this part, let's look at one of the most common places to use strings within an app: the displayed text.

## 3.5 - Text resources do more than enable localization

There are three common ways to work with localized text inside XAML files:

- As a Static resource.
- Via a MarkupExtension.
- By Binding to the text in the ViewModel.

We'll use a StaticResource for this as we looked at how to create a MarkupExtension earlier, and because binding to text from the ViewModel adds complexity to the ViewModel that we don't need.

> **Note**:  
> You can create an app with an example page showing how to localize text in the other ways by using the [MAUI App Accelerator](https://marketplace.visualstudio.com/items?itemName=MattLaceyLtd.MauiAppAccelerator).

- Create a new folder named "Strings" inside the "Resources" folder.

- Inside the "Strings" folder, add a new "Resources File" named `UiText.resx`.

In addition to creating the `UiText.resx` file, Visual Studio also creates a nested file called `UiText.Designer.cs`.

Open `UiText.resx` and add the following entries:

| Name                     | Value        |
|--------------------------|--------------|
| GetMonkeysButtonContent  | Get Monkeys  |
| FindClosestButtonContent | Find Closest |
| ShowOnMapButtonContent   | Show on Map  |

We have created entries for all the hardcoded strings displayed in the UI.

In both pages, add this XML namespace and alias:

```diff
+    xmlns:str="clr-namespace:MonkeyFinder.Resources.Strings"
```

This alias will make it so we can refer to the resources in XAML. You can do this with the following changes to the code:

In `MainPage.xaml`:

```diff
    <Button
        Command="{Binding GetMonkeysCommand}"
        IsEnabled="{Binding IsNotBusy}"
        Style="{StaticResource ButtonOutline}"
-       Text="Get Monkeys" />
+       Text="{x:Static str:UiText.GetMonkeysButtonContent}" />

    <Button
        Command="{Binding GetClosestMonkeyCommand}"
        IsEnabled="{Binding IsNotBusy}"
        Style="{StaticResource ButtonOutline}"
-       Text="Find Closest" />
+       Text="{x:Static str:UiText.FindClosestButtonContent}" />
```

In `DetailsPage.xaml`:

```diff
    <Button
        Command="{Binding OpenMapCommand}"
        HorizontalOptions="Center"
        Style="{StaticResource ButtonOutline}"
-       Text="Show on Map"
+       Text="{x:Static str:UiText.ShowOnMapButtonContent}"
        WidthRequest="200" />
```

The above works because of the static values created in `UiText.Designer.cs`.

Having these strings defined as resources means we could reuse the resource if we had two places in the code where we needed to show the same text.

> **Note**:  
> In addition to localization, there are more benefits to always putting strings in separate resource files and not putting them directly in the code. I go into details about this in https://www.manning.com/books/usability-matters ;)

We've now removed all the magic values from the XAML pages. These changes have simplified the code, making it easier to read and understand, and will help with future app modifications. As you've guessed by this not being the final part of this workshop, there are yet more improvements we can make.

[Now, head over to Part 4 to learn the impact that different names can have in XAML files](../Part%204%20-%20Naming/README.md)

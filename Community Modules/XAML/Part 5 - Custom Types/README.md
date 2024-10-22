# Part 5 - Custom Types

In this part, we will create and use custom types in our XAML files. These changes will make some of the most dramatic changes to the code we started with but are only possible once we have understood the lessons of the previous parts.

The code examples in this part will continue on from the previous part. If you have the solution open, you can carry on from there. Alternatively, open the [solution in the Finish directory from the last part](../Part%204%20-%20Naming/Finish/).

## 5.0 - Outline

Now that you've reached the final part of this workshop, you may notice that the XAML in the project already looks very different from what we started with. Despite all these differences, there are still many ways that we can improve it.

In this part, we'll look at:

- Creating types based on Styles.
- Composition of multiple types.
- Isolating stand-alone pieces of logic.

Creating UI controls can be a little daunting. Don't worry. We'll start with some code so straightforward and yet transformational that you will wonder why you've never something like it before.

## 5.1 - Creating types based on Styles

Creating reusable Styles is a great way to simplify and standardize our XAML code while also making it easier to maintain. But what if it could do more?

This isn't a trick question, but what does this code mean:

```xml
<Label Style="{StaticResource Heading}" Text="{Binding Name}" />
```

I would describe that code as saying:

1. Create a Label.
2. Make that label look like the "Heading" version of a label.
3. Bind the Text property to the "Name" of the BindingContext. (Show the name on the label.)

One little line, and it's doing three things.  
What if we could reduce that by 50%?

Why couldn't that code read:

```xml
<s:Heading Text="{Binding Name}" />
```

That code says:

1. Create a Heading.
2. Bind the Text property to the "Name" of the BindingContext. (Show the name as the text of the heading.)

Not only is this simpler, but it also provides semantic meaning to the element in the file. I don't need to know that this uses a `Label` control to display the text. I don't need to care.  

Not only does having less code to read make it easier to maintain, but it also makes it easier for someone less familiar with XAML to understand and maintain.  

This approach means that you can use the XAML document to describe the structure of the UI that you want.

You don't need to include everything in one file. Many of the specifics can be moved to their own places (files), where they can be looked at in isolation if needed.  
This matches the way you structure your C# code. You don't put everything in one big file or class. You create specific classes for specific purposes. You give those classes meaningful names. And you use those names directly to make the code easier to read and understand. That's all the above is doing to make the code easier to maintain.

But, what is this magical `s:Heading` element, and where did it come from?

It's just a very small, simple C# class that looks like this:

```csharp
    public class Heading : Label
    {
        public Heading()
        {
            if (App.Current.Resources.TryGetValue("Heading", out object result))
            {
                this.Style = result as Style;
            }
        }
    }
```

Isn't that simple?  
Couldn't you easily write a class like that?

All it does is:

- Create a new type called `Heading` that inherits from `Label`
- When created, it gets the resource called "Heading" and applies it to the control.

But wait, you may think, that's a C# file, and all the UI should be done in XAML. **Nope.** 

There is absolutely no reason not to mix C# and XAML. Your codebase already contains a combination of the two, and combining them can make your code (and your life) much simpler. Not only are some things easier or require less code in one language, but they may even require fewer files.  
Yes, you could create a version of the `Heading` class (control) that uses both a XAML file and a C# one, but why would you when the above works fine?

In fact, you don't need to write the above code at all!

Back in part 3, you installed the NuGet package `RapidXaml.CodeGen.Maui`. This created the `AppColors` class that we used to refer to colors used in `StandardPage.cs` without having to create "magic strings". When you installed that package and built (compiled) the project, it also created a file called `Styles.cs` in the "Resources\Styles" folder.

Open that file now. Inside, you'll see that for every Style with a defined `x:Key`, a type has been automatically created. This means we can use these new classes in our code right now.

Let's make some changes.

In `MainPage.xaml`, start by adding this XML namespace and alias:

```diff
+    xmlns:s="clr-namespace:MonkeyFinder"
```

I'm using the alias "s" as it's shorter than "style", but that's my preference. Use what works best for you when applying something similar in your own code.

Now we can use the new types in place of the old ones.

Make these changes:

```diff
-   <Border HeightRequest="{StaticResource SmallSquareImageSize}" Style="{StaticResource CardView}">
+   <s:CardView HeightRequest="{StaticResource SmallSquareImageSize}">

...

-             <Label Style="{StaticResource Heading}" Text="{Binding Name}" />
+             <s:Heading Text="{Binding Name}" />
-             <Label Style="{StaticResource ListDetails}" Text="{Binding Location}" />
+             <s:ListDetails Text="{Binding Location}" />
            </VerticalStackLayout>
        </Grid>
-   </Border>
+   </s:CardView>

...

-   <Button
+   <s:StandardButton
        Command="{Binding GetMonkeysCommand}"
        IsEnabled="{Binding IsNotBusy}"
-       Style="{StaticResource StandardButton}"
        Text="{x:Static str:UiText.GetMonkeysButtonContent}" />

-   <Button
+   <s:StandardButton
        Command="{Binding GetClosestMonkeyCommand}"
        IsEnabled="{Binding IsNotBusy}"
-       Style="{StaticResource StandardButton}"
        Text="{x:Static str:UiText.FindClosestButtonContent}" />
```

Now make similar changes in `DetailsPage.xaml`:

```diff
+    xmlns:s="clr-namespace:MonkeyFinder"

...

-   <Label
+   <s:Heading
        FontAttributes="Bold"
        HorizontalOptions="Center"
-       Style="{StaticResource Heading}"
        Text="{Binding Monkey.Name}"
        TextColor="White" />

...

-   <Button
+   <s:StandardButton
        Margin="8"
        Command="{Binding OpenMapCommand}"
        HorizontalOptions="Center"
-       Style="{StaticResource StandardButton}"
        Text="{x:Static str:UiText.ShowOnMapButtonContent}"
        WidthRequest="200" />

-   <Label Style="{StaticResource BodyText}" Text="{Binding Monkey.Details}" />
+   <s:BodyText Text="{Binding Monkey.Details}" />
-   <Label Style="{StaticResource AdditionalInformation}" Text="{Binding Monkey.Location, StringFormat='Location: {0}'}" />
+   <s:AdditionalInformation Text="{Binding Monkey.Location, StringFormat='Location: {0}'}" />
-   <Label Style="{StaticResource AdditionalInformation}" Text="{Binding Monkey.Population, StringFormat='Population: {0}'}" />
+   <s:AdditionalInformation Text="{Binding Monkey.Population, StringFormat='Population: {0}'}" />
```

> **Note**:  
> Because the generated Types inherit from the ones the Style was originally applied to, we can still use all the same properties of the original Types.

You've just made some considerable changes, and most of it was because of code you didn't even have to write. However, the creation of the code is less important than the fact you:

- Reduced the size of the code without losing any information.
- Added clarity by removing redundancy and making better use of the names we previously gave to the Styles.
- Created a more semantically meaningful document.

Each of these points contributes to having XAML files that are easier to understand and maintain in the future.

I find making changes like those above, where I can remove a lot of code but still maintain the same functionality, very satisfying. Even more pleasing than removing individual attributes is removing whole elements. That's what we'll get to do next.

## 5.2 - Composition of multiple types

Above, we simplified XAML elements by removing unnecessary attributes. Now, we'll look at simplifying files by removing unnecessary elements.

For this section, we'll focus on `DetailsPage.xaml` and only make changes there.

Looking in the `DetailsPage.xaml` file,  you'll notice that all the code of interest is indented three tabs (or 12 spaces) from the left margin. This indentation is due to the page's number of controls and nesting.  
The root object is a `s:StandardPage`. Inside that is `ScrollView`, which contains the actual content of the page.

It is incredibly common for app pages to have vertically stacked content that needs to support scrolling vertically. These requirements mean that many pages start with the elements `ContentPage` > `ScrollView` > `VerticalStackLayout`.

Why not combine the same three elements into a single control rather than having them at the root of every page?

_Yes, the details page currently uses a `Grid` inside the `ScrollView`, but we'll change that later._

Let's create a "VerticallyScrollingPage" control for this purpose. Its name clearly explains what it will do.

Inside the `Controls` folder, create a new C# class and call the file `VerticallyScrollingPage.cs`.

This new public class should inherit from our `StandardPage` to continue to benefit from the background theming logic we created previously.

```diff
+public class VerticallyScrollingPage : StandardPage
+{
+}
```

We want this new control to encapsulate the other controls, so we'll create them in the constructor, putting the `VerticalStackLayout` inside the `ScrollView`, which we'll set as the `Content` of the page.

```diff
public class VerticallyScrollingPage : StandardPage
{
+    private readonly ScrollView scrollView;
+    private readonly VerticalStackLayout verticalLayout;

+    public VerticallyScrollingPage()
+    {
+        verticalLayout = new VerticalStackLayout { Padding = 0 };
+        scrollView = new ScrollView
+        {
+            Content = verticalLayout
+        };

+        base.Content = scrollView;
+    }
}
```

Note the setting of the `Padding` of the `VerticalStackLayout` being set to "0". This padding value prevents additional space (padding) from being automatically enforced on anything placed inside the `VerticallyScrollingPage`. Some pages, such as this Details Page will want to have content that extends to the very edges. When creating a control that is intended to be highly reusable, it should not apply constraints on the content that will limit where it can be used.

This code handles the basic structure, but we also need to allow for the addition of elements inside our new control. A "Page" normally only supports a single child element, but we want to allow for multiple "Children" the way a `StackLayout` does. We can do this by exposing a property that maps to the `Children` property in the internal `VerticalStackLayout`.

Add this property:

```diff
+    public IList<IView> Children => verticalLayout.Children;
```

The final thing we need to do is specify that the "Children" property is the one to use for the "Content" of the element in the XAML file.  We do this by adding a `ContentProperty` attribute to the class.

```diff
+[ContentProperty(nameof(Children))]
public class VerticallyScrollingPage : StandardPage
{
```

Now we've created our new class, let's use it in the app.  
Update `DetailsPage.xaml` like this to change the root element and remove the `ScrollView` and `Grid`.

```diff
-<c:StandardPage
+<c:VerticallyScrollingPage
    x:Class="MonkeyFinder.DetailsPage"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:c="clr-namespace:MonkeyFinder.Controls"
    xmlns:e="clr-namespace:MonkeyFinder.Extensions"
    xmlns:s="clr-namespace:MonkeyFinder"
    xmlns:str="clr-namespace:MonkeyFinder.Resources.Strings"
    xmlns:viewmodel="clr-namespace:MonkeyFinder.ViewModel"
    Title="{Binding Monkey.Name}"
    x:DataType="viewmodel:MonkeyDetailsViewModel">
-    <ScrollView>
-        <Grid RowDefinitions="Auto,*">

...

            <VerticalStackLayout
                x:Name="BodyContent"
-               Grid.Row="1"
                Spacing="{StaticResource InternalSpacing}">

...

-        </Grid>
-    </ScrollView>
-</c:StandardPage>
+</c:VerticallyScrollingPage>
```

As we've changed the root element of the XAML file and MAUI generates a partial class based on this type, we also need to update the base class in `DetailsPage.xaml.cs`.

```diff
-public partial class DetailsPage : StandardPage
+public partial class DetailsPage : VerticallyScrollingPage
```

Even better, we can choose not to specify the base class at all.  
This is possible because only one part of a `partial class` needs to specify the base class, and the class generated from the XAML file will include the base. By only specifying the base class in one place, we don't need to spend time keeping them in sync if we ever again changed the class used for this page.

```diff
-public partial class DetailsPage : VerticallyScrollingPage
+public partial class DetailsPage
```

If you run the app now, as expected, you'll see that it looks and behaves the same as before.

In this section, you saw how combining multiple elements can simplify the code in each file and create reusable components you can use throughout an app without duplicating code.

We did all this with C#, but in the next section, we'll combine XAML and C# to encapsulate more complex UI logic.

## 5.3 - Isolating stand-alone pieces of logic

When you have a large class or file in any other programming language and a part of that content could be reused on its own, it is common to extract that code into its own file or class. Even if you weren't going to reuse that code in other places, multiple smaller files (and classes) can be easier to work with than a single large one.

The header information on the details page (with the colored background) is a good candidate for encapsulating into its own class (and file) as it is fairly well contained and is a disproportionately large proportion of the file, given the amount of content it contributes to the UI.

As a way of also showing the creation of a simple control that uses XAML content and doesn't do everything with C#, we'll move the current XAML code into a ContentView.

Add a XAML-based "ContentView" to the `Controls` folder by selecting "Add" > "New Item..." > ".NET MAUI ContentView (XAML)". Call the new item `DetailPageHeader`.

In the "code-behind" file (`DetailPageHeader.xaml.cs`), we need to add bindable properties for the `Title` and `ImageSource`. Both can be of type `string` as the `ImageSource` will be used in an `Image` which can automatically convert from a string.

```csharp
public partial class DetailPageHeader : ContentView
{
    public DetailPageHeader()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(DetailPageHeader), string.Empty);
    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSource), typeof(string), typeof(DetailPageHeader), null);

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string ImageSource
    {
        get => (string)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }
}
```

Remove the `VerticalStackLayout`, which is the "HeaderSection," from `DetailsPage.xaml` and paste it into `DetailPageHeader.xaml`.

We also need to give the `ContentView` a name ("this") and set the `BindingContext` of the `VerticalStackLayout` reference to the class as a whole. This is a simple way to use bindings within a ContentView

We also need to change the bindings of the `Image` and the `Heading` to use the `ImageSource` and `Title` properties we created on the class, rather than properties of the `Monkey` exposed by the `MonkeyDetailsViewModel`.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentView
    x:Class="MonkeyFinder.Controls.DetailPageHeader"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:e="clr-namespace:MonkeyFinder.Extensions"
    xmlns:s="clr-namespace:MonkeyFinder"
+   x:Name="this">
    <VerticalStackLayout
        BackgroundColor="{StaticResource Primary}"
+       BindingContext="{x:Reference this}">
        <Border
            HeightRequest="{StaticResource LargeSquareImageSize}"
            Stroke="White"
            StrokeThickness="6"
            WidthRequest="{StaticResource LargeSquareImageSize}">
            <Border.StrokeShape>
                <RoundRectangle CornerRadius="{e:GetRadius Diameter={StaticResource LargeSquareImageSize}}" />
            </Border.StrokeShape>
            <Image
                HeightRequest="{StaticResource LargeSquareImageSize}"
-               Source="{Binding Monkey.Image}"
+               Source="{Binding ImageSource}"
                WidthRequest="{StaticResource LargeSquareImageSize}" />
        </Border>
        <s:Heading
            FontAttributes="Bold"
            HorizontalOptions="Center"
-           Text="{Binding Monkey.Name}"
+           Text="{Binding Title}"
            TextColor="White" />
    </VerticalStackLayout>
</ContentView>
```

In `DetailsPage.xaml`, we can now add an instance of the new `DetailsPageHeader` control.

```diff
-    <VerticalStackLayout x:Name="HeaderSection" BackgroundColor="{StaticResource Primary}">
-        <Border
-            HeightRequest="{StaticResource LargeSquareImageSize}"
-            Stroke="White"
-            StrokeThickness="6"
-            WidthRequest="{StaticResource LargeSquareImageSize}">
-            <Border.StrokeShape>
-                <RoundRectangle CornerRadius="{e:GetRadius Diameter={StaticResource LargeSquareImageSize}}" />
-            </Border.StrokeShape>
-            <Image
-                HeightRequest="{StaticResource LargeSquareImageSize}"
-                Source="{Binding Monkey.Image}"
-                WidthRequest="{StaticResource LargeSquareImageSize}" />
-        </Border>
-        <s:Heading
-            FontAttributes="Bold"
-            HorizontalOptions="Center"
-            Text="{Binding Monkey.Name}"
-            TextColor="White" />
-    </VerticalStackLayout>

+    <c:DetailPageHeader Title="{Binding Monkey.Name}" ImageSource="{Binding Monkey.Image}" />
```

We've now reduced the size of `DetailsPage.xaml` by 19 lines. This reduction in file size was done without making the Details page any harder to understand. We've simply moved the details of the complexity of the "header"  into a separate file.

Look at `MainPage.xaml` with the above thoughts still fresh in your mind. It should hopefully be apparent that the `s:CardView` type is a strong candidate for being replaced with a single type that combines all the functionality and elements that are in the `Border` it represents. I'll leave that as a separate exercise if you wish to try applying what you've learned above.

We've now finished refactoring the app by adding custom types.  

You've seen how creating custom types based on the Styles being applied makes the code shorter and more meaningful. We then looked to combine types that were always used together and extracted stand-alone pieces of UI logic into separate files. Both of these changes further reduced the size of the code on each page and made the specific intentions of the code easier to understand.

## Conclusion

We've now made substantial changes to the code used to create the app's UI. We changed both pages, but the biggest difference is shown on `DetailsPage.xaml`.

Look back at [what we started with](../../../Finish/MonkeyFinder/View/DetailsPage.xaml) and compare it to [how we finished](../Part%205%20-%20Custom%20Types/Finish/MonkeyFinder/View/DetailsPage.xaml).

All these changes have been made without changing the functionality and look of the UI.

The goal and purpose of this workshop have not been to show you how to create with XAML but to show that you can achieve the same results with very different XAML.

More important than showing that variety is possible, I hope I've shown that different ways of writing the code make it clearer and, therefore, easier to read and understand. You wouldn't doubt that this statement applies to any other programming language, but sadly, this principle has been overlooked for code written in XAML.

Look again at the before-and-after versions of the file. Not only is the final file much smaller, but its contents are clearer and more meaningful. Imagine for a moment that you're not familiar with this code. Which would you rather have to work with? Which would you rather inherit from another developer? Which would you be more confident about changing without unexpected consequences?

I've shown the two different versions of the XAML from the DetailsPage to hundreds of developers (with varying levels of experience) and asked which version they'd rather work with and have to support, maintain, and modify. With only a few exceptions, people want to work with XAML, which looks like the shorter (final) version. (Those who prefer the "original" version said it was because it was what they're used to and didn't want to change. I hope their reluctance to change is because they are already very productive.)


Of course, I'd love your feedback on this workshop and your experiences and thoughts having completed it.  
More importantly, I hope you take some of what you've learned and apply it to your code bases. I don't expect (or want) you to rush out and rewrite all your code immediately. That's unlikely to be the most productive use of your time. Instead, start incorporating some of these ideas a little at a time. If you're maintaining existing code, look for ways to leave it better than you found it. If you're starting on something new, try defining the different styles you'll need based on the designs and composing pages from types based on those styles.  
Whatever you do, if you choose to make no changes in how you write your XAML and still complain that it's hard to understand and slow to modify, it's unlikely to be XAML's fault.

Now go, write maintainable XAML.

matt.

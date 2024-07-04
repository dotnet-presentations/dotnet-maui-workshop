# Part 2 - Responsibility

In this part, we will look at the [Single Responsibility Principle](https://en.wikipedia.org/wiki/Single-responsibility_principle) and how it can be applied to XAML. You don't need any prior knowledge of this principle to complete this part.

The code examples in this part will continue on from the previous part. If you have the solution open, you can carry on from there. Alternatively, open the [solution in the Finish directory from the last part](../Part%201%20-%20Fundamentals/Finish/).

## 2.0 - Outline

At its simplest, the Single Responsibility Principle states that "A class should have only one reason to change." This principle can be easily applied to an object-oriented language like C#, but it isn't always obvious how it applies to XAML files.

Instead of thinking about classes, we can apply this rule by asserting that **an _element_ should only have one reason to change**. An element in our XAML files represents an instance of a class, so the comparison is clear. However, rather than looking at the reasons to change a class, we'll look at the reasons to change a single class instance.

In this part, we'll look at:

- How this helps make maintainable code.
- Separating your content and layout.

Yes, this part has only two sections, but they're the building blocks of making easy-to-maintain XAML files.

## 2.1 - How this helps make code easier to maintain

If you have previous experience with .NET MAUI (or other platforms that use XAML), you've probably encountered the idea of putting the data and logic for a particular piece of UI in a `ViewModel`. ViewModels are great at separating the logic from the UI, but they don't do a lot to help make it easier to maintain the XAML files.

When we think about "maintaining" code, we normally mean **changing** it. And, before we can change code, we, ideally, need to first **understand** it. So, to make our XAML code easier to maintain, we want it to be easier to understand. Making something easier to understand starts by making it easier to read. Then, once we've read it, we can more easily understand the consequences of the changes we want to make.

At the start of this workshop, I said we wouldn't change the app's functionality. That's still the case, and so we'll have to imagine a theoretical change we might make to the app in the future. This is probably reflective of many apps you'll build. You'll build what you need now but also have ideas of things that are likely to be added in the future. In these situations, it is wise to do what you can to make those future changes easier. There will be times when you don't know what changes might be required, and it's in these instances that applying general good practices for increased maintainability is even more important.

As an example, let's imagine we want to add a third button to the bottom of the app's main page, and we need to put it between the two existing buttons.

Look at all the things we'd need to do to add this new button:

- First, we need to look at the `Grid` that contains all the elements on the page. This defines the columns that are used to separate the buttons. As we're adding another button, we'll need another column.
- The `RefreshView` has an attached property that specifies that it needs to span all the defined columns. As we've added another column, this must be updated to reflect this.
- The `ActivityIndicator` also needs to be updated so it continues to span all the defined columns.
- The existing `Button`, that's used to find the closest monkey must be moved into the newly defined (last) column.
- Finally, after we've done all that, we can add the new button.

Before we made any changes, there were 5 elements on the page. To add one more, we've had to modify 4 of the existing ones. Imagine we were modifying a more complex page that contained many more elements. Having to understand how they all interact and modify many of them to make a single small change or addition is often the reason that it can be hard to modify existing XAML files and make changes without causing unintended consequences.

In terms of responsibility, each element is not only responsible for its own content and how it looks, but it's also responsible for where it is positioned in its container (the Grid). We also have multiple reasons for an element to need to be changed:

- If it needs to be positioned differently.
- If other (sibling) elements are added.
- If other elements need their position changed.

Hopefully, you see that this isn't ideal.

Hopefully, you appreciate that using a Grid might not be the best way to create easily maintainable XAML files.

You're right.

> **Note**:  
> I'm not saying you should never use grids. Grids can be very useful. They can also be slightly faster to process as .NET MAUI measures and lays out the content it is putting on the screen. However, as mentioned in the last part, this performance benefit comes at the cost of maintainability, and so a tradeoff must be made. I'm assuming that you don't write all of your C# code to be the most performant it can be. This certainly isn't the case with the C# created in the previous workshop. Many different features and forms of syntax were used to make the C# code easier to read. It, therefore, seems appropriate to not always use something in the XAML that might be the most performant if it makes the code harder to maintain. If you absolutely do need the UI to be rendered as fast as possible, and you've measured and found that putting everything in a single big grid is the best way to do this, then go ahead. If not, don't.

Grids are a great solution to specific layout challenges, but putting everything in a Grid can make it hard to change existing code. If it's you who's going to be making future changes, you don't want to make your life difficult. Similarly, if it's going to be someone else who will be making future changes to the code you're writing, you don't want their life to be more complicated than it needs to be, either. It's not good for the company creating the code that changes are slow, and it's not good for the people who use the application to have to wait longer for changes, updates, and new features.

Now, let's move on from the theory and see how we might change the existing code so it's easier to modify without having to make changes in many different places. We'll do this by having separate elements for presentation (those that a person will see and interact with) and for positioning those presentation elements.

## 2.3 - Separating content and layout

Separating content elements and those that control where and how they're positioned may sound good in principle, but where do you start? Don't worry; it's actually very simple.

Let's start by looking at the XAML of the Main Page.

### MainPage.xaml

Forget what's there at the moment or about specific controls. We can think of the page as having a main content area (which holds the list and activity indicator) and a row of buttons at the bottom. This is a great place to use a simple Grid. It will expand to fill all the available space (the whole page), and we can divide it into parts that take up the space they need (the row of buttons) and a part that takes up all the remaining space (the list or indicator.)

We can modify the Grid on `MainPage.xaml` accordingly:

- Remove the column definitions, as we don't need these.
- Remove the `ColumnSpacing` (as there is now only one column)
- Remove the `RowSpacing` value as this (zero) is the default and so doesn't need specifying.

```diff
    <Grid
        BackgroundColor="{AppThemeBinding Light={StaticResource LightBackground},
                                          Dark={StaticResource DarkBackground}}"
-        ColumnDefinitions="*,*"
-        ColumnSpacing="5"
-        RowSpacing="0"
        RowDefinitions="*,Auto">
```

This leaves us with two rows. The second (bottom) row is for the buttons. The definition of `Auto` means it will take up only the space it needs to display the buttons. The first (top) row is for our main content.

In the `RefreshView`, we can remove the need to span multiple columns.

```diff
        <RefreshView
-            Grid.ColumnSpan="2"
```

In the `ActivityIndicator`, we can remove the need to span multiple columns.

Additionally, we can also remove the need to span multiple rows (if set to span over the two rows, no one is likely to notice the difference, but spanning both rows is unnecessary, and by assigning the `ActivityIndicator` to the same cell in the `Grid` as the `RefreshView` makes it clearer that they're meant to be in the same place on the screen).

Finally, we also want the indicator to be centred horizontally. When set to `Fill` as the value for `HorizontalOptions` the visible parts of the indicator will be centered and space allocated to stretch it to fill the full width of the container. However, by explicitly setting the value to `Center` we make it clear that this "centering" is what we want and not simply the side-effect of another option that produces the same visible result.

```diff
        <ActivityIndicator
-            Grid.RowSpan="2"
-            Grid.ColumnSpan="2"
-            HorizontalOptions="Fill"
+            HorizontalOptions="Center"
```

Now, we can turn our attention to the buttons at the bottom of the screen.

The first thing we can do is add a new container for all the buttons we want to add (both now and in the future). As we want the buttons to be displayed horizontally, we can use a `HorizontalStackLayout` for this.

- We also need to ensure it is placed in the bottom row of the Grid, so set `Grid.Row="1"`.
- Now that the `Button`s are no longer children of the `Grid`, we can remove the attached properties that specify the `Row` and `Column` they were previously positioned in.
- Previously, the spacing between and around the buttons was specified as part of the `Grid` and each individual `Button`. We can now simplify things by moving all that logic to the element that contains all the buttons.
Rather than putting space (a `Margin`) around each `Button,` we can instead put an equivalent-sized `Padding` around the inside of the `HorizontalStackLayout`.
- This leaves the space between `Button`s. Previously, this was a combination of the `ColumnSpacing` specified on the `Grid` and the `Margin` applied to the sides of the `Button`s. We can simplify this by combining these values into the `Spacing` property of the `HorizontalStackLayout`.
- Let's combine `5` from the `GridSpacing` and `8` from each side of the `Button` into a single value, so (5 + 8 + 8 = 21) `Spacing="21"`.

Now, adding another `Button` would only require it to be placed inside the `HorizontalStackLayout`, and it would be positioned relative to the other `Button`s such that the order in the file would reflect the order in the running app. Spacing between the buttons would also automatically be taken care of.

```diff
+        <HorizontalStackLayout Grid.Row="1" HorizontalOptions="Center" Padding="8" Spacing="21">
            <Button
-                Grid.Row="1"
-                Grid.Column="0"
-                Margin="8"
                Command="{Binding GetMonkeysCommand}"
                IsEnabled="{Binding IsNotBusy}"
                Style="{StaticResource ButtonOutline}"
                Text="Get Monkeys" />

            <Button
-                Grid.Row="1"
-                Grid.Column="1"
-                Margin="8"
                Command="{Binding GetClosestMonkeyCommand}"
                IsEnabled="{Binding IsNotBusy}"
                Style="{StaticResource ButtonOutline}"
                Text="Find Closest" />
+        </HorizontalStackLayout>
```

With these changes, we've now separated the logic (code) that determines where and how elements are positioned (with the `Grid` and the `HorizontalStackLayout`) from the elements that are displayed on the page (the `RefreshView`, `ActivityIndicator`, and the `Button`s.)

In terms of simplifying the amount of code, we've made the following changes:

- Removed 12 attributes from existing elements.
- Added 1 new element.
- Changed 1 attribute of an existing element.

However, on a semantic level, we've:

- Simplified the structure and complexity of the page.
- Grouped similar or related elements together.
- Used elements and values that help clarify the intent of the XAML to make it easier to visualize.
- Removed some of the duplication.

This should make the code easier for someone else to understand in the future and also make it easier for them to modify without unnecessary effort, duplication, or unintended consequences.

With that page changed, we can now focus on the other page in the app.

### DetailsPage.xaml

As with the `MainPage`, the grid on the `DetailsPage` is doing more work than is helpful for making the page easy to maintain. At a high level, the page has two key areas: the colored header at the top and the details below. The current implementation uses two rows of the Grid to display the header and a third row for the rest of the content.

Let's simplify the header to fit in a single row of the `Grid`.

For the header, the current solution uses a `BoxView` stretched over two rows as the background and then puts the `Image` in the first row and the `Label` in the second row. By requiring multiple rows for something that, we could argue, should be a single thing (the header), it makes it harder to modify (and particularly to add more elements) without impacting or needing to change other elements on the page.

Let's start modifying this page by removing the unnecessary `RowDefinition`.

```diff
-        <Grid RowDefinitions="Auto,Auto,*">
+        <Grid RowDefinitions="Auto,*">
```

We must also update the `VerticalStackLayout` that holds the body content of the page (displaying the details of the Monkey) so that it is in the correct row (**1**).

```diff
        <VerticalStackLayout
-            Grid.Row="2"
+            Grid.Row="1"
            Padding="10"
            Spacing="10">
```

We can now return to thinking about the elements that make up the page header.

In the previous version, the `BoxView` was separate from the elements within it. This separation is unnecessary and can make it harder for someone unfamiliar with the code to see how the different elements are related. We'll replace it with something that can have the `Border` (containing the `Image`) and `Label` as its children. Many controls support multiple children, but as we want them to be stacked vertically, we'll replace it with a `VerticalStackLayout`.

Yes, this now means we have a `Grid` containing two rows, and each row contains a `VerticalStackLayout`. It's reasonable to ask why we don't replace this with a single `VerticalStackLayout`. There are two reasons.

1. We want to create a clear separation between the part that is the "header" and the part that is the "body".
2. More importantly, we want to apply different styling and layout to the different parts.

- We want the "header" to have a colored background and to go all the way to the edges of the page.  
- We want the "body" to have the default page background and not go all the way to the edges of the page.

We can now change the `BoxView` to a `VerticalStackLayout`. The `BoxView` (now `VerticalStackLayout`) was "Empty" (or "self-contained") but now must have a separate closing element. This means we need to change the element so that it is not empty (remove the `/` before the `>`.)

```diff
-        VerticalOptions="Fill" />
+        VerticalOptions="Fill" >
```

Then, add the closing element after the `Label` and before the other `VerticalStackLayout`.

```diff
            TextColor="White" />
+        </VerticalStackLayout>
        <VerticalStackLayout
```

There's still more we can do to simplify the code.

If you look at the `Border` and the `Label` within our new `VerticalStackLayout` you'll see that they both have a `Margin` specified. The `Border` places the `Margin` above it, while the `Label` places the `Margin` below it. Both of these `Margin`s are instances of controls influencing their "container". Ideally, as far as possible, we want displayed elements to be separate from those that control how others are positioned.

So, rather than use a `Margin` to put additional [white] space around the elements in the header, we can add `Padding` to the inside of the `VerticalStackLayout`.

```diff
    <VerticalStackLayout
+        Padding="8"
        BackgroundColor="{StaticResource Primary}"
```

Then remove the `Margin` from the `Border`.

```diff
-        Margin="0,8,0,0"
```

And, remove the `Margin` from the `Label`.

```diff
-        Margin="0,0,0,8"
```

We can now add, remove, or reorder elements within the "header" without needing to do anything to ensure there is the desired space at the top and bottom of the header.

There are further simplifications we can make to the `VerticalStackLayout`, which is the container for the "header".

- Remove `Grid.RowSpan="2"` as it now only occupies a single row.
- Remove `VerticalOptions="Fill` as the `VerticalStackLayout` will take as much space as it needs within the defined row, and setting this property now has no effect here.
- Remove the `HorizontalOptions` attribute as it is the default behavior of the `VerticalStackLayout` in this position.
- Leave the `BackgroundColor` attribute as we still want this color to show.

We can also simplify the definition of the `Border`:

- We can remove the `VerticalOptions` attribute as the `VerticalStackLayout` it is now within will not allocate it more vertical space than it needs, so there is no need to center it.
- We can remove the `HorizontalOptions` attribute as this is the default option for a `Border`.

Within the `Image` there are currently more properties specified than necessary and that can be removed.

Because all the images that might be loaded are square (or almost all--one of them is 422x456 or 1:1.0806), we don't need to account for moving the image to be in the center of the circle. For this reason, we can remove the specification of `HorizontalOptions` and `VerticalOptions`.

```diff
        <Image
            Aspect="AspectFill"
            HeightRequest="160"
-            HorizontalOptions="Center"
            Source="{Binding Monkey.Image}"
-            VerticalOptions="Center"
            WidthRequest="160" />
```

> **Note**:  
> When loading images that are of unknown aspect ratios, you must carefully consider how they will be aligned and/or stretched into the space you have within the app. In an ideal world, you'd always be guaranteed images of known dimensions. Sadly, in your apps, you will be unlikely always to be guaranteed this. There's more on the considerations when working with images of different sizes in chapter 8 of https://www.manning.com/books/usability-matters ;)

There's an improvement we can make to the `Label` too:

- Remove the setting of the `Grid.Row` as the element is no longer directly within a `Grid` and so this will have no effect.
- We cannot remove the `HorizontalOptions` attribute as we did for the `Border` as the default value for this property in a `Label` is for it to be at the `Start` (which is the **Left** in left-to-right language configurations.)

> **Note**:  
> By having to set the `HorizontalOptions` attribute for the `Label` we are forced to break one of the principles of this part of the workshop as the element is specifying its position within its container. Sadly there are situations where this is unavoidable without wrapping the element in another container. When it's a choice between adding an attribute and adding another element (and another layer of nesting), I prefer adding an attribute as it reduces the total amount of code needed.

In summary, the changes we've made to the "Header" look like this:

```diff
-    <BoxView
+    <VerticalStackLayout
+        Padding="8"
-        Grid.RowSpan="2"
-        BackgroundColor="{StaticResource Primary}"
+        BackgroundColor="{StaticResource Primary}">
-        HorizontalOptions="Fill"
-        VerticalOptions="Fill" />
    <Border
-        Margin="0,8,0,0"
        HeightRequest="160"
-        HorizontalOptions="Center"
        Stroke="White"
        StrokeShape="RoundRectangle 80"
        StrokeThickness="6"
-        VerticalOptions="Center"
        WidthRequest="160">
        <Image
            Aspect="AspectFill"
            HeightRequest="160"
-            HorizontalOptions="Center"
            Source="{Binding Monkey.Image}"
-            VerticalOptions="Center"
            WidthRequest="160" />
    </Border>
    <Label
-        Grid.Row="1"
-        Margin="0,0,0,8"
        FontAttributes="Bold"
        HorizontalOptions="Center"
        Style="{StaticResource LargeLabel}"
        Text="{Binding Monkey.Name}"
        TextColor="White" />
+    </VerticalStackLayout>
```

These changes amount to the following:

- Removed 10 attributes from existing elements.
- Replaced 1 element with another and made it a container for the others.
- Added 1 attribute (to the replaced element).

More importantly, we've:

- Encapsulated logic.
- Simplified the code.

## 2.4 - Summary

This whole part has been about making the XAML simpler for those (possibly including ourselves) who may have to try and understand and modify the code in the future.

Try to forget everything you know about this code and imagine you're coming to it without having seen it before or in a long time. In trying to understand a piece of code so that you can change it, wouldn't you rather that code was shorter rather than longer? And, wouldn't you rather that the code didn't have unnecessary internal connections and dependencies that aren't immediately obvious? Of course, you would, and that's what we've created.

As with the last part, if you run the app now, you'll see no behavioral difference with the app. There is a small visual difference in the size of the buttons on the MainPage. Unless you were running the app on Windows, you probably wouldn't have noticed the change in button size but they're now no longer massive when running on a desktop.

 At this point, the benefits of the changes you've made might not be obvious. After all, a large part of the reason for them is to for when future changes are needed. There are more immediate benefits to the changes made in this part, and they will become clearer as we continue and build on what we've just done.

[Now, head over to Part 3, and we'll look at the use of "magic values"](../Part%203%20-%20Magic%20Values/README.md)!

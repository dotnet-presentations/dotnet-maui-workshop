# Part 4 - Naming

In this part, we will look at the names we give to the elements and resources we use in XAML files. We'll look at how to use names so they help with code maintainability and make it easier to read and understand. We'll then extend these principles to see how to create semantically meaningful files.

The code examples in this part will continue on from the previous part. If you have the solution open, you can carry on from there. Alternatively, open the [solution in the Finish directory from the last part](../Part%203%20-%20Magic%20Values/Finish/).

## 4.0 - Outline

Naming is a famously tricky part of software development. Naming is difficult because it is so crucial to explaining the code and why it is there. Understanding code is critical to its easy maintenance, so we can't overlook it.

In this part, we'll look at:

- What makes a good name.
- Naming Resources and Styles.
- Naming Elements and Controls.

What makes a good name is often subjective and the names used in your UI have different requirements to other parts of a codebase. Let's get into it.

## 4.1 - What makes a good name

General advice for naming in software is to use a specific, descriptive name for what something does. While this is good advice for objects, it doesn't help produce maintainable XAML.

Consider one of the names we use in the code, the Style `ButtonOutline`.  
At first glance, it may seem like a good name. It applies to a `Button` control, so "Button" as the first part of the name makes sense. But what about the "Outline" part of the name?  
You may argue that as it creates a button with an outline drawn around it when used, it's a good name. As a description of what the code currently does, it does meet that purpose, but that's not what makes a good name for something in the UI.

A good name for something in the UI tells us:

- Where to use it.
- When to use it.
- Is semantically meaningful.

Again, you might think you should use this style whenever you need a button drawn with an outline, so it is a good name.  
But what happens when the app's design is updated?

Imagine, at some point in the future, the app must be updated to reflect a new design. This update could be as part of a "UI refresh", a change in corporate branding, or something else. Now, instead of a button with an outline, the desired new style is to have a solid color behind the text on the button.

If we keep the same name for the style, it would be very odd and confusing to anyone looking at the code in the future.  
"It says 'ButtonOutline', but it's a solid color without an outline. - What's going on?"  
Confusing code is not easy to maintain or change with confidence.

We could create a new Style for the new requirement and use that instead. But, doing this would lead to changes throughout the code base. One of the benefits of defining something once and referencing it in multiple places is that it should make maintenance and modification of all instances easier. Would you consider something easy to maintain if you have to change every reference to it when you change that thing?

No, a better name for this would be "StandardButton".  
In our app, we want all buttons to look like this "as standard". If we have a reason not to apply this styling as an implicit Style on all buttons, a name that includes something like "standard" as part of the name makes it clear that this is not a special or different button. It looks the way buttons are supposed to look in the app.

> **Note**:  
> Be careful of using "Default" when you mean "Standard" or something that is the ordinary version of something. "Default buttons" are typically the ones you expect or want the user to select from several options. If a page/screen/prompt/dialog included three "DefaultButton"s, it could be confusing. If it included two "StandardButton"s and one "DefaultButton" a person looking at the code could hopefully understand more of the intent and expected behavior of the app.

Having many of the same elements in an XAML file can cause similar issues. If an XAML file contains many nested `Grid` or `StackLayout` controls, it can be hard to understand their purpose. Giving these controls names to clarify their purpose can remove uncertainty when changing the code.

The above points for what makes a good name should serve as guidelines for naming items in the UI.

With these guidelines defined, we can now start applying them to the code.

## 4.2 - Naming Resources and Styles

We'll first look at improving the names of the Styles used in the code. We'll start by updating the "ButtonOutline" mentioned above and then look at the different Styes used for Labels.

Modify the style's name (in `Styles.xaml`) to use the name we discussed above.

```diff
-    <Style x:Key="ButtonOutline" TargetType="Button">
+    <Style x:Key="StandardButton" TargetType="Button">
```

I trust you can update each reference to this style and don't need to show it.  There are two references on `MainPage.xaml` and one on `DetailsPage.xaml`.

That was the easy part. Now, let's look at the Styles used for `Label`s. There are currently four of them:

- LargeLabel
- MediumLabel
- SmallLabel
- MicroLabel

From these names, you can assume that these Styles refer to the size of the text. Sadly, they do nothing else and don't meet any of the requirements of good naming identified above.

By looking at how the `LargeLabel` is used in the code, we see that it indicates a "Title" or a "Heading" of an item in a list or at the top of the details page.

Change the name of the Style to `Heading`.

```diff
    <Style
-       x:Key="LargeLabel"
+       x:Key="Heading"
```

Update the use of this Style on each page.

The name "Heading" is more meaningful and communicates to anyone looking at the code why that element is there. If the way headings look is changed in the future, this element will still represent a "heading". You only need to alter the styling in one place, and all instances throughout the app will be updated.  
Imagine we wanted to visually distinguish "Headings" not by size but by using a different font family and weight. The name "LargeLabel" would no longer be appropriate, but "Heading" will still be.

Using a suffix or prefix of the Type is unnecessary as part of the name. "HeadingLabel" or "LabelHeading" add no useful additional information when compared to "Heading", so the shorter version is preferred. If you have multiple similar names, and it becomes necessary to distinguish them by type name, then this can be done. Note that the autocomplete (intellisense) within Visual Studio will filter resources by TargetType when you use them. This should remove the argument for including the type in the name to help ensure it is used correctly. A good name will also help avoid accidental misuse. In the next part, we'll also look at another related way that can help ensure you don't use the wrong resource with a type.

It's now possible to look at the code and know that "that's a heading, which makes sense given the content and position." There's no need to wonder why something is styled as being a "LargeLabel" and question if that is appropriate. 

The `MediumLabel` is more complicated. It shows two semantically different types of information. On the main page, it shows the location of each monkey in the list. On the details page, the Style is used for the text that is the "main" or "body" of the content.

Rather than using a single style for different reasons just because they are currently rendered to look the same, let's rename the `MediumLabel` to `ListDetails` and add another `Style` with the name `BodyText` and other details matching those of' ListDetails'.

```diff
    <Style
-       x:Key="MediumLabel"
+       x:Key="ListDetails"
        BasedOn="{StaticResource BaseLabel}"
        TargetType="Label">
        <Setter Property="FontSize" Value="16" />
    </Style>

+   <Style
+       x:Key="BodyText"
+       BasedOn="{StaticResource BaseLabel}"
+       TargetType="Label">
+       <Setter Property="FontSize" Value="16" />
+   </Style>
```

We can now update the places where "MediumLabel" was used.

In `MainPage.xaml`:

```diff
-    <Label Style="{StaticResource MediumLabel}" Text="{Binding Location}" />
+    <Label Style="{StaticResource ListDetails}" Text="{Binding Location}" />
```

Semantically, each list item now contains a "Heading" followed by "ListDetails". The intent of the labels within each item template should now be more apparent to anyone looking at the code.

In `DetailsPage.xaml`:

```diff
-    <Label Style="{StaticResource MediumLabel}" Text="{Binding Monkey.Details}" />
+    <Label Style="{StaticResource BodyText}" Text="{Binding Monkey.Details}" />
```

Also on the Details page are the uses of "SmallLabel". They are used for text that displays additional information about the selected monkey.  
Let's rename the Style to "AdditionalInformation".

```diff
    <Style
-       x:Key="SmallLabel"
+       x:Key="AdditionalInformation"
```

We can then use this style for Labels that display "additional information" in `DetailsPage.xaml`:

```diff
-    <Label Style="{StaticResource SmallLabel}" Text="{Binding Monkey.Location, StringFormat='Location: {0}'}" />
-    <Label Style="{StaticResource SmallLabel}" Text="{Binding Monkey.Population, StringFormat='Population: {0}'}" />
+    <Label Style="{StaticResource AdditionalInformation}" Text="{Binding Monkey.Location, StringFormat='Location: {0}'}" />
+    <Label Style="{StaticResource AdditionalInformation}" Text="{Binding Monkey.Population, StringFormat='Population: {0}'}" />
```

It is now clearer that text on the details page shows some details as the main body of the text and this is followed by two additional pieces of information.

There are no uses of the `MicroLabel` style. Remove it:

```diff
-    <Style
-        x:Key="MicroLabel"
-        BasedOn="{StaticResource BaseLabel}"
-        TargetType="Label">
-        <Setter Property="FontSize" Value="10" />
-    </Style>
```

With these changes, we've made the names of the Styles more meaningful and less likely to need to be changed when the way they look needs to change. Now, we can look at adding more meaningful names to the elements in the XAML and the controls they represent.

## 4.3 - Naming Elements and Controls

The names we give to things help us understand their purpose and capabilities. However, adding names to things has two potentially negative consequences:

1. It means there is more text on the screen to read and process.
2. It has a very small potential impact on the time taken to compile and run the code.

We can offset the first issue by only adding names where having the name provides more potential value than the cost of reading it.

The impact on performance is only listed as "potential" because it is so tiny you will have trouble measuring it. If you are trying to deal with performance issues of such sizes, you will have to be measuring them very carefully. Unless you have to optimize your code for the absolute highest levels of performance, there are many other things you can optimize in your code that will have a bigger effect than removing names in this way.

For most applications, ease of maintenance is more important than tiny micro-optimizations. That is why the primary goal of this workshop has been to help you improve the maintainability of your XAML files.

The above consequences can be easily overcome by only adding names to things where they help clarify the code for future readers.

Looking at `MainPage.xaml`, we see it contains two main controls. The first contains the list of monkeys. The second is a `HorizontalStackLayout`. If we give this element a name, its purpose should become immediately apparent to anyone looking at the code. As this control hosts the buttons at the bottom of the screen, we'll call it `RowOfButtons`.

```diff
    <HorizontalStackLayout
+       x:Name="RowOfButtons"
        Grid.Row="1"
        HorizontalOptions="Center"
        Spacing="21">
```

This name does two things. First, it indicates how the control will be displayed in the running app, and second, it indicates its intended purpose.  
What do you think should go inside such an element?  
Would you question if something other than a `Button` was put inside it? I hope so. Adding something other than a `Button` may be appropriate but is probably unexpected and is likely to indicate the control is being used in a way beyond the author's original intention.

Adding names in this way has the additional benefit of documenting the code.

Switching to look at `DetailsPage.xaml`, we see that the page's content is composed of two sections. These sections are the colored area containing the image at the top of the page and the text underneath. These two sections are each defined in `VerticalStackLayout`s. Giving each one a name will help to indicate their purpose, especially as having vertical stack layouts positioned one above the other is often unnecessary and could suggest an inappropriate use of controls.

```diff
-   <VerticalStackLayout BackgroundColor="{StaticResource Primary}">
+   <VerticalStackLayout x:Name="HeaderSection" BackgroundColor="{StaticResource Primary}">

...

    </VerticalStackLayout>

-   <VerticalStackLayout Grid.Row="1" Spacing="{StaticResource InternalSpacing}">
+   <VerticalStackLayout
+       x:Name="BodyContent"
+       Grid.Row="1"
+       Spacing="{StaticResource InternalSpacing}">
```

Again, this is a small change to the code, but it will help anyone reading it in the future and demonstrate our intent to make it easier for them.

In the next part of this workshop, we'll look at the names of created types but don't overlook the potential benefit of doing something as simple as assigning a name to a control to indicate its purpose.

The level of focus we've spent on naming may seem unnecessary or over-the-top for such simple code, but the reason for spending time on ensuring names are well chosen is that it makes understanding code easier. It's unlikely you'll ever spend as much time thinking about a simple app as you have with the MonkeyFinder, but as you work on larger files and code bases, the benefit that good names bring will soon become evident to you.

In this part, we covered what makes a good name for something in XAML and updated the code to reflect this. There's more that we can do with these names than specify them. We'll see that next.

[Now, head over to Part 5 and we'll see how we can use custom types](../Part%205%20-%20Custom%20Types/README.md)

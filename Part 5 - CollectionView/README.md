## Adding Pull-to-Refresh

The Xamarin.Forms `ListView` has native support for pull-to-refresh, however a `RefreshView` was been added in Xamarin.Forms 4.3 that enables developers to add pull-to-refresh to other controls such as ScrollView & CollectionView. 

Let's add the new `RefreshView` to add pull-to-refresh to our `CollectionView`.

Update the `CollectionView` logic by wrapping it with a `RefreshView` from:

```xml
<CollectionView
    Grid.ColumnSpan="2"
    ItemsSource="{Binding Monkeys}"
    SelectionChanged="CollectionView_SelectionChanged"
    SelectionMode="Single">
    <!-- Template -->
</CollectionView>
```

to:

```xml
<RefreshView
    Grid.ColumnSpan="2"
    Command="{Binding GetMonkeysCommand}"
    IsRefreshing="{Binding IsBusy, Mode=OneWay}">
    <CollectionView
        ItemsSource="{Binding Monkeys}"
        SelectionChanged="CollectionView_SelectionChanged"
        SelectionMode="Single">
        <!-- Template -->
    </CollectionView>
</RefreshView>
```
Notice that we moved the `Grid.ColumnSpan="2"` to the `RefreshView` since it is the new parent view in the `Grid`.

This will enable pull-to-refresh on iOS, Android, and Windows (on touch screen):

![Android emulator with pull to refresh enabled](../Art/PullToRefresh.PNG)

## Layout

CollectionView will automatically layout items in a vertical stack layout. There are several built in `ItemsLayout` that can be used. Let's explore.

### LinearItemsLayout 

This is the default layout that can display items in either vertical or horizontal orientations. You can set the `ItemsLayout` property to `VerticalList` or `HorizontalList`. 

To access additional properties on the `LinearItemsLayout` we will need to set a sub-property:

```xml
<CollectionView
    ItemsSource="{Binding Monkeys}"
    SelectionChanged="CollectionView_SelectionChanged"
    SelectionMode="Single">
    <!-- Add ItemsLayout -->
    <CollectionView.ItemsLayout>
        <LinearItemsLayout Orientation="Vertical" />
    </CollectionView.ItemsLayout>
    <!-- ItemTemplate -->
</CollectionView>
```

### GridItemsLayout

More interesting is the ability to use `GridItemsLayout` that automatically spaces out items with different spans.  

Let's use the `GridItemsLayout` and change the span to 3 

```xml
<CollectionView
    ItemsSource="{Binding Monkeys}"
    SelectionChanged="CollectionView_SelectionChanged"
    SelectionMode="Single">
    <!-- Change ItemsLayout to GridItemsLayout-->
    <CollectionView.ItemsLayout>
        <GridItemsLayout Orientation="Vertical" Span="3" />
    </CollectionView.ItemsLayout>
    <!-- ItemTemplate -->
</CollectionView>
```

![Monkeys in a grid with 3 columns](../Art/GridItemsLayoutVert.png)

We can change the `Orientation` to `Horizontal` and now our CollectionView will scroll left to right!

![List of monkeys scrolling left to right](../Art/GridItemsLayoutHorizontal.png)

## EmptyView

There are many neat features to `CollectionView` including grouping, header, footers, and the ability to set a view that is displayed when there are no items.

Let's add an image centered in the `EmptyView`:

```xml
<CollectionView
    ItemsSource="{Binding Monkeys}"
    SelectionChanged="CollectionView_SelectionChanged"
    SelectionMode="Single">
    <!-- Add EmptyView -->
    <CollectionView.EmptyView>
        <StackLayout Padding="100">
            <Image
                HorizontalOptions="CenterAndExpand"
                Source="nodata.png"
                VerticalOptions="CenterAndExpand" />
        </StackLayout>
    </CollectionView.EmptyView>
    <!-- ItemTemplate & ItemsLayout-->
</CollectionView>
```


![Emulator without any items in it showing an image in the middle](../Art/EmptyView.png)

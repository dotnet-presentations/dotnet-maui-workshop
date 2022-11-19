## 实验五: 为 CollectionView 添加下拉刷新

.NET MAUI `ListView` 内置了对拉动刷新的支持，但是 `RefreshView` 使开发人员能够将拉动刷新添加到其他控件，例如 `ScrollView` 和`CollectionView`。

让我们添加新的 `RefreshView` 以将 pull-to-refresh 添加到我们的 `CollectionView`。

通过使用 `RefreshView` 包装来更新 `CollectionView` 逻辑

从:

```xml
<CollectionView
    Grid.ColumnSpan="2"
    ItemsSource="{Binding Monkeys}"
    SelectionMode="None">
    <!-- Template -->
</CollectionView>
```

修改为:

```xml
<RefreshView
    Grid.ColumnSpan="2"
    Command="{Binding GetMonkeysCommand}"
    IsRefreshing="{Binding IsRefreshing}">
    <ContentView>
        <CollectionView
            ItemsSource="{Binding Monkeys}"
            SelectionMode="None">
            <!-- Template -->
        </CollectionView>
    </ContentView>
</RefreshView>
```

请注意，我们将 `Grid.ColumnSpan="2"` 移动到了 `RefreshView`，因为它是 `Grid` 中的新父视图。

由于用户可以启动刷新，我们将希望在后面的代码中创建一个新变量来绑定以在完成后停止刷新。

1. 打开 `MonkeysViewModel.cs` 并添加一个新属性：

    ```csharp
    [ObservableProperty]
    bool isRefreshing;
    ```

2. 在 `GetMonkeysAsync` 的 `finally` 中，将 `IsRefreshing` 设置为 `false`：

    ```csharp
    finally
    {
        IsBusy = false;
        IsRefreshing = false;
    }
    ```
这将在 iOS、Android、macOS 和 Windows（在触摸屏上）上启用下拉刷新：

![启用拉刷新的Android模拟器](../Art/PullToRefresh.PNG)

> 重要提示：如果您使用的是 iOS，则当前存在使 UI 看起来不正确的错误。 建议在实验的其余部分在 iOS 上进行测试时删除 RefreshView。

## 布局

`CollectionView` 将自动在垂直堆栈布局中布局项目。 有几个内置的 `ItemsLayout` 可以使用。 让我们探索一下。

### 线性项目布局 - LinearItemsLayout

这是可以在垂直或水平方向显示项目的默认布局。 您可以将 `ItemsLayout` 属性设置为 `VerticalList` 或 `HorizontalList`。

要访问 `LinearItemsLayout` 的其他属性，我们需要设置一个子属性：


```xml
<CollectionView
    ItemsSource="{Binding Monkeys}"
    SelectionMode="None">
    <!-- Add ItemsLayout -->
    <CollectionView.ItemsLayout>
        <LinearItemsLayout Orientation="Vertical" />
    </CollectionView.ItemsLayout>
    <!-- ItemTemplate -->
</CollectionView>
```

### 网格项目布局 - GridItemsLayout

更有趣的是使用 `GridItemsLayout` 自动分隔具有不同跨度的项目的能力。

让我们使用 `GridItemsLayout` 并将跨度更改为 3

```xml
<CollectionView
    ItemsSource="{Binding Monkeys}"
    SelectionMode="None">
    <!-- Change ItemsLayout to GridItemsLayout-->
    <CollectionView.ItemsLayout>
        <GridItemsLayout Orientation="Vertical" Span="3" />
    </CollectionView.ItemsLayout>
    <!-- ItemTemplate -->
</CollectionView>
```

![3 列网格项目布局中的显示](../Art/GridItemsLayoutVert.png)

我们可以将 `Orientation` 更改为 `Horizontal`，现在我们的 CollectionView 将从左到右滚动！

```xml
<CollectionView.ItemsLayout>
    <GridItemsLayout Orientation="Horizontal" Span="5" />
</CollectionView.ItemsLayout>
```

![从左到右滚动的猴子列表](../Art/GridItemsLayoutHorizontal.png)

让我们回到我们原来的单列`CollectionView`:

```xml
<CollectionView.ItemsLayout>
    <LinearItemsLayout Orientation="Vertical" />
</CollectionView.ItemsLayout>
```

## 空视图 - EmptyView

> 重要提示：Android 上目前存在 EmptyView 不会消失的问题。 建议此时在Android上测试时将其移除。

`CollectionView` 有许多简洁的功能，包括分组、页眉、页脚，以及设置在没有项目时显示的视图的能力。

让我们添加一个以 `EmptyView` 为中心的图像：

```xml
<CollectionView
    ItemsSource="{Binding Monkeys}"
    SelectionMode="None">
    <!-- Add EmptyView -->
    <CollectionView.EmptyView>
        <StackLayout Padding="100">
            <Image
                HorizontalOptions="Center"
                Source="nodata.png"
                HeightRequest="160"
                WidthRequest="160"
                VerticalOptions="Center" />
        </StackLayout>
    </CollectionView.EmptyView>
    <!-- ItemTemplate & ItemsLayout-->
</CollectionView>
```

![模拟器中没有任何项目，中间显示图像](../Art/EmptyView.png)

在我们的下一个实验中，我们将了解应用程序主题。 前往 [实验六: 应用程序主题设置](../Part%206%20-%20AppThemes/README.zh-cn.md)

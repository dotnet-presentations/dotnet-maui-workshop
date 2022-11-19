## Hands-on Lab Part 5: 幫 CollectionView 增加下拉更新

.NET MAUI 的 `ListView` 有內建下拉更新的功能，而透過 `RefreshView` 也能替其他的控制元件來進行下拉更新的處理，例如 `ScrollView` 和 `CollectionView` ...等。

透過增加 `RefreshView` 以便將下拉更新 (pull-to-refresh) 的功能增加到 `CollectionView` 上。

下面呈現如何透過 `RefreshView` 的運用來與 `CollectionView` 搭配的設計。 

更改前:

```xml
<CollectionView
    Grid.ColumnSpan="2"
    ItemsSource="{Binding Monkeys}"
    SelectionMode="None">
    <!-- Template -->
</CollectionView>
```

更改後:

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

這邊請注意，已經將 `Grid.ColumnSpan="2"` 的設定變更到了 `RefreshView` 當中，因為變更成這樣的設計時對 `Grid` 來說， `RefreshView` 才是 `Grid` 在配置版面中所處理的控制元件。

由於希望使用者可以手動更新，後面將希望透過程式碼建立一個控制變數，在資料更新完畢後能停止更新。

1. 開啟 `MonkeysViewModel.cs` 並增加一個新的欄位，並透過掛上 `[ObservableProperty]` 變成一個公開的通知屬性：

    ```csharp
    [ObservableProperty]
    bool isRefreshing;
    ```

2. 在原本的 `GetMonkeysAsync()` 方法中，找到 `finally` 區塊，並將 `IsRefreshing` 的值設定為 `false`：

    ```csharp
    finally
    {
        IsBusy = false;
        IsRefreshing = false;
    }
    ```
這樣就可以將 iOS、Android、macOS 和 Windows（若有觸控螢幕控制）上啟動下拉更新的動作：

![啟用下拉更新的 Android 模擬器](../Art/PullToRefresh.PNG)

> 重要提醒：如果您使用的是 iOS 環境，則當前存在著 UI 顯示上不正確的錯誤。建議在此 Hands-on Lab 的其餘部分進行 iOS 測試時先刪除 RefreshView。


## 版面配置

`CollectionView` 將自動在垂直的版面配置中控制項目呈現。有幾個內建的 `ItemsLayout` 可以使用，透過接下來的介紹認識一下。

### 線性的項目配置 - LinearItemsLayout

這是在在垂直或水平方向顯示項目的預設版面配置，可以將 `ItemsLayout` 屬性設定為 `VerticalList` 或 `HorizontalList`。

若要設置 `LinearItemsLayout` 的其他屬性，則需要透過如下的標記碼撰寫，來設定其屬性資料值：

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

### 網格式的項目版面配置 - GridItemsLayout

更有趣的是使用 `GridItemsLayout` 將自動分隔具有不同跨欄列大小的項目能力。

透過使用 `GridItemsLayout` 的設計，並將其跨欄列值設定為 3，如下的 XAML 標記碼:

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

![3 列網格式的項目版面配置的顯示](../Art/GridItemsLayoutVert.png)

在這邊可以將 `Orientation` 的設定值更改為 `Horizontal`，而此 CollectionView 將可以從左到右的橫向捲動！

```xml
<CollectionView.ItemsLayout>
    <GridItemsLayout Orientation="Horizontal" Span="5" />
</CollectionView.ItemsLayout>
```

![從左到右的橫向捲動的猴子列表](../Art/GridItemsLayoutHorizontal.png)

重新回到原來的單列呈現的 `CollectionView` 把剛剛改動的 XAML 標記碼改為如下:

```xml
<CollectionView.ItemsLayout>
    <LinearItemsLayout Orientation="Vertical" />
</CollectionView.ItemsLayout>
```

## 空白檢視 - EmptyView

> 重要提醒：Android 上目前存在 EmptyView 不會消失的問題，建議在 Android 上測試建 EmptyView 時將其設定移除。

在 `CollectionView` 有很多呈現上的檢視設計，這包括分組、標頭、標尾，以及設定在沒有任何項目呈現時的空白檢視。

接著在 `CollectionView` 當中增加一個 `EmptyView` 的設計，並在其中設定一張垂直水平皆置中呈現的圖片：

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

![透過模擬器呈現空白檢視，並在其正中央呈現圖片](../Art/EmptyView.png)

下一個要進行的 Hands-on Lab 的部分，是了解有關 App 的佈景主題設定，繼續前往 [Hands-on Lab Part 6: 設定 App 的佈景主題 ](../Part%206%20-%20AppThemes/README.zh-tw.md)

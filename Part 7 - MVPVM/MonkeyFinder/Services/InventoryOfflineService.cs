#pragma warning disable CA1416


using Adventures.Common.Model;
using MonkeyFinder.Interfaes;

namespace MonkeyFinder.Services;

public class InventoryOfflineService : IInventoryDataService
{
    public string Mode { get; set; } = "OFFLINE";

    List<ListItem> inventoryList;

    public InventoryOfflineService() { }

    public async Task<List<ListItem>> GetInventory()
    {
        if (inventoryList?.Count > 0)
            return inventoryList;

        // Offline
        using var stream = await FileSystem.OpenAppPackageFileAsync("inventorydata.json");
        using var reader = new StreamReader(stream);
        var contents = await reader.ReadToEndAsync();
        inventoryList = JsonSerializer.Deserialize<List<ListItem>>(contents);

        return inventoryList;
    }

    public async Task<T> GetDataAsync<T>(object sender, EventArgs e) where T : ServiceResult
    {
        var retValue = new ServiceResult
        {
            Data = await GetInventory()
        };
        return (T)retValue;
    }
}

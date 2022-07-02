#pragma warning disable CA1416

using System.Net.Http.Json;

namespace Adventures.Inventory.Services.Online;

public class InventoryOnlineService : IInventoryDataService
{
    HttpClient httpClient;

    List<ListItem> inventoryList;

    public string Mode { get; set; } = "ONLINE";

    public InventoryOnlineService()
    {
        this.httpClient = new HttpClient();
    }

    public async Task<List<ListItem>> GetInventory()
    {
        if (isAlwayTrue()) // eliminate unreachable code warnings until we're ready to implement
        {
            // Until we stand up an online service for inventory, we'll reuse
            // the offline service and strip off the "(offline)" text.
            var result =  await new InventoryOfflineService().GetInventory();
            result[0].Name = result[0].Name.Replace("(offline)", "");
            return result;
        }

        if (inventoryList?.Count > 0)
            return inventoryList;

        // Online
        var response = await httpClient.GetAsync("https://www.montemagno.com/monkeys.json");
        if (response.IsSuccessStatusCode)
        {
            inventoryList = await response.Content.ReadFromJsonAsync<List<ListItem>>();
        }
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

    public bool isAlwayTrue()
    {
        return true;
    }
}

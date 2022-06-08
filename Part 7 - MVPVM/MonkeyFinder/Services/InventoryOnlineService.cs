#pragma warning disable CA1416

using System.Net.Http.Json;
using Adventures.Common.Model;
using MonkeyFinder.Interfaes;

namespace MonkeyFinder.Services;

public class InventoryOnlineService : IInventoryDataService
{
    HttpClient httpClient;

    List<ListItem> monkeyList;

    public string Mode { get; set; } = "ONLINE";

    public InventoryOnlineService()
    {
        this.httpClient = new HttpClient();
    }

    public async Task<List<ListItem>> GetInventory()
    {
        return await new InventoryOfflineService().GetInventory();

/*
        if (monkeyList?.Count > 0)
            return monkeyList;

        // Online
        var response = await httpClient.GetAsync("https://www.montemagno.com/monkeys.json");
        if (response.IsSuccessStatusCode)
        {
            monkeyList = await response.Content.ReadFromJsonAsync<List<ListItem>>();
        }
        return monkeyList;
*/
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

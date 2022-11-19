#pragma warning disable CA1416

namespace Adventures.Monkey.Services.Online;

public class MonkeyOnlineService : IMonkeyDataService
{
    HttpClient httpClient;

    List<ListItem> monkeyList;

    public string Mode { get; set; } = AppConstants.Online;

    public MonkeyOnlineService()
    {
        this.httpClient = new HttpClient();
    }

    public async Task<List<ListItem>> GetMonkeys()
    {
        if (monkeyList?.Count > 0)
            return monkeyList;

        // Online
        var response = await httpClient
            .GetAsync("https://www.montemagno.com/monkeys.json");

        if (response.IsSuccessStatusCode)
        {
            monkeyList = await response.Content
                .ReadFromJsonAsync<List<ListItem>>();
        }
        return monkeyList;
    }

    public async Task<T> GetDataAsync<T>(object sender, EventArgs e)
        where T : ServiceResult
    {
        var retValue = new ServiceResult
        {
            Data = await GetMonkeys()
        };
        return (T)retValue;
    }
}

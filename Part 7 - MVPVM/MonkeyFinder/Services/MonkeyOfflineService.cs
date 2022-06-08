﻿#pragma warning disable CA1416


using Adventures.Common.Model;
using MonkeyFinder.Interfaes;

namespace MonkeyFinder.Services;

public class MonkeyOfflineService : IMonkeyDataService
{
    public string Mode { get; set; } = "OFFLINE";

    List<ListItem> monkeyList;

    public MonkeyOfflineService() { }

    public async Task<List<ListItem>> GetMonkeys()
    {
        if (monkeyList?.Count > 0)
            return monkeyList;

        // Offline
        using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
        using var reader = new StreamReader(stream);
        var contents = await reader.ReadToEndAsync();
        monkeyList = JsonSerializer.Deserialize<List<ListItem>>(contents);

        return monkeyList;
    }

    public async Task<T> GetDataAsync<T>(object sender, EventArgs e) where T : ServiceResult
    {
        var retValue = new ServiceResult
        {
            Data = await GetMonkeys()
        };
        return (T)retValue;
    }
}

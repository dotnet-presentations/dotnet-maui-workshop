using System;
using System.Net.Http.Json;
using Adventures.Common.Constants;
using Adventures.Common.Entities;
using Adventures.Common.Model;
using Adventures.Data.Interfaces;
using Adventures.Data.Results;

namespace Adventures.Data.Services
{
	public class TripleStoreService : ITripleStoreService
	{
        HttpClient httpClient;

        public TripleStoreService()
		{
            this.httpClient = new HttpClient();
        }

        public async Task<List<Triple>> GetTripleStoreData()
        {
            var response = await httpClient
                .GetAsync("https://www.montemagno.com/monkeys.json");

            var retList = new List<Triple>();

            if (response.IsSuccessStatusCode)
            {
                var monkeyList = await response.Content
                    .ReadFromJsonAsync<List<ListItem>>();

                foreach (var monkey in monkeyList)
                {
                    var triple = new Triple
                    {
                        Subject = $"http://adventuresOnTheEdge.net/data/monkey/{monkey.Name}",
                        Predicate = "http://adventuresOnTheEdge.net/data/type/link",
                        Object = monkey.Image
                    };
                    retList.Add(triple);
                }
            }
            return retList;
        }

        public string Mode { get; set; } = AppConstants.Online;

        public async Task<T> GetDataAsync<T>(
            object sender = null, EventArgs e = null) where T : ServiceResult
        {
            var retValue = new TripleStoreResult
            {
                Data = await GetTripleStoreData()
            };

            return (T)(ServiceResult)retValue;
        }
    }
}


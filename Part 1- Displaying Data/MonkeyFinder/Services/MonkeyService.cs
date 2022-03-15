namespace MonkeyFinder.Services;

public class MonkeyService
{
    List<Monkey> monkeyList;
    public async Task<List<Monkey>> GetMonkeys()
    {
        return monkeyList;
    }

    public async Task<Monkey> GetMonkey(string name)
    {
        return null;
    }
}

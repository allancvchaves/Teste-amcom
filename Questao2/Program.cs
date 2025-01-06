using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Program
{
    private static readonly HttpClient client = new HttpClient();
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await GetTotalScoredGoals(teamName, year);

        Console.WriteLine($"Team {teamName} scored {totalGoals} goals in {year}");

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await GetTotalScoredGoals(teamName, year);

        Console.WriteLine($"Team {teamName} scored {totalGoals} goals in {year}");

        // Output expected:
        // Endpoint desatualizado - valores atuais com marcação *
        // Team Paris Saint - Germain scored 109 goals in 2013 // *62 goals in 2013
        // Team Chelsea scored 92 goals in 2014 // *47 goals in 2014
    }

    public static async Task<int> GetTotalScoredGoals(string team, int year)
    {

        int totalGoals = 0;
        int page = 1;
        bool hasMorePages = true;


        while (hasMorePages)
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={page}";
            string jsonResponse = await client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<JObject>(jsonResponse);

            JArray matches = (JArray)data["data"];
            if (matches == null || matches.Count == 0)
            {
                hasMorePages = false;
                break;
            }

            foreach (var match in matches)
            {

                if (match["team1"]?.ToString() == team)
                {
                    totalGoals += match["team1goals"]?.Value<int>() ?? 0;
                }

                if (match["team2"]?.ToString() == team)
                {
                    totalGoals += match["team2goals"]?.Value<int>() ?? 0;
                }
            }

            int totalMatches = data["total"]?.Value<int>() ?? 0;
            int matchesPerPage = matches.Count;

            hasMorePages = (page * matchesPerPage) < totalMatches;
            page++;
        }

        return totalGoals;

    }
}
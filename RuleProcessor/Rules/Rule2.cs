using RuleProcessor.DB;

namespace RuleProcessor.Rules;

public class Rule2 : BaseRule
{
    public Rule2(IDbService dbService, IHttpClientFactory httpClientFactory)
        : base(dbService, httpClientFactory)
    {
        this.RuleId = 2;
    }

    public override async Task<string> GatherData()
    {
        return await Task.FromResult("Rule2: Gathering Data");
    }

    public override async Task<string> Send(string request)
    {
        return await Task.FromResult("Rule2: Collecting Data");
    }

    public override async Task<string> Map(string response)
    {
        return await Task.FromResult("Rule2: Calling Endpoint");
    }
}

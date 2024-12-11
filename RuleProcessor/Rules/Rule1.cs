using RuleProcessor.DB;

namespace RuleProcessor.Rules;

public class Rule1 : BaseRule
{
    public Rule1(IDbService dbService, IHttpClientFactory httpClientFactory)
        : base(dbService, httpClientFactory)
    {
        this.RuleId = 1;
    }

    public override async Task<string> GatherData()
    {
        return await Task.FromResult("Rule1: Gathering Data");
    }

    public override async Task<string> Send(string request)
    {
        return await Task.FromResult("Rule1: Collecting Data");
    }

    public override async Task<string> Map(string response)
    {
        return await Task.FromResult("Rule1: Calling Endpoint");
    }
}

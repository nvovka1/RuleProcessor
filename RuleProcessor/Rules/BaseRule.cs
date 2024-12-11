using RuleProcessor.DB;

namespace RuleProcessor.Rules;

public abstract class BaseRule
{
    protected readonly IDbService DbService;
    protected readonly IHttpClientFactory HttpClientFactory;

    public int RuleId { get; set; }

    protected BaseRule(IDbService dbService, IHttpClientFactory httpClientFactory)
    {
        RuleId = 0;
        DbService = dbService;
        HttpClientFactory = httpClientFactory;
    }

    public abstract Task<string> GatherData();
    public abstract Task<string> Send(string request);
    public abstract Task<string> Map(string response);
}

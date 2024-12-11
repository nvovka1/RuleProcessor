using Microsoft.Azure.WebJobs;
using RuleProcessor.Rules;

namespace RuleProcessor.Process;

public class RuleProcessorService(IEnumerable<BaseRule> rules)
{
    private IAsyncCollector<string> _eventHubOutput;

    public async Task ProcessAllRulesAsync(int batchSize)
    {
        var batches = rules.Select((rule, index) => new { rule, index })
                      .GroupBy(x => x.index / batchSize, x => x.rule);

        foreach (var batch in batches)
        {
            // Map Phase
            var results = await Task.WhenAll(batch.Select(async rule =>
            {
                var data = await rule.GatherData();

                var collectedData = await rule.Send(data);

                return new
                {
                    RuleId = rule.RuleId,
                    Data = data,
                    CollectedData = collectedData
                };
            }));

            // Reduce
            var callResults = await Task.WhenAll(results.Select(async result =>
            {
                var rule = batch.First(r => r.RuleId == result.RuleId);

                var response = await rule.Map(result.C);
                return new
                {
                    result.RuleId,
                    result.Data,
                    result.CollectedData,
                    response = response
                };
            }));

            foreach (var result in callResults)
            {
                await _eventHubOutput.AddAsync(result.response);
            }
        }
    }

    internal void SetOutput(IAsyncCollector<string> eventHubOutput)
    {
        _eventHubOutput = eventHubOutput;
    }
}

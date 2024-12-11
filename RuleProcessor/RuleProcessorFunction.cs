using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using RuleProcessor.Process;

namespace RuleProcessor;

public class RuleProcessorFunction
{
    private readonly ILogger _logger;
    private readonly RuleProcessorService _ruleProcessor;

    public RuleProcessorFunction(ILoggerFactory loggerFactory, RuleProcessorService ruleProcessor)
    {
        _logger = loggerFactory.CreateLogger<RuleProcessorFunction>();
        _ruleProcessor = ruleProcessor;
    }
   
    [FunctionName(nameof(RuleProcessorFunction))]
    public async Task RunAsync(
    [TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo,
    ILogger log,
    [EventHub("outputEventHubMessage", Connection = "EventHubConnectionAppSetting")] IAsyncCollector<string> eventHubOutput)
    {
        _ruleProcessor.SetOutput(eventHubOutput);

        await _ruleProcessor.ProcessAllRulesAsync(10);
    }
}

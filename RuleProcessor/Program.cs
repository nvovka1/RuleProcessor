using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RuleProcessor.DB;
using RuleProcessor.Process;
using RuleProcessor.Rules;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddSingleton<IDbService, DbService>();
        services.AddHttpClient();

        var ruleTypes = typeof(BaseRule).Assembly.GetTypes()
            .Where(t => t.IsClass && t.IsSubclassOf(typeof(BaseRule)));

        foreach (var ruleType in ruleTypes)
        {
            services.AddTransient(typeof(BaseRule), ruleType);
        }

        services.AddScoped<RuleProcessorService>();
    })
    .Build();

host.Run();
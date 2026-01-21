using FluentEmail.Core;
using FluentEmail.Core.Models;
using Polly;
using Polly.Registry;
using Polly.Retry;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddFluentEmail("info@tanersaydam.com").AddSmtpSender(new SmtpClient() { Host = "localhost", Port = 25 });

builder.Services.AddResiliencePipeline("http", cfr =>
{
    cfr.AddRetry(new RetryStrategyOptions()
    {
        ShouldHandle = new PredicateBuilder().Handle<SmtpException>()
    }).Build();
});

var app = builder.Build();

app.MapGet("/get-todos", async (HttpClient httpClient) =>
{
    string endpoint = "https://jsonplaceholder.typicode.com/todos";
    var res = await PipelineService.HttpPipeline.ExecuteAsync(async ct => await httpClient.GetAsync(endpoint, ct));

    res.EnsureSuccessStatusCode();

    var todos = await res.Content.ReadFromJsonAsync<List<Todo>>();

    return todos;
});

app.MapGet("/send-email", async (IFluentEmail fluentEmail, ResiliencePipelineProvider<string> pipelineProvider) =>
{
    var res = await PipelineService.EmailPipeline.ExecuteAsync(async ct => await fluentEmail.To("my@business.com").Subject("Hello world").SendAsync());
    //var pipeline = pipelineProvider.GetPipeline("http");
    //var res = await pipeline.ExecuteAsync(async ct => await fluentEmail.To("my@business.com").Subject("Hello world").SendAsync());

    return res.Successful;
});

app.Run();

record Todo(
    int UserId,
    int Id,
    string Title,
    bool Completed);

public class PipelineService
{
    public static ResiliencePipeline<HttpResponseMessage> HttpPipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
    .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
    {
        MaxRetryAttempts = 3, //deneme süresi
        Delay = TimeSpan.FromSeconds(5), //her denemede bu kadar bekle
        ShouldHandle = new PredicateBuilder<HttpResponseMessage>() //sonucu nasıl işleyeceği
            .Handle<HttpRequestException>() //exception fırltatırsa tekrar dene
            .HandleResult(r => !r.IsSuccessStatusCode) //exception fırlatmasa bile dönen sonuç buysa başarısız say
    })
    .AddTimeout(TimeSpan.FromSeconds(20)) //Timeout, bir deneme 20 saniyeyi aşarsa iptal eder.
    .Build();

    public static ResiliencePipeline<SendResponse> EmailPipeline = new ResiliencePipelineBuilder<SendResponse>()
        .AddRetry(new RetryStrategyOptions<SendResponse>
        {
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromSeconds(5),
            ShouldHandle = new PredicateBuilder<SendResponse>()
            .Handle<SmtpException>() //exception fırltatırsa tekrar dene
            //.HandleResult(r => !r.Successful) //exception fırlatmasa bile dönen sonuç buysa başarısız say
        })
        .AddTimeout(TimeSpan.FromSeconds(20))
        .Build();
}
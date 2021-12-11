# Mandrill.net

Simple, cross-platform Mandrill api wrapper for .NET Core

[![Coverage Status](https://coveralls.io/repos/github/feinoujc/Mandrill.net/badge.svg?branch=main)](https://coveralls.io/github/feinoujc/Mandrill.net?branch=main)
<a href="http://www.nuget.org/packages/Mandrill.net/"><img src="https://img.shields.io/nuget/v/Mandrill.net.svg" title="NuGet Status"></a>

## API Docs

https://mandrillapp.com/api/docs/

## Getting Started

```ps
dotnet add package Mandrill.net

# use Mandrill with HttpClientFactory
dotnet add package Mandrill.net.Extensions.DependencyInjection
```


### Send a new transactional message through Mandrill

```cs
var api = new MandrillApi("YOUR_API_KEY_GOES_HERE");
var message = new MandrillMessage("from@example.com", "to@example.com",
                "hello mandrill!", "...how are you?");
var result = await api.Messages.SendAsync(message);
```

### Send a new transactional message through Mandrill using a template

```cs
var api = new MandrillApi("YOUR_API_KEY_GOES_HERE");
var message = new MandrillMessage();
message.FromEmail = "no-reply@acme.com";
message.AddTo("recipient@example.com");
message.ReplyTo = "customerservice@acme.com";
//supports merge var content as string
message.AddGlobalMergeVars("invoice_date", DateTime.Now.ToShortDateString());
//or as objects (handlebar templates only)
message.AddRcptMergeVars("recipient@example.com", "invoice_details", new[]
{
    new Dictionary<string, object>
    {
        {"sku", "apples"},
        {"qty", 4},
        {"price", "0.40"}
    },
    new Dictionary<string, object>
    {
        {"sku", "oranges"},
        {"qty", 6},
        {"price", "0.30"}

    }
});

var result = await api.Messages.SendTemplateAsync(message, "customer-invoice");

```


### Service Registration

It is recommended that you do not create an instance of the `MandrillApi` for every request, to effectively pool connections to mandrill, and prevent socket exhaustion in your app. If you are using .net dependency injection, you can use the `Mandrill.net.Extensions.DependencyInjection` package which includes a `IServiceCollection.AddMandrill()` extension method, allowing you to register all the needed interfaces and also customize the [HttpClientFactory](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests) to efficiently manage the HttpClient connections.

```cs
using Microsoft.Extensions.DependencyInjection;
using Mandrill;
using Mandrill.Model;
using Mandrill.Extensions.DependencyInjection;

var services = ConfigureServices(new ServiceCollection()).BuildServiceProvider();
var api = services.GetRequiredService<IMandrillApi>();
// we can also target specific mandrill api endpoint interfaces...
//var messagesApi = services.GetRequiredService<IMandrillMessagesApi>();
var message = new MandrillMessage("from@example.com", "to@example.com",
        "hello mandrill!", "...how are you?");
var result = await api.Messages.SendAsync(message);

static IServiceCollection ConfigureServices(IServiceCollection services)
{
    services.AddMandrill(options =>
    {
        options.ApiKey = "YOUR_API_KEY_GOES_HERE"; // Load the api key from configuration
    });

    return services;
}
```


### Processing a web hook batch

```cs
[HttpPost]
[Route("/api/some/route/outbound")]
[Consumes("application/x-www-form-urlencoded")]
public async Task<IActionResult> Outbound([FromForm(Name = "mandrill_events")] string body)
{
    if (!Request.Headers.TryGetValue("X-Mandrill-Signature", out var signature))
    {
        return Unauthorized();
    }


    var events = MandrillMessageEvent.ParseMandrillEvents(body);

    // accept an empty test request
    if (events.Count == 0)
    {
        return Accepted();
    }

    if (!ValidateRequest(body, signature, "WEBHOOK_SECRET_KEY_HERE"))
    {
        return Forbid();
    }

    foreach (var messageEvent in events)
    {
        // do something with the event
    }
    return Ok();
}


private bool ValidateRequest(string body, string signature, string authKey)
{
    var form = new NameValueCollection();
    form.Set("mandrill_events", body);
    return WebHookSignatureHelper.VerifyWebHookSignature(signature, authKey, new Uri(Request.GetDisplayUrl()), form);
}
```

## Building

```sh
dotnet build
```

## Testing

**You must set the user environment variable MANDRILL_API_KEY in order to run these tests. Go to https://mandrillapp.com/ to obtain an api key.**

**In order for the email from address to match your allowed sending domains, you can set MANDRILL_SENDING_DOMAIN to match your account.**

```sh
# include MANDRILL_API_KEY and MANDRILL_SENDING_DOMAIN in your env. For example:
# MANDRILL_API_KEY=xxxxxxxxx MANDRILL_SENDING_DOMAIN=acme.com dotnet test tests
dotnet test
```

## API coverage

See [this issue](https://github.com/feinoujc/Mandrill.net/issues/1) to track progress of api implementation

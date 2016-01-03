Mandrill.net
============

Mandrill api wrapper for .net

[![Travis](https://travis-ci.org/feinoujc/Mandrill.net.svg?branch=master)](https://travis-ci.org/feinoujc/Mandrill.net)
[![AppVeyor](https://ci.appveyor.com/api/projects/status/kfgnqdmrvhlc36co/branch/master?svg=true)](https://ci.appveyor.com/project/feinoujc/mandrill-net/branch/master)
<a href="http://www.nuget.org/packages/Mandrill.net/"><img src="http://img.shields.io/nuget/v/Mandrill.net.svg?" title="NuGet Status"></a>

## API Docs

https://mandrillapp.com/api/docs/

## Getting Started

```ps
Install-Package Mandrill.net
```

### .NET Core support

.NET core support is available in pre-release

```ps
Install-Package Mandrill.net -Pre
```

### Send a new transactional message through Mandrill (async)

```cs
var api = new MandrillApi("YOUR_API_KEY_GOES_HERE");
var message = new MandrillMessage("from@example.com", "to@example.com",
                "hello mandrill!", "...how are you?");
var result = await api.Messages.SendAsync(message);
```

### Send a new transactional message through Mandrill (non-async)

All the api's are available in async or non-async in the .net 4.5 target version of this library. In .NET Core, only async is supported because the underlying api's for web requests in .NET are async only.

```cs
var api = new MandrillApi("YOUR_API_KEY_GOES_HERE");
var message = new MandrillMessage("from@example.com", "to@example.com",
                "hello mandrill!", "...how are you?");
var result = api.Messages.Send(message);
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

### Processing a web hook batch

```cs
[HttpPost]
public IHttpActionResult MyWebApiControllerMethod(FormDataCollection value)
{
    //optional: validate your webhook signature
    // see https://mandrill.zendesk.com/hc/en-us/articles/205583257-How-to-Authenticate-Webhook-Requests
    if(!ValidateRequest(value))
    {
      return Forbidden();
    }
    
    var events = MandrillMessageEvent.ParseMandrillEvents(value.Get("mandrill_events"));
    foreach (var messageEvent in events)
    {
        // do something with the event
    }
    return Ok();
}

private bool ValidateRequest(FormDataCollection value)
{
   IEnumerable<string> headers;
   if (!Request.Headers.TryGetValues("X-Mandrill-Signature", out headers))
   {
     return false;
   }
   var signature = headers.Single();
   var key = "MANDRILL_WEBHOOK_KEY_HERE";
   
   return WebHookSignatureHelper.VerifyWebHookSignature(signature, key, Request.RequestUri, value.ReadAsNameValueCollection());
}
```

## Integrations

* [NServiceBus.Mandrill](https://github.com/feinoujc/NServiceBus.Mandrill) - Integrates NServiceBus messaging framework and mandrill, for more reliable mail processing

## API coverage



See [this issue](https://github.com/feinoujc/Mandrill.net/issues/1) to track progress of api implementation


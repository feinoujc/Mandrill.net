Mandrill.net
============

Mandrill api wrapper for .net

[![Build Status](https://travis-ci.org/feinoujc/Mandrill.net.svg?branch=master)](https://travis-ci.org/feinoujc/Mandrill.net)

## API Docs

https://mandrillapp.com/api/docs/

## Getting Started

```ps
Install-Package Mandrill.net
```

### Send a new transactional message through Mandrill

```cs
var api = new MandrillApi("YOUR_API_KEY_GOES_HERE");
var message = new MandrillMessage("from@example.com", "to@example.com",
                "hello mandrill!", "...how are you?");
var result = await api.Messages.SendAsync(message);
//or non-async (all methods have non-async version)
//var result = api.Messages.Send(message);
```

### Send a new transactional message through Mandrill using a template
```cs
var api = new MandrillApi("YOUR_API_KEY_GOES_HERE");
var message = new MandrillMessage();
message.AddTo("recipient@example.com");
message.ReplyTo = "customerservice@acme.com";
//supports merge var content as string
message.AddGlobalMergeVars("invoice_date", DateTime.Now.ToShortDateString());
//or as objects (handlebar templates only)
message.AddRcptMergeVars("recipient@example.com", "invoice_details", new[]
{
    new Dictionary<string, string>
    {
        {"sku", "apples"},
        {"qty", "4"},
        {"price", "0.40"}
    },
    new Dictionary<string, string>
    {
        {"sku", "oranges"},
        {"qty", "6"},
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
    var events = MandrillMessageEvent.ParseMandrillEvents((value.Get("mandrill_events")));
    foreach (var messageEvent in events)
    {
        //...
    }
    return Ok();
}
```

## API coverage



See [this issue](https://github.com/feinoujc/Mandrill.net/issues/1) to track progress of api implementation


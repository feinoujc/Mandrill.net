Mandrill.net
============

Mandrill api wrapper for .net

## API Docs

https://mandrillapp.com/api/docs/

## Getting Started

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
var message = new MandrillMessage("from@example.com", "to@example.com");
message.AddGlobalMergeVars("INVOICE_DETAILS", "This is an invoice...");
var templateContent = new List<MandrillTemplateContent>();
templateContent.Add(new MandrillTemplateContent() {Name = "footer", Content = "<footer>Invoice footer</footer>"});
var result = await api.Messages.SendTemplateAsync(message, "account-invoice", templateContent);
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


Mandrill.net
============

Mandrill api wrapper for .net

## API Docs

https://mandrillapp.com/api/docs/

## Getting Started

```cs
var api = new MandrillApi("YOUR_API_KEY_GOES_HERE");
var message = new MandrillMessage("from@example.com", "to@example.com",
                "hello mandrill!", "...how are you?");
await api.Messages.SendAsync(message);
//or non-async
api.Messages.Send(message);
```
## API coverage

See [this issue](https://github.com/feinoujc/Mandrill.net/issues/1) to track progress of api implementation


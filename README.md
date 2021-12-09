# SignalrMQ

<img src="Resources/Logo_SignalrMQ.png" width=128 />

## The Idea

Message Brokers like MQTT oder RabbitMQ are very great and awesome tools, but maybe sometimes is it a little too much overhead to setup a dedicated instance of such type of software.

Also if you are going ahead with the .net core environment maybe you wish to implemement a MQTT-like broker instance for your own or in your own application. So, here is the try to implement such a piece of software using SignalR.

## Features

* Publish-Subscribe principle similar to MQTT
* Reqest messages (Messages with return value, personal wishlist-feature)


## Implementation

This project consist of:

* a broker library which implements the SignalR-Hub
* a client library

The implementation based on .Net 6 Core

## Usage

### Broker

#### Nuget
Use [Nuget-Package](https://www.nuget.org/packages/SignalrMQ.Broker)
```
Install-Package SignalrMQ.Broker
```


Implement in exisiting or new (Asp.Net)-Application-Startup:

```csharp
app.UseEndpoints(endpoints => endpoints.MapHub<SignalrMqBroker>("signalrmqbrokerhub"));
```

Sample-Project: SignalrMQ.BlazorBroker

### Clients

#### Nuget
Use [Nuget-Package](https://www.nuget.org/packages/SignalrMQ.Client)
```
Install-Package SignalrMQ.Client
```

```csharp
services.AddSignalrMqClientService(opts => hostContext.Configuration.GetSection(nameof(SignalrMqEndpoint)).Bind(opts));
```

Configure in aasettings.json:
```json
"SignalrMqEndpoint": {
    "Host": "localhost",
    "Port": 7284
  }
```

Subscribe, Publish, Request ... more description coming soon. Please take a look at [Worker.cs](https://github.com/msiggi/SignalrMQ/blob/master/SignalrMQ.WorkerServiceClientRcv/Worker.cs)


## Missing features

Currently is no authentification implemented. Please be aware of this if you publish SignalrMQ-based Applications in public internet.
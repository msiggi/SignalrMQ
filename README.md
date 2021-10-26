# SignalrMQ

<img src="Resources/Logo_SignalrMQ.png" width=128 />

## The Idea

Message Brokers like MQTT oder RabbitMQ are very great and awesome tools, but maybe sometimes is it a little too much overhead to setup a dedicated instance of such type of software.

Also if you are going ahead with the .net core environment maybe you wish to implemement a MQTT-like broker instance for your own or in your own application. So, here is the try to implement such a piece of software using SignalR.

## Features

* Publish-Subscribe principle similar to MQTT
* Reqest messages (Messages with return value)


## Implementation

This project consist of:

* a broker library which implements the SignalR-Hub
* a client library

The implementation based on .Net 6 Core

## Missing features

Currently is no authentification implemented. Please be aware of this if you publish SignalrMQ-based Applications in public internet.
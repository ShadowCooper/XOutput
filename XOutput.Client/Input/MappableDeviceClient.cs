﻿using System;
using System.Threading.Tasks;
using XOutput.Api.Message;
using XOutput.Api.Serialization;
using XOutput.Client.Websocket;
using XOutput.Core.WebSocket;
using XOutput.Message.Mappable;

namespace XOutput.Client.Input
{
    public class MappableDeviceClient : WebsocketJsonClient
    {
        public event FeedbackReceived FeedbackReceived;

        public MappableDeviceClient(MessageReader messageReader, MessageWriter messageWriter, WebSocketHelper webSocketHelper, Uri baseUri) : base(messageReader, messageWriter, webSocketHelper, baseUri)
        {

        }

        public async Task ConnectAsync(MappableDeviceDetailsMessage message)
        {
            await ConnectAsync("mappableDevice");
            await SendAsync(message);
        }

        protected override void ProcessMessage(MessageBase message)
        {
            if (message is MappableDeviceFeedbackMessage)
            {
                var feedbackMessage = message as MappableDeviceFeedbackMessage;
                FeedbackReceived?.Invoke(this, new FeedbackReceivedEventArgs
                {
                    SmallForceFeedback = feedbackMessage.SmallForceFeedback,
                    BigForceFeedback = feedbackMessage.BigForceFeedback,
                });
            }
        }

        protected Task SendInputAsync(MappableDeviceInputMessage message)
        {
            return SendAsync(message);
        }
    }

    public delegate void FeedbackReceived(object sender, FeedbackReceivedEventArgs args);

    public class FeedbackReceivedEventArgs
    {
        public double SmallForceFeedback { get; set; }
        public double BigForceFeedback { get; set; }
    }
}

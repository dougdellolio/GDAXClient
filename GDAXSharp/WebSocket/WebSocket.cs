﻿using GDAXSharp.Exceptions;
using GDAXSharp.Network.Authentication;
using GDAXSharp.Shared.Types;
using GDAXSharp.Shared.Utilities;
using GDAXSharp.Shared.Utilities.Clock;
using GDAXSharp.Shared.Utilities.Extensions;
using GDAXSharp.WebSocket.Models.Request;
using GDAXSharp.WebSocket.Models.Response;
using GDAXSharp.WebSocket.Types;
using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using WebSocket4Net;

namespace GDAXSharp.WebSocket
{
    public class WebSocket
    {
        private readonly IWebSocketFeed webSocketFeed;

        private readonly IAuthenticator authenticator;

        private readonly IClock clock;

        private bool stopWebSocket;

        private List<ProductType> productTypes;

        private List<ChannelType> channelTypes;

        public WebSocket(
            IWebSocketFeed webSocketFeed,
            IAuthenticator authenticator,
            IClock clock)
        {
            this.webSocketFeed = webSocketFeed;
            this.authenticator = authenticator;
            this.clock = clock;
        }

        ~WebSocket()
        {
            webSocketFeed?.Close();
            webSocketFeed?.Dispose();
        }

        public void Start(
            List<ProductType> providedProductTypes, 
            List<ChannelType> providedChannelTypes = null)
        {
            if (providedProductTypes.Count == 0)
            {
                throw new ArgumentException("You must specify at least one product type");
            }

            stopWebSocket = false;

            productTypes = providedProductTypes;
            channelTypes = providedChannelTypes;

            webSocketFeed.Closed += WebSocket_Closed;
            webSocketFeed.Error += WebSocket_Error;
            webSocketFeed.MessageReceived += WebSocket_MessageReceived;
            webSocketFeed.Opened += WebSocket_Opened;
            webSocketFeed.SetEvents();
            webSocketFeed.Open();
        }

        public void WebSocket_Opened(object sender, EventArgs e)
        {
            if (productTypes.Count == 0)
            {
                throw new ArgumentException("You must specify at least one product type");
            }

            var channels = GetChannels();

            var timeStamp = clock.GetTime().ToTimeStamp();
            var json = JsonConfig.SerializeObject(new TickerChannel
            {
                Type = ActionType.Subscribe,
                ProductIds = productTypes,
                Channels = channels,
                Timestamp = timeStamp.ToString("F0", CultureInfo.InvariantCulture),
                Key = authenticator.ApiKey,
                Passphrase = authenticator.Passphrase,
                Signature = authenticator.ComputeSignature(HttpMethod.Get, authenticator.UnsignedSignature, timeStamp,
                    "/users/self/verify")
            });

            webSocketFeed.Send(json);
        }

        private List<Channel> GetChannels()
        {
            var channels = new List<Channel>();
            if (channelTypes == null || channelTypes.Count == 0)
            {
                foreach (ChannelType channelType in Enum.GetValues(typeof(ChannelType)))
                {
                    channels.Add(new Channel(channelType, productTypes));
                }
            }
            else
            {
                foreach (var channelType in channelTypes)
                {
                    channels.Add(new Channel(channelType, productTypes));
                }
            }

            return channels;
        }

        public void WebSocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (stopWebSocket)
            {
                webSocketFeed.Close();
                return;
            }

            var json = e.Message;
            var response = JsonConfig.DeserializeObject<BaseMessage>(json);

            switch (response.Type)
            {
                case ResponseType.Subscriptions:
                    break;
                case ResponseType.Ticker:
                    var ticker = JsonConfig.DeserializeObject<Ticker>(json);
                    OnTickerReceived?.Invoke(sender, new WebfeedEventArgs<Ticker>(ticker));
                    break;
                case ResponseType.Snapshot:
                    var snapshot = JsonConfig.DeserializeObject<Snapshot>(json);
                    OnSnapShotReceived?.Invoke(sender, new WebfeedEventArgs<Snapshot>(snapshot));
                    break;
                case ResponseType.L2Update:
                    var level2 = JsonConfig.DeserializeObject<Level2>(json);
                    OnLevel2UpdateReceived?.Invoke(sender, new WebfeedEventArgs<Level2>(level2));
                    break;
                case ResponseType.Heartbeat:
                    var heartbeat = JsonConfig.DeserializeObject<Heartbeat>(json);
                    OnHeartbeatReceived?.Invoke(sender, new WebfeedEventArgs<Heartbeat>(heartbeat));
                    break;
                case ResponseType.Received:
                    var received = JsonConfig.DeserializeObject<Received>(json);
                    OnReceivedReceived?.Invoke(sender, new WebfeedEventArgs<Received>(received));
                    break;
                case ResponseType.Open:
                    var open = JsonConfig.DeserializeObject<Open>(json);
                    OnOpenReceived?.Invoke(sender, new WebfeedEventArgs<Open>(open));
                    break;
                case ResponseType.Done:
                    var done = JsonConfig.DeserializeObject<Done>(json);
                    OnDoneReceived?.Invoke(sender, new WebfeedEventArgs<Done>(done));
                    break;
                case ResponseType.Match:
                    var match = JsonConfig.DeserializeObject<Match>(json);
                    OnMatchReceived?.Invoke(sender, new WebfeedEventArgs<Match>(match));
                    break;
                case ResponseType.LastMatch:
                    var lastMatch = JsonConfig.DeserializeObject<LastMatch>(json);
                    OnLastMatchReceived?.Invoke(sender, new WebfeedEventArgs<LastMatch>(lastMatch));
                    break;
                case ResponseType.Error:
                    var error = JsonConfig.DeserializeObject<Error>(json);
                    OnErrorReceived?.Invoke(sender, new WebfeedEventArgs<Error>(error));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void WebSocket_Error(object sender, ErrorEventArgs e)
        {
            throw new GDAXSharpWebSocketException($"WebSocket Feed Error: {e.Exception.Message}")
            {
                WebSocketFeed = webSocketFeed,
                StatusCode = webSocketFeed.State,
                ErrorEvent = e
            };
        }

        public void WebSocket_Closed(object sender, EventArgs e)
        {
            webSocketFeed.Dispose();
            Start(productTypes, channelTypes);
        }

        public event EventHandler<WebfeedEventArgs<Ticker>> OnTickerReceived;
        public event EventHandler<WebfeedEventArgs<Snapshot>> OnSnapShotReceived;
        public event EventHandler<WebfeedEventArgs<Level2>> OnLevel2UpdateReceived;
        public event EventHandler<WebfeedEventArgs<Heartbeat>> OnHeartbeatReceived;
        public event EventHandler<WebfeedEventArgs<Received>> OnReceivedReceived;
        public event EventHandler<WebfeedEventArgs<Open>> OnOpenReceived;
        public event EventHandler<WebfeedEventArgs<Done>> OnDoneReceived;
        public event EventHandler<WebfeedEventArgs<Match>> OnMatchReceived;
        public event EventHandler<WebfeedEventArgs<LastMatch>> OnLastMatchReceived;
        public event EventHandler<WebfeedEventArgs<Error>> OnErrorReceived;
    }
}

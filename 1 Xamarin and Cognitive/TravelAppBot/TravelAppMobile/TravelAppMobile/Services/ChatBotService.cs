using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TravelAppMobile.Models;

namespace TravelAppMobile.Services
{
    public class ChatBotService
    {
        private readonly string _baseBotEndPointAddress = "https://directline.botframework.com";
        private HttpClient _client;
        private string _directLineKey = "Q9IBXWIBSUg.cwA.1pw.aVXMkIWleOmQgRWWRKzYJwMoj63fP57FCeATbE7qW40";
        private Conversation _lastConversation = null;

        public ChatBotService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseBotEndPointAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("BotConnector", _directLineKey);

            InitializeConversation();
        }

        private async void InitializeConversation()
        {
            var response = await _client.GetAsync("/api/tokens/");
            if (response.IsSuccessStatusCode)
            {
                var conversation = new Conversation();
                HttpContent contentPost = new StringContent(JsonConvert.SerializeObject(conversation), Encoding.UTF8,
                    "application/json");
                response = await _client.PostAsync("/api/conversations/", contentPost);
                if (response.IsSuccessStatusCode)
                {
                    var conversationInfo = await response.Content.ReadAsStringAsync();
                    _lastConversation = JsonConvert.DeserializeObject<Conversation>(conversationInfo);
                }
            }
        }

        public async Task<MessageSet> SendMessage(string message)
        {
            try
            {
                var messageToSend = new BotMessage() { Text = message, ConversationId = _lastConversation.ConversationId };
                var contentPost = new StringContent(JsonConvert.SerializeObject(messageToSend), Encoding.UTF8, "application/json");
                var conversationUrl = $"{_baseBotEndPointAddress}/api/conversations/{_lastConversation.ConversationId}/messages/";


                var response = await _client.PostAsync(conversationUrl, contentPost);
                var messageInfo = await response.Content.ReadAsStringAsync();

                var messagesReceived = await _client.GetAsync(conversationUrl);
                var messagesReceivedData = await messagesReceived.Content.ReadAsStringAsync();

                var messages = JsonConvert.DeserializeObject<MessageSet>(messagesReceivedData);

                return messages;
            }
            catch (Exception ex)
            {
                return SetExceptionMessage(ex);
            }
        }

        private static MessageSet SetExceptionMessage(Exception ex)
        {
            Debug.WriteLine(ex.Message);

            return new MessageSet()
            {
                Messages = (new List<BotMessage>
                {
                    new BotMessage
                    {
                        Text = "Something went wrong!",
                        From = "Bot"
                    }
                }).ToArray()
            };
        }
    }
}

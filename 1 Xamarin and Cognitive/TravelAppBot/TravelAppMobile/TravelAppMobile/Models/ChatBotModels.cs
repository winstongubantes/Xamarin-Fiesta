using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TravelAppMobile.Models
{
    public class MessageSet
    {
        [JsonProperty(PropertyName = "messages")]
        public BotMessage[] Messages { get; set; }

        [JsonProperty(PropertyName = "watermark")]
        public string WaterMark { get; set; }

        [JsonProperty(PropertyName = "eTag")]
        public string ETag { get; set; }

    }

    public class BotMessage
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "conversationId")]
        public string ConversationId { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        [JsonProperty(PropertyName = "from")]
        public string From { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "channelData")]
        public string ChannelData { get; set; }

        [JsonProperty(PropertyName = "images")]
        public string[] Images { get; set; }

        [JsonProperty(PropertyName = "attachments")]
        public Attachment[] Attachments { get; set; }

        [JsonProperty(PropertyName = "eTag")]
        public string ETag { get; set; }
    }

    public class Attachment
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; set; }
    }

    public class Conversation
    {
        [JsonProperty(PropertyName = "conversationId")]
        public string ConversationId { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "eTag")]
        public string ETag { get; set; }
    }
}

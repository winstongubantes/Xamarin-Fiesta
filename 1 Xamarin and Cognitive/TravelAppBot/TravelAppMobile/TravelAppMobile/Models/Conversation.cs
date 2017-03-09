using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TravelAppMobile.Models
{
    public class ConversationMessage
    {
        public string Id { get; set; }

        public string Message { get; set; }

        public string FromUser { get; set; }

        public ImageSource UserImageUrl { get; set; }
    }
}

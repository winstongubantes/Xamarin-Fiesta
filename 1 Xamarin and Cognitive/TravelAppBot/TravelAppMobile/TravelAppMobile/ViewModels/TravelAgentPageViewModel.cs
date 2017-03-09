using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelAppMobile.Models;
using TravelAppMobile.Services;
using Xamarin.Forms;
using Application = Xamarin.Forms.PlatformConfiguration.AndroidSpecific.Application;

namespace TravelAppMobile.ViewModels
{
    public class TravelAgentPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<ConversationMessage> _messages;

        private string _textMessage;

        private bool _isRefreshing;

        private ICommand _sendMessageCommand;

        private ICommand _backCommand;

        private ChatBotService _chatBotService = new ChatBotService();

        public TravelAgentPageViewModel()
        {
            _messages = new ObservableCollection<ConversationMessage>();
            _messages.Add(new ConversationMessage
            {
                Message = "Hi this is travel agent, how may i help you?",
                FromUser = "TRAVEL AGENT",
                UserImageUrl = "travelagent.jpg"
            });
        }

        public ObservableCollection<ConversationMessage> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                OnPropertyChanged("Messages");
            }
        }

        public string TextMessage
        {
            get { return _textMessage; }
            set
            {
                _textMessage = value;
                OnPropertyChanged("TextMessage");
            }
        }

        public ICommand SendMessageCommand
        {
            get
            {
                return _sendMessageCommand = _sendMessageCommand ?? new Command(async () =>
                {
                    Messages.Add(new ConversationMessage
                    {
                        FromUser = "AVID CUSTOMER",
                        Message = TextMessage,
                        UserImageUrl = "winston.png"
                    });

                    IsRefreshing = true;
                   
                    var reply = await _chatBotService.SendMessage(TextMessage);

                    TextMessage = "";

                    Messages.Clear();

                    foreach (var msgItem in reply.Messages)
                    {
                        Messages.Add(new ConversationMessage
                        {
                            FromUser = msgItem.From == "travelagentbotcn" ? "TRAVEL AGENT" : "AVID CUSTOMER",
                            Message = msgItem.Text,
                            UserImageUrl = msgItem.From == "travelagentbotcn" ? "travelagent.png" : "winston.png" //travelagentbotcn IS THE NAME OF THE BOT YOU CREATED
                        });
                    }

                    IsRefreshing = false;
                });
            }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged("IsRefreshing");
            }
        }

        public ICommand BackCommand
        {
            get
            {
                return _backCommand = _backCommand ?? new Command(() =>
                {
                    Xamarin.Forms.Application.Current.MainPage.Navigation.PopAsync();
                });
            }
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

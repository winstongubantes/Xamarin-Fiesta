using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace TravelAppMobile.ViewModels
{
    public class OCRPageViewModel : INotifyPropertyChanged
    {
        private ICommand _backCommand;
        private ICommand _convertCommand;
        private string _textValueFromImage;
        private bool _isBusy;

        public Action<string> OnErrorAction { get; set; }

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

        public string TextValueFromImage
        {
            get { return _textValueFromImage; }
            set
            {
                _textValueFromImage = value;
                OnPropertyChanged("TextValueFromImage");
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public ICommand ConvertCommand
        {
            get
            {
                return _convertCommand = _convertCommand ?? new Command(async() =>
                {
                    try
                    {
                        IsBusy = true;
                        // 1. Add camera logic.
                        await CrossMedia.Current.Initialize();

                        MediaFile photo;
                        if (CrossMedia.Current.IsCameraAvailable)
                        {
                            photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                            {
                                Directory = "Images",
                                Name = "Images"
                            });
                        }
                        else
                        {
                            photo = await CrossMedia.Current.PickPhotoAsync();
                        }

                        if (photo == null)
                        {
                            return;
                        }


                        // 2. Add  OCR logic.
                        OcrResults text;

                        var client = new VisionServiceClient("071e3503b2ec468998db34945a72e134");

                        using (var stream = photo.GetStream())
                            text = await client.RecognizeTextAsync(stream);

                        var words = from region in text.Regions
                                      from line in region.Lines
                                      from word in line.Words
                                      select word.Text;


                        TextValueFromImage = string.Join(",", words);
                    }
                    catch (Exception ex)
                    {
                        OnErrorAction?.Invoke(ex.Message);
                    }
                    finally
                    {
                        IsBusy = false;
                    }
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

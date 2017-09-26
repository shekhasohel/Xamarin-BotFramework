using Plugin.AudioRecorder;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Cognitive.BingSpeech;
using Xamarin.Forms;

namespace TodoList
{
    public class TodoItemListViewModel : INotifyPropertyChanged
    {
        string resultText = "";
        string audioDirectoryPath = "";
        bool recording = false;
        string extension = ".wav";
        string audioFilePath;

        BingSpeechApiClient bingSpeechClient;

        ObservableCollection<TodoItemViewModel> _items;
        public ObservableCollection<TodoItemViewModel> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new ObservableCollection<TodoItemViewModel>();
                    _items = TodoItems.Items;
                }

                return _items;
            }
            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        TodoItemViewModel _selectedItem;
        public TodoItemViewModel SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;

                if (_selectedItem == null)
                    return;

                UpdateTodoItemCommand.Execute(_selectedItem);

                SelectedItem = null;
            }
        }

        bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public ICommand SpeakItemCommand { get; private set; }

        public ICommand NewTodoItemCommand { get; private set; }

        public ICommand UpdateTodoItemCommand { get; private set; }

        public TodoItemListViewModel()
        {
            //recorder = new Plugin.AudioRecorder.AudioRecorderService()
            //{
            //    StopRecordingOnSilence = true,
            //    StopRecordingAfterTimeout = true,
            //    TotalAudioTimeout = TimeSpan.FromSeconds(10)
            //};

            SpeakItemCommand = new Command(SpeakItem);
            NewTodoItemCommand = new Command(AddNewItem);
            UpdateTodoItemCommand = new Command<TodoItemViewModel>(UpdateItem);
            
            bingSpeechClient = new BingSpeechApiClient("68c55c3ad7f1435f841ee128b903c822");

            //go fetch an auth token up front - this should decrease latecy on the first call. 
            //	Otherwise, this would be called automatically the first time I use the speech client
            Task.Run(() => bingSpeechClient.Authenticate());
        }

        static TodoItemListViewModel()
        {
            //fill up default items
            //Items = TodoItems.Items;
        }

        async void SpeakItem()
        {
            //change isSpeech to false if you want text interface.
            bool isSpeech = true;
            if (isSpeech)
            {
                try
                {
                    if (Device.OS == TargetPlatform.Android)
                    {
                        audioDirectoryPath = "/sdcard/";
                    }
                    else if (Device.OS == TargetPlatform.iOS)
                    {
                        audioDirectoryPath = "";
                    }

                    DependencyService.Get<IAudio>().StopAudio();
                    if (recording)
                    {
                        DependencyService.Get<IAudio>().StopRecording();

                        //set the selected recognition mode & profanity mode
                        bingSpeechClient.RecognitionMode = RecognitionMode.Interactive;
                        bingSpeechClient.ProfanityMode = ProfanityMode.Masked;

                        //if we're not streaming the audio as we're recording, we'll use the file-based STT API here
                        if (audioFilePath != null)
                        {

                            DependencyService.Get<IAudio>().PlayAudio(audioFilePath);
                            resultText = await SpeechToText(audioFilePath);
                        }

                        recording = !recording;

                        //TODO: Update UI to indicate recording is done
                    }
                    else
                    {
                        var dateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fff", System.Globalization.CultureInfo.InvariantCulture);
                        audioFilePath = audioDirectoryPath + dateTime + extension;

                        DependencyService.Get<IAudio>().StartRecording(audioFilePath);
                        recording = !recording;

                        //TODO: Update UI to indicate recording is in progress

                        return;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                resultText = await InputBox(); //simple text input
            }

            IsBusy = true;
            
            await DependencyService.Get<IBotConnection>().SendMessageAsync(resultText);

            var message = DependencyService.Get<IBotConnection>().GetMessagesAsync().Result;

            //Bot is directly sending detected entity in message, ideally it should be sent as parameters.
            TodoItemViewModel Item = new TodoItemViewModel() { Title = message, Text = "Today", IsDone = false };
            Items.Add(Item);

            IsBusy = false;
        }
        
        void AddNewItem()
        {
            TodoItemViewModel Item = new TodoItemViewModel();
            Items.Add(Item);
            Application.Current.MainPage.Navigation.PushModalAsync(new TodoItemDetail(Item));
        }

        void UpdateItem(TodoItemViewModel item)
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new TodoItemDetail(item));
        }

        public static Task<string> InputBox()
        {
            // wait in this proc, until user did his input 
            var tcs = new TaskCompletionSource<string>();

            var lblMessage = new Label { Text = "Type your message" };
            var txtInput = new Entry { Text = "" };

            var btnOk = new Button
            {
                Text = "Ok",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8),
            };
            btnOk.Clicked += async (s, e) =>
            {
                // close page
                var result = txtInput.Text;
                await Application.Current.MainPage.Navigation.PopModalAsync();
                // pass result
                tcs.SetResult(result);
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                WidthRequest = 100,
                BackgroundColor = Color.FromRgb(0.8, 0.8, 0.8)
            };
            btnCancel.Clicked += async (s, e) =>
            {
                // close page
                await Application.Current.MainPage.Navigation.PopModalAsync();
                // pass empty result
                tcs.SetResult(null);
            };

            var slButtons = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { btnOk, btnCancel },
            };

            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { lblMessage, txtInput, slButtons },
            };

            // create and show page
            var page = new ContentPage();
            page.Content = layout;
            Application.Current.MainPage.Navigation.PushModalAsync(page);
            // open keyboard
            txtInput.Focus();

            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
            return tcs.Task;
        }

        public async Task ShowActivityIndicator()
        {

            var ai = new ActivityIndicator { IsRunning = true, IsEnabled = true, IsVisible = true };
            
            var layout = new StackLayout
            {
                Padding = new Thickness(0, 40, 0, 0),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { ai },
            };

            // create and show page
            var page = new ContentPage();
            page.Content = layout;
            await Application.Current.MainPage.Navigation.PushModalAsync(page);

            // code is waiting her, until result is passed with tcs.SetResult() in btn-Clicked
            // then proc returns the result
            while(true)
            {
                if (IsBusy == false)
                {
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                    return;
                }
                await Task.Delay(1000);
            }
        }

        async Task<string> SpeechToText(string audioFile)
        {
            try
            {
                var speechResult = await bingSpeechClient.SpeechToTextSimple(audioFile);

                if (speechResult != null)
                {
                    var RecognitionStatus = speechResult.RecognitionStatus;
                    var DisplayText = speechResult.DisplayText;
                    var Offset = speechResult.Offset;
                    var Duration = speechResult.Duration;
                }

                return speechResult.DisplayText;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

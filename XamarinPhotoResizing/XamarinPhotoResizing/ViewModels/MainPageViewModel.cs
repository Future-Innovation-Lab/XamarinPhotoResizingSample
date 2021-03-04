using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using System.Windows.Input;
using FFImageLoading;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System;

namespace XamarinPhotoResizing.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private ICommand _photoCommand;
        public ICommand PhotoCommand => _photoCommand ?? (_photoCommand = new Command(TakePhoto));

        public ImageSource PhotoImage { get; private set; }

        public MainPageViewModel(INavigation navigationService)
            : base(navigationService)
        {

        }

        public async void TakePhoto()
        {
            var photoImage = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions { PhotoSize = PhotoSize.Full });  //you can also control image size from camera here
            PhotoImage = ImageSource.FromStream(() => photoImage.GetStream());

            //Resize to whatever you need
            var resizedPngStream = await ImageService.Instance.LoadStream(new Func<CancellationToken, Task<Stream>>((arg) => Task.FromResult<Stream>(photoImage.GetStream())))
     .DownSample(300,300,true)
     .AsPNGStreamAsync();

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "resizedpic.png");

            // This saves it as a file, but you could stream this to other things, like an API, DB etc
            ResizeAndSavePhoto(resizedPngStream, path);
        }

        public void ResizeAndSavePhoto(Stream stream, string path)
        {
            using (FileStream fileStream = File.Create(path, (int)stream.Length))
            {
                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, (int)bytesInStream.Length);
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
            }
        }
    }
}

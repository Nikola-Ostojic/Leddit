using ReactiveUI;

namespace Mobile.ViewModels.Movies
{
    public class MovieCellViewModel : ReactiveObject
    {

        private int _id;
        public int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set => this.RaiseAndSetIfChanged(ref _imageUrl, value);
        }

        public MovieCellViewModel(int id, string name, string imageUrl)
        {
            _id = id;
            _name = name;
            _imageUrl = imageUrl;
        }

    }
}

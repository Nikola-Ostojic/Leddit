using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.ViewModels.Thread
{
    public class CommentCellViewModel : ReactiveObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        private string _author;
        public string Author
        {
            get => _author;
            set => this.RaiseAndSetIfChanged(ref _author, value);
        }

        private string _content;
        public string Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }
        public CommentCellViewModel(int id, string author, string content)
        {
            _id = id;
            _author = author;
            _content = content;
        }
    }
}

using ReactiveUI;
using System;

namespace Mobile.ViewModels.Thread
{
    public class ThreadCellViewModel : ReactiveObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => this.RaiseAndSetIfChanged(ref _title, value);
        }

        private string _content;
        public string Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content, value);
        }

        private string _author;
        public string Author
        {
            get => _author;
            set => this.RaiseAndSetIfChanged(ref _author, value);
        }

        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => this.RaiseAndSetIfChanged(ref _createdAt, value);
        }

        private DateTime _modifiedAt;
        public DateTime ModifiedAt
        {
            get => _modifiedAt;
            set => this.RaiseAndSetIfChanged(ref _modifiedAt, value);
        }

        private int _commentsCount;
        public int CommentsCount
        {
            get => _commentsCount;
            set => this.RaiseAndSetIfChanged(ref _commentsCount, value);
        }

        public ThreadCellViewModel(int id, string title, string content, string author, DateTime createdAt, DateTime modifiedAt, int commentsCount)
        {
            _id = id;
            _title = title;
            _content = content;
            _author = author;
            _createdAt = createdAt;
            _modifiedAt = modifiedAt;
            _commentsCount = commentsCount;
        }
    }
}

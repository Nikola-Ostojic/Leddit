using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Mobile.Controls
{
    public class PageableListView : ListView
    {
        public static readonly BindableProperty LoadMoreCommandProperty =
           BindableProperty.Create(nameof(LoadMoreCommand), typeof(ICommand), typeof(PageableListView), default(ICommand));

        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }

        public static readonly BindableProperty PageSizeProperty =
            BindableProperty.Create(nameof(PageSize), typeof(int), typeof(PageableListView), 25);

        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        public PageableListView() : base(ListViewCachingStrategy.RecycleElement)
        {
            Observable
                .FromEventPattern<ItemVisibilityEventArgs>(
                    x => this.ItemAppearing += x,
                    x => this.ItemAppearing -= x)
                .SubscribeOn(RxApp.MainThreadScheduler)
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Select(
                    ev =>
                    {
                        if (this.ItemsSource is IEnumerable<object> items)
                        {
                            var itemIndex = items.ToList().IndexOf(ev.EventArgs.Item);
                            var itemCount = items.Count();

                            if (itemCount % PageSize == 0 && itemIndex == itemCount - 5 && LoadMoreCommand != null)
                            {
                                PageSize += 25;
                            }
                        }
                        return PageSize;
                    })
                .Where(offset => offset != -1)
                .DistinctUntilChanged()
                .InvokeCommand(this, x => x.LoadMoreCommand);
        }
    }
}

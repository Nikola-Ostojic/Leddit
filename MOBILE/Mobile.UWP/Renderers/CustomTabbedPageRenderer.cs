using Mobile.UWP.Renderers;
using Mobile.Views.BottomTabNavigation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(BottomTabNavigationView), typeof(CustomTabbedPageRenderer))]
namespace Mobile.UWP.Renderers
{
    public class CustomTabbedPageRenderer : TabbedPageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var view = (Element as TabbedPage);
                if (view != null)
                {
                    Control.Style = App.Current.Resources["TabbedPageStyle"] as Windows.UI.Xaml.Style;
                }
            }
        }
    }
}

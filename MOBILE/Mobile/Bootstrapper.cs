using Mobile.Core.Api;
using Mobile.Core.Api.Rest;
using Mobile.Core.Runtime;
using Mobile.Core.Services.AuthenticationService;
using Mobile.Core.Services.CommentService;
using Mobile.Core.Services.MoviesService;
using Mobile.Core.Services.Scheduler;
using Mobile.Core.Services.ThreadService;
using Mobile.ViewModels.BottomTabNavigation;
using Mobile.ViewModels.Login;
using Mobile.ViewModels.Movies;
using Mobile.ViewModels.Profile;
using Mobile.ViewModels.Thread;
using Mobile.Views.BottomTabNavigation;
using Mobile.Views.Login;
using Mobile.Views.Movies;
using Mobile.Views.Profile;
using Mobile.Views.Register;
using Mobile.Views.Threads;
using Sextant;
using Splat;

namespace Mobile
{
    public class Bootstrapper
    {
        internal void RegisterServices()
        {
            Locator.CurrentMutable.Register(() => new SchedulerService(), typeof(ISchedulerService));
            Locator.CurrentMutable.Register(() => new AuthenticationService(), typeof(IAuthenticationService));
            Locator.CurrentMutable.Register(() => new MoviesService(), typeof(IMoviesService));
            Locator.CurrentMutable.Register(() => new ApiService<IAuthApi>(), typeof(IApiService<IAuthApi>));
            Locator.CurrentMutable.Register(() => new ApiService<IMoviesApi>(), typeof(IApiService<IMoviesApi>));
            Locator.CurrentMutable.Register(() => new ApiService<IThreadApi>(), typeof(IApiService<IThreadApi>));
            Locator.CurrentMutable.Register(() => new ApiService<ICommentApi>(), typeof(IApiService<ICommentApi>));
            Locator.CurrentMutable.Register(() => new ThreadService(), typeof(IThreadService));
            Locator.CurrentMutable.Register(() => new CommentService(), typeof(ICommentService));
            Locator.CurrentMutable.Register(() => new RuntimeContext(), typeof(IRuntimeContext));
        }

        internal void RegisterViews()
        {
            SextantHelper.RegisterView<AutoLoginView, AutoLoginViewModel>();
            SextantHelper.RegisterView<LoginView, LoginViewModel>();
            SextantHelper.RegisterView<RegisterView, RegisterViewModel>();
            SextantHelper.RegisterView<MoviesView, MoviesViewModel>();
            SextantHelper.RegisterView<BottomTabNavigationView, BottomTabNavigationViewModel>();
            SextantHelper.RegisterView<ProfileView, ProfileViewModel>();
            SextantHelper.RegisterView<MovieDetailsView, MovieDetailsViewModel>();
            SextantHelper.RegisterView<AddMovieView, AddMovieViewModel>();
            SextantHelper.RegisterView<ThreadView, ThreadViewModel>();
            SextantHelper.RegisterView<AddThreadView, AddThreadViewModel>();
            SextantHelper.RegisterView<ThreadDetailsView, ThreadDetailsViewModel>();
        }
    }
}

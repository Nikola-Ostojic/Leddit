using DynamicData;
using Mobile.Core.Api;
using Mobile.Core.Api.Rest;
using Mobile.Core.Dtos;
using Mobile.Core.Dtos.Request;
using Mobile.Core.Extensions;
using Splat;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Mobile.Core.Services.MoviesService
{
    public class MoviesService : IMoviesService
    {
        private readonly IApiService<IMoviesApi> _moviesApi;

        private readonly Subject<MovieDTO> _movie = new Subject<MovieDTO>();
        public IObservable<MovieDTO> Movie => _movie.AsObservable();

        private readonly SourceCache<MovieDTO, int> _movies = new SourceCache<MovieDTO, int>(x => x.Id);
        public IObservableCache<MovieDTO, int> Movies => _movies;

        public MoviesService(IApiService<IMoviesApi> moviesApi = null)
        {
            _moviesApi = moviesApi ?? Locator.Current.GetService<IApiService<IMoviesApi>>();
        }

        public IObservable<Unit> Create(CreateMovieRequestDTO movie)
        {
            return _moviesApi.GetClient().CreateMovie(movie);
        }

        public IObservable<bool> Delete(int id)
        {
            return _moviesApi.GetClient().DeleteMovie(id).Select(x => true);
        }

        public IObservable<bool> Edit(MovieDTO movie)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> GetMovie(int id) =>
            _moviesApi.GetClient().FetchMovie(id).Select(movie =>
            {
                _movie.OnNext(movie);
                return Unit.Default;
            });

        public IObservable<Unit> GetMovies(string movieName, int itemsPerPage) =>
            _moviesApi.GetClient().FetchMovies(movieName, itemsPerPage).Select(result =>
            {
                _movies.EditDiff(result.Data);
                return Unit.Default;
            });
    }
}

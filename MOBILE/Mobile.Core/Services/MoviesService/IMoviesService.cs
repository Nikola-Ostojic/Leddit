using DynamicData;
using Mobile.Core.Dtos;
using Mobile.Core.Dtos.Request;
using System;
using System.Reactive;

namespace Mobile.Core.Services.MoviesService
{
    public interface IMoviesService
    {
        IObservable<MovieDTO> Movie { get; }
        IObservableCache<MovieDTO, int> Movies { get; }
        IObservable<Unit> GetMovies(string movieName, int itemsPerPage);
        IObservable<Unit> GetMovie(int id);
        IObservable<Unit> Create(CreateMovieRequestDTO movie);
        IObservable<bool> Delete(int id);
        IObservable<bool> Edit(MovieDTO movie);

    }
}

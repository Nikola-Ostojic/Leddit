using Mobile.Core.Dtos;
using Mobile.Core.Dtos.Request;
using Refit;
using System;
using System.Reactive;

namespace Mobile.Core.Api.Rest
{
    [Headers("Content-Type: application/json", "Accept: application/json")]
    public interface IMoviesApi
    {
        [Get("/movies/{id}")]
        [Headers("Authorization: Bearer")]
        IObservable<MovieDTO> FetchMovie(int id);

        [Get("/movies?movieName={movieName}&page=1&itemsPerPage={itemsPerPage}")]
        [Headers("Authorization: Bearer")]
        IObservable<PageableDTO<MovieDTO>> FetchMovies(string movieName, int itemsPerPage);

        [Delete("/movies/{id}")]
        [Headers("Authorization: Bearer")]
        IObservable<string> DeleteMovie(int id);


        [Post("/movies")]
        [Headers("Authorization: Bearer")]
        IObservable<Unit> CreateMovie([Body] CreateMovieRequestDTO movie);
    }
}

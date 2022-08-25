﻿using MovieProDemo.Data;
using MovieProDemo.Models.TMDB;

namespace MovieProDemo.Services.Interfaces;

public interface IRemoteMovieService
{
    Task<MovieDetail> MovieDetailAsync(int id);
    Task<MovieSearch> SearchMoviesAsync(MovieCategory category, int count);
    Task<ActorDetail> ActorDetailAsync(int id);
}

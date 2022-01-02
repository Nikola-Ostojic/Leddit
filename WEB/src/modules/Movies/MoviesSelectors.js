import { REDUCER_KEY } from "./MoviesConstants";

export const getMoviesData = (state) => state[REDUCER_KEY]?.data;
export const getMoviesError = (state) => state[REDUCER_KEY]?.error;
export const getMoviesLoading = (state) => state[REDUCER_KEY]?.fetching;

export const REDUCER_KEY = "movies";
export const FETCHING_MOVIES = `${REDUCER_KEY}.fetching.movies`;
export const FETCHING_MOVIES_SUCCESS = `${REDUCER_KEY}.fetching.movies.success`;
export const FETCHING_MOVIES_FAILURE = `${REDUCER_KEY}.fetching.movies.failure`;

export const INIT_STATE = {
  fetching: false,
  data: {},
  error: null,
  page: 1,
  totalPages: 1,
  totalItems: 0
};

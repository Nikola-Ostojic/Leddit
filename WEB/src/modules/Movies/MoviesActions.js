import { get } from "lodash";

import { apiCall } from "utils/ApiCall";

import {
  FETCHING_MOVIES_SUCCESS,
  FETCHING_MOVIES_FAILURE,
  FETCHING_MOVIES
} from "./MoviesConstants";

const fetchingMovies = () => ({
  type: FETCHING_MOVIES
});

const fetchingMoviesSuccess = (movies) => ({
  type: FETCHING_MOVIES_SUCCESS,
  payload: movies
});

const fetchingMoviesFailure = (error) => ({
  type: FETCHING_MOVIES_FAILURE,
  payload: error
});

export const fetchMovies = (
  movieName = "",
  page = 1,
  itemsPerPage = 5
) => async (dispatch, getState) => {
  dispatch(fetchingMovies());
  try {
    const { data } = await apiCall({
      url: `/movies?movieName=${movieName}&page=${page}&itemsPerPage=${itemsPerPage}`,
      method: "GET",
      requiresAuth: true
    });
    dispatch(fetchingMoviesSuccess(data));
  } catch (e) {
    const errorInfo = getErrorInfo(e);

    dispatch(fetchingMoviesFailure(errorInfo));
  }
};
const getErrorInfo = (error) =>
  get(error, "response.data") || get(error, "message") || error;

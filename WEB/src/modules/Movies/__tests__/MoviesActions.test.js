import { apiCall } from "utils/ApiCall";

import { fetchMovies } from "../MoviesActions";
import {
  FETCHING_MOVIES,
  FETCHING_MOVIES_SUCCESS,
  FETCHING_MOVIES_FAILURE
} from "../MoviesConstants";

jest.mock("utils/ApiCall");

describe("MoviesActions test", () => {
  it("Should dispatch FETCHING_MOVIES -> FETCHING_MOVIES_SUCCESS", async () => {
    // Arrange
    const dispatch = jest.fn();
    const movies = ["Movie One", "Movie two"];
    const movieName = "One";
    const page = 1;
    const itemsPerPage = 20;
    apiCall.mockImplementation(() => ({
      data: { movies }
    }));

    // Act
    await fetchMovies(movieName, page, itemsPerPage)(dispatch);

    // Assert
    expect(apiCall).toHaveBeenCalledWith({
      url: `/movies?movieName=${movieName}&page=${page}&itemsPerPage=${itemsPerPage}`,
      method: "GET",
      requiresAuth: true
    });
    expect(dispatch).toHaveBeenNthCalledWith(1, { type: FETCHING_MOVIES });
    expect(dispatch).toHaveBeenNthCalledWith(2, {
      type: FETCHING_MOVIES_SUCCESS,
      payload: {
        movies
      }
    });
  });

  it("Should dispatch FETCHING_MOVIES -> FETCHING_MOVIES_FAILURE login", async () => {
    // Arrange
    const dispatch = jest.fn();
    const error = "An error happened.";
    const movieName = "One";
    const page = 1;
    const itemsPerPage = 20;
    apiCall.mockImplementation(() => Promise.reject(error));

    // Act
    await fetchMovies(movieName, page, itemsPerPage)(dispatch);

    // Assert
    expect(dispatch).toHaveBeenNthCalledWith(1, { type: FETCHING_MOVIES });
    expect(dispatch).toHaveBeenNthCalledWith(2, {
      type: FETCHING_MOVIES_FAILURE,
      payload: error
    });
  });
});

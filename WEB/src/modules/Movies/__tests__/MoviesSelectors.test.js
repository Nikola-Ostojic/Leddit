import {
  getMoviesData,
  getMoviesError,
  getMoviesLoading
} from "../MoviesSelectors";
import { REDUCER_KEY } from "../MoviesConstants";

describe("MoviesConstants test", () => {
  describe("getMoviesData test", () => {
    it("Should be a function", () => {
      expect(typeof getMoviesData).toEqual("function");
    });

    it("should return movies from state", () => {
      const expectedResult = {
        movies: ["one", "Two"]
      };

      const state = {
        [REDUCER_KEY]: {
          data: expectedResult
        }
      };

      const result = getMoviesData(state);

      expect(result).toEqual(expectedResult);
    });
  });

  describe("getMoviesError test", () => {
    it("Should be a function", () => {
      expect(typeof getMoviesError).toEqual("function");
    });

    it("should return error from state", () => {
      const expectedResult = "An error happened";

      const state = {
        [REDUCER_KEY]: {
          error: expectedResult
        }
      };

      const result = getMoviesError(state);

      expect(result).toEqual(expectedResult);
    });
  });

  describe("getMoviesLoading test", () => {
    it("Should be a function", () => {
      expect(typeof getMoviesLoading).toEqual("function");
    });

    it("should return loading from state", () => {
      const expectedResult = true;

      const state = {
        [REDUCER_KEY]: {
          fetching: expectedResult
        }
      };

      const result = getMoviesLoading(state);

      expect(result).toEqual(expectedResult);
    });
  });
});

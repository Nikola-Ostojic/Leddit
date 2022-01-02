import MoviesReducer from "../MoviesReducer";
import {
  FETCHING_MOVIES,
  FETCHING_MOVIES_SUCCESS,
  FETCHING_MOVIES_FAILURE
} from "../MoviesConstants";

describe("MoviesReducer tests", () => {
  it("Should be a function", () => {
    expect(typeof MoviesReducer).toBe("function");
  });

  it("Should return an object", () => {
    expect(typeof MoviesReducer({}, {})).toBe("object");
  });

  it("Should return fetching:true case: FETCHING_MOVIES", () => {
    // Arrange
    const state = {
      fetching: false,
      error: null,
      data: {}
    };

    const action = { type: FETCHING_MOVIES };

    // Act
    const result = MoviesReducer(state, action);

    // Assert
    expect(result).toEqual({
      fetching: true,
      error: null,
      data: {}
    });
  });

  it("Should return fetching:false and movies case: FETCHING_MOVIES_SUCCESS", () => {
    const state = {
      fetching: true
    };

    const action = {
      type: FETCHING_MOVIES_SUCCESS,
      payload: {
        movies: ["One", "Two"]
      }
    };

    const result = MoviesReducer(state, action);

    expect(result).toEqual({
      fetching: false,
      data: action.payload,
      error: null
    });
  });

  it("Should return fetching: false, error:error, movies: null case: FETCHING_MOVIES_FAILURE", () => {
    const state = {
      fetching: true,
      error: null,
      data: {
        movies: ["One", "Two"]
      }
    };

    const action = {
      type: FETCHING_MOVIES_FAILURE,
      payload: "An error happened."
    };

    const result = MoviesReducer(state, action);

    expect(result).toEqual({
      fetching: false,
      data: {},
      error: action.payload
    });
  });
});

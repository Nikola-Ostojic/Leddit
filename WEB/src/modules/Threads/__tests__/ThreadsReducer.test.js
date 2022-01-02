import ThreadsReducer from "../ThreadsReducer";
import {
  FETCHING_THREADS,
  FETCHING_THREADS_SUCCESS,
  FETCHING_THREADS_FAILURE
} from "../ThreadsConstants";

describe("ThreadsReducer tests", () => {
  it("Should be a function", () => {
    expect(typeof ThreadsReducer).toBe("function");
  });

  it("Should return an object", () => {
    expect(typeof ThreadsReducer({}, {})).toBe("object");
  });

  it("Should return fetching:true case: FETCHING_THREADS", () => {
    // Arrange
    const state = {
      fetching: false,
      error: null,
      data: {}
    };

    const action = { type: FETCHING_THREADS };

    // Act
    const result = ThreadsReducer(state, action);

    // Assert
    expect(result).toEqual({
      fetching: true,
      error: null,
      data: {}
    });
  });

  it("Should return fetching:false and threads case: FETCHING_THREADS_SUCCESS", () => {
    const state = {
      fetching: true
    };

    const action = {
      type: FETCHING_THREADS_SUCCESS,
      payload: {
        threads: ["One", "Two"]
      }
    };

    const result = ThreadsReducer(state, action);

    expect(result).toEqual({
      fetching: false,
      data: action.payload,
      error: null
    });
  });

  it("Should return fetching: false, error:error, threads: null case: FETCHING_THREADS_FAILURE", () => {
    const state = {
      fetching: true,
      error: null,
      data: {
        threads: ["One", "Two"]
      }
    };

    const action = {
      type: FETCHING_THREADS_FAILURE,
      payload: "An error happened."
    };

    const result = ThreadsReducer(state, action);

    expect(result).toEqual({
      fetching: false,
      data: {},
      error: action.payload
    });
  });
});

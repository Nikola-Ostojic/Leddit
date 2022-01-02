import UserReducer from "../AuthReducer";
import {
  AUTH_IN_PROGRESS,
  AUTH_SUCCESS,
  AUTH_FAILURE,
  LOGOUT
} from "../AuthConstants";

describe("AuthReducer tests", () => {
  it("Should be a function", () => {
    expect(typeof UserReducer).toBe("function");
  });

  it("Should return an object", () => {
    expect(typeof UserReducer({}, {})).toBe("object");
  });

  it("Should return fetching:true case: AUTH_IN_PROGRESS", () => {
    // Arrange
    const state = {
      fetching: false,
      error: null
    };

    const action = { type: AUTH_IN_PROGRESS };

    // Act
    const result = UserReducer(state, action);

    // Assert
    expect(result).toEqual({
      fetching: true,
      error: null,
      user: null
    });
  });

  it("Should return fetching:false and user case: AUTH_SUCCESS", () => {
    const state = {
      fetching: true
    };

    const action = {
      type: AUTH_SUCCESS,
      payload: {
        username: "user",
        email: "user@levi9.com",
        role: "User"
      }
    };

    const result = UserReducer(state, action);

    expect(result).toEqual({
      fetching: false,
      user: action.payload,
      error: null
    });
  });

  it("Should return fetching: false, error:error, user: null case: AUTH_FAILURE", () => {
    const state = {
      fetching: true,
      error: null,
      user: {
        username: "user",
        email: "user@levi9.com",
        role: "User"
      }
    };

    const action = {
      type: AUTH_FAILURE,
      payload: "An error happened."
    };

    const result = UserReducer(state, action);

    expect(result).toEqual({
      fetching: false,
      user: null,
      error: action.payload
    });
  });

  it("Should set user to null and clear error on : LOGOUT", () => {
    const state = {
      fetching: true,
      error: "An error happened",
      user: {
        username: "user",
        email: "user@levi9.com",
        role: "User"
      }
    };

    const action = {
      type: LOGOUT
    };

    const result = UserReducer(state, action);

    expect(result).toEqual({
      fetching: false,
      user: null,
      error: null
    });
  });
});

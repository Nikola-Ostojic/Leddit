import JwtDecode from "jwt-decode";

import { apiCall } from "utils/ApiCall";

import { login, register, logout, checkUser, getToken } from "../AuthActions";
import { getUser } from "../AuthSelectors";
import {
  AUTH_FAILURE,
  AUTH_IN_PROGRESS,
  AUTH_SUCCESS,
  LOGOUT
} from "../AuthConstants";

jest.mock("utils/ApiCall");
jest.mock("jwt-decode");
jest.mock("../AuthSelectors.js");

describe("AuthActions test", () => {
  it("Should dispatch AUTH_IN_PROGRESS -> AUTH_SUCCESS, login", async () => {
    // Arrange
    const dispatch = jest.fn();

    const username = "TestUser";
    const password = "password@123";
    const email = "test@gmail.com";
    const roles = ["User"];

    JwtDecode.mockImplementation(() => ({
      username,
      sub: email,
      roles
    }));
    apiCall.mockImplementation(() => ({
      data: { accessToken: "accessToken", refreshToken: "refreshToken" }
    }));

    // Act
    await login(email, password)(dispatch);

    // Assert
    expect(apiCall).toHaveBeenCalledWith({
      url: "/auth/login",
      method: "POST",
      data: { password, email }
    });
    expect(dispatch).toHaveBeenNthCalledWith(1, { type: AUTH_IN_PROGRESS });
    expect(dispatch).toHaveBeenNthCalledWith(2, {
      type: AUTH_SUCCESS,
      payload: {
        username,
        email,
        role: roles[0]
      }
    });
  });

  it("Should dispatch AUTH_IN_PROGRESS -> AUTH_FAILURE login", async () => {
    // Arrange
    const dispatch = jest.fn();
    const error = "An error happened.";
    const expectedResult = {
      username: "user",
      sub: "email@email.com",
      roles: ["User"]
    };
    JwtDecode.mockImplementation(() => expectedResult);
    apiCall.mockImplementation(() => Promise.reject(error));

    // Act
    await login("user", "password")(dispatch);

    // Assert
    expect(dispatch).toHaveBeenNthCalledWith(1, { type: AUTH_IN_PROGRESS });
    expect(dispatch).toHaveBeenNthCalledWith(2, {
      type: AUTH_FAILURE,
      payload: error
    });
  });

  it("Should dispatch AUTH_IN_PROGRESS -> AUTH_SUCCESS register", async () => {
    // Arrange
    const dispatch = jest.fn();
    const username = "TestUser";
    const email = "test@gmail.com";
    const password = "password@123";
    const confirmPassword = "password@123";
    JwtDecode.mockImplementation(() => ({
      username,
      sub: email,
      roles: ["User"]
    }));
    apiCall.mockImplementation(() => ({
      data: { accessToken: "accessToken", refreshToken: "refreshToken" }
    }));

    // Act
    await register(username, email, password, confirmPassword)(dispatch);

    // Assert
    expect(apiCall).toHaveBeenCalledWith({
      url: "/auth/register",
      method: "POST",
      data: { username, email, password, confirmPassword }
    });
    expect(dispatch).toHaveBeenNthCalledWith(1, { type: AUTH_IN_PROGRESS });
    expect(dispatch).toHaveBeenNthCalledWith(2, {
      type: AUTH_SUCCESS,
      payload: {
        username,
        email,
        role: "User"
      }
    });
  });

  it("Should dispatch AUTH_IN_PROGRESS -> AUTH_FAILURE register", async () => {
    // Arrange
    const dispatch = jest.fn();
    const error = "An error happened.";
    apiCall.mockImplementation(() => Promise.reject(new Error(error)));
    const username = "TestUser";
    const email = "test@gmail.com";
    const password = "password@123";
    const confirmPassword = "password@123";

    // Act
    await register(username, email, password, confirmPassword)(dispatch);

    // Assert
    expect(apiCall).toHaveBeenCalledWith({
      url: "/auth/register",
      method: "POST",
      data: { username, email, password, confirmPassword }
    });
    expect(dispatch).toHaveBeenNthCalledWith(1, { type: AUTH_IN_PROGRESS });
    expect(dispatch).toHaveBeenNthCalledWith(2, {
      type: AUTH_FAILURE,
      payload: error
    });
  });

  it("Should dispatch AUTH_IN_PROGRESS -> AUTH_FAILURE register with error", async () => {
    // Arrange
    const dispatch = jest.fn();
    const error = "An error happened.";
    apiCall.mockImplementation(() => Promise.reject(error));
    const username = "TestUser";
    const email = "test@gmail.com";
    const password = "password@123";
    const confirmPassword = "password@123";

    // Act
    await register(username, email, password, confirmPassword)(dispatch);

    // Assert
    expect(apiCall).toHaveBeenCalledWith({
      url: "/auth/register",
      method: "POST",
      data: { username, email, password, confirmPassword }
    });
    expect(dispatch).toHaveBeenNthCalledWith(1, { type: AUTH_IN_PROGRESS });
    expect(dispatch).toHaveBeenNthCalledWith(2, {
      type: AUTH_FAILURE,
      payload: error
    });
  });

  it("Should clear local storage and dispatch LOGOUT when logout is called", async () => {
    // Arrange
    const localStorageClearSpy = jest.spyOn(Storage.prototype, "clear");
    const dispatch = jest.fn();

    // Act
    await logout()(dispatch);

    // Assert
    expect(apiCall).toHaveBeenCalledWith({
      url: "/auth/logout",
      method: "POST",
      requiresAuth: true
    });
    expect(localStorageClearSpy).toBeCalled();
    expect(dispatch).toHaveBeenCalledWith({ type: LOGOUT });
  });

  it("Should call logout if token expired", () => {
    const localStorageGetItemSpy = jest.spyOn(Storage.prototype, "getItem");
    const localStorageClearSpy = jest.spyOn(Storage.prototype, "clear");
    const dispatch = jest.fn((f) => f());
    localStorageGetItemSpy.mockImplementation(() => ({ jwtToken: "jwtToken" }));
    getUser.mockImplementation(() => "TestUser");
    JwtDecode.mockImplementation(() => ({ exp: 50000000 }));

    // Act
    dispatch(checkUser());

    // Assert
    expect(localStorageClearSpy).toBeCalled();
  });

  it("Should login user back in if the user is not logged in and the token did not expire", async () => {
    // Arrange
    const localStorageGetItemSpy = jest.spyOn(Storage.prototype, "getItem");
    const token = "Token";
    const getState = jest.fn();
    const dispatch = jest.fn((dispatched) => {
      if (typeof dispatched === "function") {
        dispatched(dispatch, getState);
      }
    });

    const username = "User";
    const email = "user@levi9.com";
    const roles = ["User"];

    localStorageGetItemSpy.mockImplementation(() => ({ accessToken: token }));
    JwtDecode.mockImplementation(() => ({
      exp: Date.now(),
      username,
      sub: email,
      roles
    }));
    getUser.mockImplementation(() => null);

    // Act
    await dispatch(checkUser());

    // Assert
    expect(dispatch).toHaveBeenCalledWith({
      type: AUTH_SUCCESS,
      payload: {
        username,
        email,
        role: roles[0]
      }
    });
  });

  it("Should return access token if it did not expire", async () => {
    // Arrange
    const localStorageGetItemSpy = jest.spyOn(Storage.prototype, "getItem");
    const token = "Token";
    localStorageGetItemSpy.mockImplementation(() => token);

    const getState = jest.fn();
    const dispatch = jest.fn((dispatched) => {
      if (typeof dispatched === "function") {
        dispatched(dispatch, getState);
      }
    });
    const cancel = jest.fn();
    // Act
    const result = await getToken(cancel)(dispatch);

    expect(result).toEqual(token);
  });

  it("Should call cancel if get token fails", async () => {
    // Arrange
    const localStorageGetItemSpy = jest.spyOn(Storage.prototype, "getItem");
    localStorageGetItemSpy.mockImplementation(() => {
      throw new Error("Failed");
    });

    const getState = jest.fn();
    const dispatch = jest.fn((dispatched) => {
      if (typeof dispatched === "function") {
        dispatched(dispatch, getState);
      }
    });
    const cancel = jest.fn();

    // Act
    await getToken(cancel)(dispatch);

    // Assert
    expect(cancel).toHaveBeenCalledWith("Failed");
  });
});

import JwtDecode from "jwt-decode";
import { get } from "lodash";

import { apiCall } from "utils/ApiCall";

import {
  AUTH_IN_PROGRESS,
  AUTH_SUCCESS,
  AUTH_FAILURE,
  LOGOUT
} from "./AuthConstants";

const authInProgress = () => ({
  type: AUTH_IN_PROGRESS
});

const authSuccess = (token) => ({
  type: AUTH_SUCCESS,
  payload: {
    username: token.username,
    email: token.sub,
    role: token.roles[0]
  }
});

const authFailure = (data) => ({
  type: AUTH_FAILURE,
  payload: data
});

export const getToken = (cancel) => async (dispatch) =>
  refreshToken(dispatch)
    .then(() => localStorage.getItem("accessToken"))
    .catch((e) => {
      const errorInfo = getErrorInfo(e);
      cancel(errorInfo);
      dispatch(logout());
    });
export const refreshToken = async (dispatch) => {
  const accessToken = localStorage.getItem("accessToken");
  const decodedToken = JwtDecode(accessToken);

  if (isAccessTokenValid(decodedToken)) {
    return accessToken;
  }

  const refreshToken = localStorage.getItem("refreshToken");
  try {
    const { data } = await apiCall({
      url: "/auth/token",
      method: "POST",
      data: { refreshToken }
    });
    setTokensInLocalStorage(data);
    return data.accessToken;
  } catch (e) {
    dispatch(logout());
  }
};

export const login = (email, password) => async (dispatch) => {
  try {
    dispatch(authInProgress());
    const { data } = await apiCall({
      url: "/auth/login",
      method: "POST",
      data: { email, password }
    });
    setTokensInLocalStorage(data);

    const decodedToken = JwtDecode(data.accessToken);
    dispatch(authSuccess(decodedToken));
  } catch (error) {
    const errorInfo = getErrorInfo(error);
    dispatch(authFailure(errorInfo));
  }
};

export const register = (username, email, password, confirmPassword) => async (
  dispatch
) => {
  try {
    dispatch(authInProgress());
    const { data } = await apiCall({
      url: "/auth/register",
      method: "POST",
      data: { username, email, password, confirmPassword }
    });
    setTokensInLocalStorage(data);
    const decodedToken = JwtDecode(data.accessToken);
    dispatch(authSuccess(decodedToken));
  } catch (error) {
    const errorInfo = getErrorInfo(error);
    dispatch(authFailure(errorInfo));
  }
};

export const logout = () => async (dispatch) => {
  try {
    await apiCall({
      url: "/auth/logout",
      method: "POST",
      requiresAuth: true
    });
  } catch {}
  localStorage.clear();
  dispatch({ type: LOGOUT });
};

export const checkUser = () => (dispatch) => {
  const token = localStorage.getItem("accessToken");
  if (!token) {
    return;
  }
  const decodedToken = JwtDecode(token);
  if (isAccessTokenValid(decodedToken)) {
    dispatch(authSuccess(decodedToken));
  } else {
    refreshToken()
      .then(() => {
        const decodedToken = JwtDecode(localStorage.getItem("accessToken"));
        dispatch(authSuccess(decodedToken));
      })
      .catch((e) => {
        dispatch(logout());
      });
  }
};

const isAccessTokenValid = (token) => token && token.exp > Date.now() / 1000;

const setTokensInLocalStorage = ({ accessToken, refreshToken }) => {
  localStorage.setItem("accessToken", accessToken);
  localStorage.setItem("refreshToken", refreshToken);
};

const getErrorInfo = (error) =>
  get(error, "response.data") || get(error, "message") || error;

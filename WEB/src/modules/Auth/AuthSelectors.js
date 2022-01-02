import { REDUCER_KEY } from "./AuthConstants";

export const getError = (state) => state[REDUCER_KEY].error;
export const getUser = (state) => state[REDUCER_KEY] && state[REDUCER_KEY].user;
export const getUsername = (state) =>
  getUser(state) && state[REDUCER_KEY].user.username;
export const getUserEmail = (state) =>
  getUser(state) && state[REDUCER_KEY].user.email;
export const getUserRole = (state) =>
  getUser(state) && state[REDUCER_KEY].user.role;

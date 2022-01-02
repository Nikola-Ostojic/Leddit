export const REDUCER_KEY = "auth";

export const AUTH_IN_PROGRESS = `${REDUCER_KEY}.in.progress`;
export const AUTH_SUCCESS = `${REDUCER_KEY}.success`;
export const AUTH_FAILURE = `${REDUCER_KEY}.failure`;
export const LOGOUT = `${REDUCER_KEY}.logout`;

export const INIT_STATE = {
  user: {
    username: null,
    email: null,
    role: null
  },
  fetching: false,
  error: null
};

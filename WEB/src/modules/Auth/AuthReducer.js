import {
  INIT_STATE,
  AUTH_IN_PROGRESS,
  AUTH_SUCCESS,
  AUTH_FAILURE,
  LOGOUT
} from "./AuthConstants";

export default (state = INIT_STATE, action) => {
  switch (action.type) {
    case AUTH_IN_PROGRESS:
      return {
        ...state,
        error: null,
        user: null,
        fetching: true
      };
    case AUTH_SUCCESS:
      return {
        ...state,
        user: action.payload,
        error: null,
        fetching: false
      };
    case AUTH_FAILURE:
      return {
        ...state,
        error: action.payload,
        user: null,
        fetching: false
      };
    case LOGOUT:
      return {
        ...state,
        error: null,
        user: null,
        fetching: false
      };
    default:
      return state;
  }
};

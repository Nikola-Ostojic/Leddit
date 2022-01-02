import {
  INIT_STATE,
  FETCHING_MOVIES,
  FETCHING_MOVIES_SUCCESS,
  FETCHING_MOVIES_FAILURE
} from "./MoviesConstants";

export default (state = INIT_STATE, action) => {
  switch (action.type) {
    case FETCHING_MOVIES:
      return {
        ...state,
        fetching: true,
        error: null
      };
    case FETCHING_MOVIES_SUCCESS: {
      return {
        ...state,
        error: null,
        fetching: false,
        data: action.payload
      };
    }
    case FETCHING_MOVIES_FAILURE: {
      return {
        ...state,
        data: {},
        fetching: false,
        error: action.payload
      };
    }
    default:
      return state;
  }
};

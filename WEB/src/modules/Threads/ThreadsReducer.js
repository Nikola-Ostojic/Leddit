import {
  INIT_STATE,
  FETCHING_THREADS,
  FETCHING_THREADS_SUCCESS,
  FETCHING_THREADS_FAILURE
} from "./ThreadsConstants";

export default (state = INIT_STATE, action) => {
  switch (action.type) {
    case FETCHING_THREADS:
      return {
        ...state,
        fetching: true,
        error: null
      };
    case FETCHING_THREADS_SUCCESS: {
      return {
        ...state,
        error: null,
        fetching: false,
        data: action.payload
      };
    }
    case FETCHING_THREADS_FAILURE: {
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

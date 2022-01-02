import { REDUCER_KEY as userReducerKey } from "modules/Auth/AuthConstants";
import userReducer from "modules/Auth/AuthReducer";

import { REDUCER_KEY as moviesReducerKey } from "modules/Movies/MoviesConstants";
import moviesReducer from "modules/Movies/MoviesReducer";

import { REDUCER_KEY as threadsReducerKey } from "modules/Threads/ThreadsConstants";
import threadsReducer from "modules/Threads/ThreadsReducer";

export default {
  [moviesReducerKey]: moviesReducer,
  [userReducerKey]: userReducer,
  [threadsReducerKey]: threadsReducer
};

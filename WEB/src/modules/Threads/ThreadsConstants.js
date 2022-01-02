export const REDUCER_KEY = "threads";

export const FETCHING_THREADS = `${REDUCER_KEY}.fetching.threads`;
export const FETCHING_THREADS_SUCCESS = `${REDUCER_KEY}.fetching.threads.success`;
export const FETCHING_THREADS_FAILURE = `${REDUCER_KEY}.fetching.threads.failure`;

export const INIT_STATE = {
  fetching: false,
  data: {},
  error: null,
  page: 1,
  totalPages: 1,
  totalItems: 0
};

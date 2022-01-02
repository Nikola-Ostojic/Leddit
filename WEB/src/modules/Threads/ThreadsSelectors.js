import { REDUCER_KEY } from "./ThreadsConstants";

export const getThreadsData = (state) => state[REDUCER_KEY]?.data;
export const getThreadsError = (state) => state[REDUCER_KEY]?.error;
export const getThreadsLoading = (state) => state[REDUCER_KEY]?.fetching;

import { get } from "lodash";

import { apiCall } from "utils/ApiCall";

import {
  FETCHING_THREADS_SUCCESS,
  FETCHING_THREADS_FAILURE,
  FETCHING_THREADS
} from "./ThreadsConstants";

const fetchingThreads = () => ({
  type: FETCHING_THREADS
});

const fetchingThreadsSuccess = (threads) => ({
  type: FETCHING_THREADS_SUCCESS,
  payload: threads
});

const fetchingThreadsFailure = (error) => ({
  type: FETCHING_THREADS_FAILURE,
  payload: error
});

export const fetchThreads = (
  searchCriteria = "",
  page = 1,
  itemsPerPage = 5
) => async (dispatch, getState) => {
  dispatch(fetchingThreads());
  try {
    const { data } = await apiCall({
      url: `/threads?searchCriteria=${searchCriteria}&page=${page}&itemsPerPage=${itemsPerPage}`,
      method: "GET"
    });
    dispatch(fetchingThreadsSuccess(data));
  } catch (e) {
    const errorInfo = getErrorInfo(e);

    dispatch(fetchingThreadsFailure(errorInfo));
  }
};
const getErrorInfo = (error) =>
  get(error, "response.data") || get(error, "message") || error;

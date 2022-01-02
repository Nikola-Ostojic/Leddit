import { apiCall } from "utils/ApiCall";

import { fetchThreads } from "../ThreadsActions";
import {
  FETCHING_THREADS,
  FETCHING_THREADS_SUCCESS,
  FETCHING_THREADS_FAILURE
} from "../ThreadsConstants";

jest.mock("utils/ApiCall");

describe("ThreadsActions test", () => {
  it("Should dispatch FETCHING_THREADS -> FETCHING_THREADS_SUCCESS", async () => {
    // Arrange
    const dispatch = jest.fn();
    const threads = ["Thread One", "Thread two"];
    const searchCriteria = "One";
    const page = 1;
    const itemsPerPage = 20;
    apiCall.mockImplementation(() => ({
      data: { threads }
    }));

    // Act
    await fetchThreads(searchCriteria, page, itemsPerPage)(dispatch);

    // Assert
    expect(apiCall).toHaveBeenCalledWith({
      url: `/threads?searchCriteria=${searchCriteria}&page=${page}&itemsPerPage=${itemsPerPage}`,
      method: "GET"
    });
    expect(dispatch).toHaveBeenNthCalledWith(1, { type: FETCHING_THREADS });
    expect(dispatch).toHaveBeenNthCalledWith(2, {
      type: FETCHING_THREADS_SUCCESS,
      payload: {
        threads
      }
    });
  });

  it("Should dispatch FETCHING_THREADS -> FETCHING_THREADS_FAILURE login", async () => {
    // Arrange
    const dispatch = jest.fn();
    const error = "An error happened.";
    const searchCriteria = "One";
    const page = 1;
    const itemsPerPage = 20;
    apiCall.mockImplementation(() => Promise.reject(error));

    // Act
    await fetchThreads(searchCriteria, page, itemsPerPage)(dispatch);

    // Assert
    expect(dispatch).toHaveBeenNthCalledWith(1, { type: FETCHING_THREADS });
    expect(dispatch).toHaveBeenNthCalledWith(2, {
      type: FETCHING_THREADS_FAILURE,
      payload: error
    });
  });
});

import {
  getThreadsData,
  getThreadsError,
  getThreadsLoading
} from "../ThreadsSelectors";
import { REDUCER_KEY } from "../ThreadsConstants";

describe("ThreadsConstants test", () => {
  describe("getThreadsData test", () => {
    it("Should be a function", () => {
      expect(typeof getThreadsData).toEqual("function");
    });

    it("should return threads from state", () => {
      const expectedResult = {
        threads: ["one", "Two"]
      };

      const state = {
        [REDUCER_KEY]: {
          data: expectedResult
        }
      };

      const result = getThreadsData(state);

      expect(result).toEqual(expectedResult);
    });
  });

  describe("getThreadsError test", () => {
    it("Should be a function", () => {
      expect(typeof getThreadsError).toEqual("function");
    });

    it("should return error from state", () => {
      const expectedResult = "An error happened";

      const state = {
        [REDUCER_KEY]: {
          error: expectedResult
        }
      };

      const result = getThreadsError(state);

      expect(result).toEqual(expectedResult);
    });
  });

  describe("getThreadsLoading test", () => {
    it("Should be a function", () => {
      expect(typeof getThreadsLoading).toEqual("function");
    });

    it("should return loading from state", () => {
      const expectedResult = true;

      const state = {
        [REDUCER_KEY]: {
          fetching: expectedResult
        }
      };

      const result = getThreadsLoading(state);

      expect(result).toEqual(expectedResult);
    });
  });
});

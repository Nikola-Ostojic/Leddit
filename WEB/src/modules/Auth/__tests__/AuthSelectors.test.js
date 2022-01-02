import { getUser, getError } from "../AuthSelectors";
import { REDUCER_KEY } from "../AuthConstants";

describe("AuthSelectors test", () => {
  describe("getUser test", () => {
    it("Should be a function", () => {
      expect(typeof getUser).toEqual("function");
    });

    it("should return user from state", () => {
      const expectedResult = "TestUser";

      const state = {
        [REDUCER_KEY]: {
          user: expectedResult
        }
      };

      const result = getUser(state);

      expect(result).toEqual(expectedResult);
    });
  });

  describe("getError test", () => {
    it("Should be a function", () => {
      expect(typeof getError).toEqual("function");
    });

    it("should return error from state", () => {
      const expectedResult = "An error happened";

      const state = {
        [REDUCER_KEY]: {
          error: expectedResult
        }
      };

      const result = getError(state);

      expect(result).toEqual(expectedResult);
    });
  });
});

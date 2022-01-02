import { mapStateToProps } from "../LoginContainer";

jest.mock("modules/Auth/AuthSelectors", () => ({
  getError: () => "An error happened"
}));

describe("LoginContainer tests", () => {
  it("Should be a function", () => {
    expect(typeof mapStateToProps).toEqual("function");
  });

  it("should return an object", () => {
    expect(typeof mapStateToProps({})).toEqual("object");
  });

  it("should have property errors", () => {
    const props = mapStateToProps({});

    expect(props).toHaveProperty("error");
    expect(props.error).toEqual("An error happened");
  });
});

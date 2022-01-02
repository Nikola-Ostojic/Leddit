import { mapStateToProps } from "../AuthPageContainer";

describe("AuthContainer tests", () => {
  it("Should be a function", () => {
    expect(typeof mapStateToProps).toEqual("function");
  });

  it("should return an object", () => {
    expect(typeof mapStateToProps({})).toEqual("object");
  });

  it("should have property values", () => {
    const props = mapStateToProps({});
    expect(props).toHaveProperty("user");
  });
});

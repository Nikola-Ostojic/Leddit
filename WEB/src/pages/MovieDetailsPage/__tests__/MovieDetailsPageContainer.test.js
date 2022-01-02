import { mapStateToProps } from "../MovieDetailsPageContainer";

jest.mock("modules/Auth/AuthSelectors", () => ({
  getUserRole: () => "User"
}));

describe("MovieDetailsPageContainer tests", () => {
  it("Should be a function", () => {
    expect(typeof mapStateToProps).toEqual("function");
  });

  it("should return an object", () => {
    expect(typeof mapStateToProps({})).toEqual("object");
  });

  it("should have property role", () => {
    const props = mapStateToProps({});

    expect(props).toHaveProperty("role");
    expect(props.role).toEqual("User");
  });
});

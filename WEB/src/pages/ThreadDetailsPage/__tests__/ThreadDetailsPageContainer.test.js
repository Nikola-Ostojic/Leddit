import { mapStateToProps } from "../ThreadDetailsPageContainer";

jest.mock("modules/Auth/AuthSelectors", () => ({
  getUserRole: () => "User",
  getUsername: () => "User 1"
}));

describe("ThreadDetailsPageContainer tests", () => {
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

    expect(props).toHaveProperty("user");
    expect(props.user).toEqual("User 1");
  });
});

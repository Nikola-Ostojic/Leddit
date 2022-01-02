import { mapStateToProps } from "../MoviesPageContainer";

jest.mock("modules/Auth/AuthSelectors", () => ({
  getUserRole: () => "User"
}));
jest.mock("modules/Movies/MoviesSelectors", () => ({
  getMoviesData: () => ["one", "two"],
  getMoviesError: () => null,
  getMoviesLoading: () => false
}));

describe("AppRoutesContainer tests", () => {
  it("Should be a function", () => {
    expect(typeof mapStateToProps).toEqual("function");
  });
  it("should return an object", () => {
    expect(typeof mapStateToProps({})).toEqual("object");
  });
  it("should have property user role", () => {
    const props = mapStateToProps({});

    expect(props).toHaveProperty("userRole");
    expect(props.userRole).toEqual("User");
  });
  it("should have property loading", () => {
    const props = mapStateToProps({});

    expect(props).toHaveProperty("loading");
    expect(props.loading).toEqual(false);
  });
  it("should have property error", () => {
    const props = mapStateToProps({});

    expect(props).toHaveProperty("error");
    expect(props.error).toEqual(null);
  });
  it("should have property", () => {
    const props = mapStateToProps({});

    expect(props).toHaveProperty("moviesData");
    expect(props.moviesData).toEqual(["one", "two"]);
  });
});

import { mapStateToProps } from "../HomePageContainer";

jest.mock("modules/Auth/AuthSelectors", () => ({
  getUsername: () => "Username"
}));

jest.mock("modules/Threads/ThreadsSelectors", () => ({
  getThreadsData: () => "Data",
  getThreadsError: () => "Error",
  getThreadsLoading: () => "Loading"
}));

describe("HomePageContainer tests", () => {
  it("Should be a function", () => {
    expect(typeof mapStateToProps).toEqual("function");
  });

  it("should return an object", () => {
    expect(typeof mapStateToProps({})).toEqual("object");
  });

  it("should have property user", () => {
    const props = mapStateToProps({});

    expect(props).toHaveProperty("user");
    expect(props.user).toEqual("Username");
  });

  it("should have property threads data", () => {
    const props = mapStateToProps({});

    expect(props).toHaveProperty("threadsData");
    expect(props.threadsData).toEqual("Data");
  });

  it("should have property error", () => {
    const props = mapStateToProps({});

    expect(props).toHaveProperty("error");
    expect(props.error).toEqual("Error");
  });

  it("should have property loading", () => {
    const props = mapStateToProps({});

    expect(props).toHaveProperty("loading");
    expect(props.loading).toEqual("Loading");
  });
});

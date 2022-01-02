import React from "react";
import { shallow } from "enzyme";

import AppNavigation from "../AppNavigation";

describe("AppNavigation tests", () => {
  const mockedProps = {
    checkUser: jest.fn()
  };
  it("Should match snapshot", () => {
    const wrapper = shallow(<AppNavigation {...mockedProps} />);
    expect(wrapper).toMatchSnapshot();
  });

  it("Should call check user after mount", () => {
    shallow(<AppNavigation {...mockedProps} />);
    expect(mockedProps.checkUser).toHaveBeenCalled();
  });
});

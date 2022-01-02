import React from "react";
import { shallow } from "enzyme";

import { Navbar } from "../Navbar";

describe("Navbar tests", () => {
  const mockedProps = {
    user: "user",
    logout: jest.fn(),
    setDrawerOpen: jest.fn(),
    drawerOpen: false
  };
  it("Should match snapshot with user defined", () => {
    const wrapper = shallow(<Navbar {...mockedProps} />);

    expect(wrapper).toMatchSnapshot();
  });

  it("Should match snapshot with undefined user", () => {
    const wrapper = shallow(<Navbar {...mockedProps} user={null} />);

    expect(wrapper).toMatchSnapshot();
  });
});

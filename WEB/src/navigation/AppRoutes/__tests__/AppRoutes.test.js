import React from "react";
import { shallow } from "enzyme";

import AppRoutes from "../AppRoutes";

describe("AppRoutes test", () => {
  it("Should match snapshot", () => {
    const wrapper = shallow(<AppRoutes />);
    expect(wrapper).toMatchSnapshot();
  });
});

import React from "react";
import { shallow } from "enzyme";

import HomePage from "../HomePage";

describe("HomePage tests", () => {
  it("Should match snapshot", () => {
    const wrapper = shallow(<HomePage />);

    expect(wrapper).toMatchSnapshot();
  });
});

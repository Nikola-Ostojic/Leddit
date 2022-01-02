import React from "react";
import { shallow } from "enzyme";

import CreateMovie from "../CreateMovie";

describe("CreateMovie component tests", () => {
  it("Should match snapshot", () => {
    const wrapper = shallow(<CreateMovie />);

    expect(wrapper).toMatchSnapshot();
  });
});

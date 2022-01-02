import React from "react";
import { shallow } from "enzyme";

import CreateComment from "../CreateComment";

describe("CreateComment component tests", () => {
  it("Should match snapshot", () => {
    const wrapper = shallow(<CreateComment />);

    expect(wrapper).toMatchSnapshot();
  });
});

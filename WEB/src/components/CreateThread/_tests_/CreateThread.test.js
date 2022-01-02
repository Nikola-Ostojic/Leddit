import React from "react";
import { shallow } from "enzyme";

import CreateThread from "../CreateThread";

describe("CreateThread component tests", () => {
  it("Should match snapshot", () => {
    const wrapper = shallow(<CreateThread />);

    expect(wrapper).toMatchSnapshot();
  });
});

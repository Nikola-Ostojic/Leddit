import { shallow } from "enzyme";
import React from "react";

import App from "../App";

describe("App tests", () => {
  it("Should match snapshot", () => {
    const wrapper = shallow(<App />);

    expect(wrapper).toMatchSnapshot();
  });
});

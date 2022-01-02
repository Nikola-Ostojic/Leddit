import React from "react";
import { shallow } from "enzyme";

import MoviesPage from "../MoviesPage";

describe("MoviesPage tests", () => {
  it("Should match snapshot", () => {
    const wrapper = shallow(<MoviesPage />);

    expect(wrapper).toMatchSnapshot();
  });
});

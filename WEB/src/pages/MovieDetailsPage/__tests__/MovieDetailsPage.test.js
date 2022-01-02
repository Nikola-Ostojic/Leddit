import React from "react";
import { shallow } from "enzyme";

import MovieDetailsPage from "../MovieDetailsPage";

describe("MovieDetailsPage tests", () => {
  it("Should match snapshot", () => {
    const wrapper = shallow(<MovieDetailsPage />);

    expect(wrapper).toMatchSnapshot();
  });
});

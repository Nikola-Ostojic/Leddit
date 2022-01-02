import React from "react";
import { shallow } from "enzyme";

import ThreadDetailsPage from "../ThreadDetailsPage";

describe("ThreadDetailsPage tests", () => {
  it("Should match snapshot", () => {
    const wrapper = shallow(<ThreadDetailsPage />);

    expect(wrapper).toMatchSnapshot();
  });
});

import React from "react";
import { shallow } from "enzyme";

import { NotFoundPage } from "../NotFoundPage";

describe("NotFoundPage component tests", () => {
  it("Should match snapshot", () => {
    const mockedProps = {
      location: {
        pathname: "Not existing route"
      }
    };
    const wrapper = shallow(<NotFoundPage {...mockedProps} />);

    expect(wrapper).toMatchSnapshot();
  });
});

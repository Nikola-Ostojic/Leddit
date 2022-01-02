import React from "react";
import { shallow } from "enzyme";

import { Card } from "@material-ui/core";

import MovieItem from "../MovieItem";

describe("MovieItem component tests", () => {
  const mockedProps = {
    movie: {
      name: "Test"
    },
    onClick: jest.fn()
  };
  it("Should match snapshot", () => {
    const wrapper = shallow(<MovieItem {...mockedProps} />);

    expect(wrapper).toMatchSnapshot();
  });

  it("Should call on click when item is clicked", () => {
    const wrapper = shallow(<MovieItem {...mockedProps} />);

    wrapper.find(Card).simulate("click");

    expect(mockedProps.onClick).toHaveBeenCalled();
  });
});

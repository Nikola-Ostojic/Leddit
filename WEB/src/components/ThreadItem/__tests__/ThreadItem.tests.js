import React from "react";
import { shallow } from "enzyme";

import { Card } from "@material-ui/core";

import ThreadItem from "../ThreadItem";

describe("ThreadItem component tests", () => {
  const mockedProps = {
    thread: {
      title: "Test",
      content: "Test"
    },
    onClick: jest.fn()
  };
  it("Should match snapshot", () => {
    const wrapper = shallow(<ThreadItem {...mockedProps} />);

    expect(wrapper).toMatchSnapshot();
  });

  it("Should call on click when item is clicked", () => {
    const wrapper = shallow(<ThreadItem {...mockedProps} />);

    wrapper.find(Card).simulate("click");

    expect(mockedProps.onClick).toHaveBeenCalled();
  });
});

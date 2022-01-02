import React from "react";
import { shallow } from "enzyme";
import { Button } from "@material-ui/core";
import Select from "@material-ui/core/Select";

import CommentsFilter from "../CommentsFilter";

describe("CommentsFilter component tests", () => {
  const mockedProps = {
    totalPages: 3,
    currentPage: 2,
    setCurrentPage: jest.fn(),
    itemsPerPage: 10,
    setItemsPerPage: jest.fn()
  };
  it("Should match snapshot", () => {
    const wrapper = shallow(<CommentsFilter {...mockedProps} />);

    expect(wrapper).toMatchSnapshot();
  });

  it("Should decrement currentPage on first button click", () => {
    const wrapper = shallow(<CommentsFilter {...mockedProps} />);

    wrapper.find(Button).at(0).simulate("click");

    expect(mockedProps.setCurrentPage).toHaveBeenCalledWith(
      mockedProps.currentPage - 1
    );
  });

  it("Should increment currentPage on second button click", () => {
    const wrapper = shallow(<CommentsFilter {...mockedProps} />);

    wrapper.find(Button).at(1).simulate("click");

    expect(mockedProps.setCurrentPage).toHaveBeenCalledWith(
      mockedProps.currentPage + 1
    );
  });

  it("Should call setItemsPerPage on dropdown change", () => {
    const wrapper = shallow(<CommentsFilter {...mockedProps} />);
    const eventMock = {
      target: {
        value: 20
      }
    };
    wrapper.find(Select).simulate("change", eventMock);

    expect(mockedProps.setItemsPerPage).toHaveBeenCalledWith(
      eventMock.target.value
    );
  });
});

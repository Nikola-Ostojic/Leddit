import React from "react";
import { shallow } from "enzyme";
import { Button, TextField } from "@material-ui/core";
import Select from "@material-ui/core/Select";

import ThreadsFilter from "../ThreadsFilter";

describe("ThreadsFilter component tests", () => {
  const mockedProps = {
    totalPages: 3,
    searchCriteria: "Test",
    setThreadTitleContent: jest.fn(),
    currentPage: 2,
    setCurrentPage: jest.fn(),
    itemsPerPage: 10,
    setItemsPerPage: jest.fn()
  };
  it("Should match snapshot", () => {
    const wrapper = shallow(<ThreadsFilter {...mockedProps} />);

    expect(wrapper).toMatchSnapshot();
  });

  it("Should trigger set thread title and content on text field on change event", () => {
    const wrapper = shallow(<ThreadsFilter {...mockedProps} />);
    const eventMock = {
      target: {
        value: mockedProps.searchCriteria
      }
    };
    wrapper.find(TextField).simulate("change", eventMock);

    expect(mockedProps.setThreadTitleContent).toHaveBeenCalledWith(
      mockedProps.searchCriteria
    );
  });

  it("Should decrement currentPage on first button click", () => {
    const wrapper = shallow(<ThreadsFilter {...mockedProps} />);

    wrapper.find(Button).at(0).simulate("click");

    expect(mockedProps.setCurrentPage).toHaveBeenCalledWith(
      mockedProps.currentPage - 1
    );
  });

  it("Should increment currentPage on second button click", () => {
    const wrapper = shallow(<ThreadsFilter {...mockedProps} />);

    wrapper.find(Button).at(1).simulate("click");

    expect(mockedProps.setCurrentPage).toHaveBeenCalledWith(
      mockedProps.currentPage + 1
    );
  });

  it("Should call setItemsPerPage on dropdown change", () => {
    const wrapper = shallow(<ThreadsFilter {...mockedProps} />);
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

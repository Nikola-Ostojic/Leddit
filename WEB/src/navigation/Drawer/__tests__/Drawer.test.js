import React from "react";
import { shallow } from "enzyme";
import MaterialDrawer from "@material-ui/core/Drawer";
import ListItem from "@material-ui/core/ListItem";

import { Drawer } from "../Drawer";

describe("Drawer component tests", () => {
  const mockedProps = {
    drawerOpen: true,
    setDrawerOpen: jest.fn(),
    history: { push: jest.fn() }
  };
  it("Should match snapshot", () => {
    const wrapper = shallow(<Drawer {...mockedProps} />);

    expect(wrapper).toMatchSnapshot();
  });

  it("Should call setDrawerOpen when material drawer is pressed", () => {
    const wrapper = shallow(<Drawer {...mockedProps} />);

    wrapper.find(MaterialDrawer).simulate("close");
    expect(mockedProps.setDrawerOpen).toHaveBeenCalledWith(
      !mockedProps.drawerOpen
    );
  });

  it("Should call push to home on first list item click", () => {
    const wrapper = shallow(<Drawer {...mockedProps} />);

    wrapper.find(ListItem).at(0).simulate("click");
    expect(mockedProps.history.push).toHaveBeenCalledWith("");
  });

  it("Should call push to movies on second list item click", () => {
    const wrapper = shallow(<Drawer {...mockedProps} />);

    wrapper.find(ListItem).at(1).simulate("click");
    expect(mockedProps.history.push).toHaveBeenCalledWith("/movies");
  });

  it("Should call setDrawerOpen with false when the presentation div is clicked", () => {
    const wrapper = shallow(<Drawer {...mockedProps} />);

    wrapper.find('[role="presentation"]').simulate("click");
    expect(mockedProps.setDrawerOpen).toHaveBeenCalledWith(false);
  });
});

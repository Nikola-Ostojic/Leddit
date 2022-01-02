import React from "react";
import { shallow } from "enzyme";

import { Route } from "react-router-dom";

import PrivateRoute from "../PrivateRoute";

const TestComponent = () => <div></div>;

describe("PrivateRoute test", () => {
  const mockedNavProps = { location: "/auth" };
  it("Should match snapshot when user does not exists", () => {
    const wrapper = shallow(<PrivateRoute component={TestComponent} />)
      .find(Route)
      .renderProp("render")(mockedNavProps);
    expect(wrapper).toMatchSnapshot();
  });

  it("Should match snapshot when user does exists", () => {
    const wrapper = shallow(
      <PrivateRoute component={TestComponent} user="test" />
    )
      .find(Route)
      .renderProp("render")(mockedNavProps);
    expect(wrapper).toMatchSnapshot();
  });
});

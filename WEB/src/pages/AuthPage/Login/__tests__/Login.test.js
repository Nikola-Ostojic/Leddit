import React from "react";
import { shallow } from "enzyme";
import { TextField } from "@material-ui/core";

import { Login } from "../Login";

describe("Login component tests", () => {
  it("Should match snapshot", () => {
    const wrapper = shallow(<Login />);

    expect(wrapper).toMatchSnapshot();
  });

  it("Should match snapshot with error", () => {
    expect(shallow(<Login error="An error happened" />)).toMatchSnapshot();
  });

  it("Should call login if the required fields are filled out in the form", () => {
    // Arrange
    const login = jest.fn();
    const wrapper = shallow(<Login login={login} />);
    const email = "Test@gmail.com";
    const password = "Test";

    // Act
    wrapper.setState({ email, password });

    const form = wrapper.find("form");
    form.simulate("submit", {
      preventDefault: jest.fn()
    });

    // Assert
    expect(login).toBeCalledWith(email, password);
  });

  it("Should onChange happen handleChange should appropriately update the state", () => {
    // Arrange
    const wrapper = shallow(<Login />);
    const changeValue = "user@levi9.com";
    const changeEvent = { target: { value: changeValue, name: "email" } };
    const setStateSpy = jest.spyOn(wrapper.instance(), "setState");

    // Act
    const emailInputField = wrapper.find(TextField).at(0);
    emailInputField.simulate("change", changeEvent);

    // Assert
    expect(setStateSpy).toBeCalledWith({ email: changeValue });
    expect(wrapper.state()).toEqual({
      password: "",
      email: changeValue
    });
  });
});

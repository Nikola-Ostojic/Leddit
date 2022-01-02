import React from "react";
import { shallow } from "enzyme";
import { TextField } from "@material-ui/core";

import { Register } from "../Register";

describe("Register component tests", () => {
  it("Should match snapshot", () => {
    const wrapper = shallow(<Register />);

    expect(wrapper).toMatchSnapshot();
  });

  it("Should call register if the required fields are filled out in the form", () => {
    // Arrange
    const register = jest.fn();
    const wrapper = shallow(<Register register={register} />);
    const username = "Test";
    const email = "Test@gmail.com";
    const password = "Test";
    const confirmPassword = "Test";

    // Act
    wrapper.setState({ username, email, password, confirmPassword });
    const form = wrapper.find("form");
    form.simulate("submit", {
      preventDefault: jest.fn()
    });

    // Assert
    expect(register).toBeCalledWith(username, email, password, confirmPassword);
  });

  it("Should not call register if password and confirm password do not match", () => {
    // Arrange
    const register = jest.fn();
    const wrapper = shallow(<Register register={register} />);
    const username = "Test";
    const email = "Test@gmail.com";
    const password = "Test";
    const confirmPassword = "Test123";
    const setStateSpy = jest.spyOn(wrapper.instance(), "setState");

    // Act
    wrapper.setState({ username, email, password, confirmPassword });
    const form = wrapper.find("form");
    form.simulate("submit", {
      preventDefault: jest.fn()
    });

    // Assert
    expect(register).not.toBeCalled();
    expect(setStateSpy).toBeCalledWith({
      validationError: "Password and confirm password don't match"
    });
  });

  it("Should onChange happen handleChange should appropriately update the state", () => {
    // Arrange
    const wrapper = shallow(<Register />);
    const changeValue = "Test123";
    const changeEvent = { target: { value: changeValue, name: "username" } };
    const setStateSpy = jest.spyOn(wrapper.instance(), "setState");

    // Act
    const usernameInputField = wrapper.find(TextField).at(0);
    usernameInputField.simulate("change", changeEvent);

    // Assert
    expect(setStateSpy).toBeCalledWith({ username: changeValue });
    expect(wrapper.state()).toEqual({
      username: changeValue,
      password: "",
      confirmPassword: "",
      email: "",
      validationError: null
    });
  });
});

import React from "react";
import { shallow } from "enzyme";
import { Button } from "@material-ui/core";

import { Auth } from "../AuthPage";

// Note: enzyme doesn't support testing functional stateful components using useState hooks
describe("AuthPage test", () => {
  it("Should match snapshot when showLogin is true", () => {
    const wrapper = shallow(<Auth />);

    const btnTxt = wrapper.find(Button).text();

    expect(btnTxt).toEqual("Don't have an account? Register.");
    expect(wrapper).toMatchSnapshot();
  });

  it("Should match snapshot when showLogin is false", () => {
    const wrapper = shallow(<Auth />);

    const btnTxtBeforeSwitchingToRegistration = wrapper.find(Button).text();
    expect(btnTxtBeforeSwitchingToRegistration).toEqual(
      "Don't have an account? Register."
    );

    wrapper.find(Button).simulate("click");

    const btnTxtAfterSwitchingToRegistration = wrapper.find(Button).text();
    expect(btnTxtAfterSwitchingToRegistration).toEqual(
      "Already have an account? Login."
    );

    expect(wrapper).toMatchSnapshot();
  });

  // it("Should match snapshot when the user is already logged in", () => {
  //   const user = "TestUser";
  //   const wrapper = shallow(<Auth user={user} />);

  //   const message = wrapper.find("p").text();

  //   expect(message).toEqual(`You are already logged in ${user}`);
  //   expect(wrapper).toMatchSnapshot();
  // });
});

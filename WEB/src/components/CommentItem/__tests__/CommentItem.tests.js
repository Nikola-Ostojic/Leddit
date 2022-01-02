import React from "react";
import { shallow } from "enzyme";

import CommentItem from "../CommentItem";

describe("CommentItem component tests", () => {
  const mockedProps = {
    comment: {
      content: "Test"
    }
  };
  it("Should match snapshot", () => {
    const wrapper = shallow(<CommentItem {...mockedProps} />);

    expect(wrapper).toMatchSnapshot();
  });
});

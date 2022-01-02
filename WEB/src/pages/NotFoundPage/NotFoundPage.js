import React from "react";
import { Container } from "@material-ui/core";
import { withRouter } from "react-router-dom";

export const NotFoundPage = ({ location: { pathname } }) => (
  <Container>Page not found url: {pathname}</Container>
);

export default withRouter(NotFoundPage);

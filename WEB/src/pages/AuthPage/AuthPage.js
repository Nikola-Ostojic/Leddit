import React, { useState, memo } from "react";
import { Redirect } from "react-router-dom";
import { Container, Button, Box } from "@material-ui/core";

import Login from "./Login";
import Register from "./Register";

export const Auth = ({ user }) => {
  const [showLogin, setShowLogin] = useState(true);

  if (user) {
    return <Redirect to="/" />;
  }

  return (
    <>
      <Box m={10}>
        <Container>
          {showLogin ? <Login /> : <Register />}
          <br />
          <Button
            onClick={() => setShowLogin(!showLogin)}
            variant="contained"
            color="primary"
          >
            {showLogin
              ? "Don't have an account? Register."
              : "Already have an account? Login."}
          </Button>
        </Container>
      </Box>
    </>
  );
};

export default memo(
  Auth,
  ({ user: previousUser }, { user: nextUser }) => previousUser === nextUser
);

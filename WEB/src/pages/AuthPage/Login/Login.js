import React from "react";
import { withRouter } from "react-router-dom";
import { Button, TextField } from "@material-ui/core";

export class Login extends React.Component {
  state = {
    password: "",
    email: ""
  };

  handleChange = (e) => {
    const { name, value } = e.target;
    this.setState({ [name]: value });
  };

  submit = (e) => {
    e.preventDefault();
    const { email, password } = this.state;
    const { login } = this.props;

    login(email, password);
  };

  render() {
    const { email, password } = this.state;
    const { error } = this.props;
    return (
      <div>
        <h3>Login</h3>
        <p>To make our lives easier</p>
        <p>user@levi9.com -- User</p>
        <p>admin@levi9.com -- Admin</p>
        <form onSubmit={this.submit}>
          <TextField
            fullWidth
            margin="normal"
            name="email"
            type="email"
            required
            label="Email"
            onChange={this.handleChange}
            value={email}
          />
          <TextField
            fullWidth
            margin="normal"
            name="password"
            type="password"
            required
            onChange={this.handleChange}
            label="Password"
            value={password}
          />
          <br />
          <br />
          <Button color="primary" variant="contained" type="submit">
            Login
          </Button>
          {error && <p>{error}</p>}
        </form>
      </div>
    );
  }
}

export default withRouter(Login);

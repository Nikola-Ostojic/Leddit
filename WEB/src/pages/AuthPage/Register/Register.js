import React from "react";
import { Button, TextField } from "@material-ui/core";

export class Register extends React.Component {
  state = {
    username: "",
    email: "",
    password: "",
    confirmPassword: "",
    validationError: null
  };

  handleChange = (e) => {
    const { name, value } = e.target;
    this.setState({ [name]: value });
  };

  register = (e) => {
    e.preventDefault();
    const { username, email, password, confirmPassword } = this.state;
    const { register } = this.props;
    if (password !== confirmPassword) {
      this.setState({
        validationError: "Password and confirm password don't match"
      });
      return;
    }
    register(username, email, password, confirmPassword);
  };
  render() {
    const {
      username,
      email,
      password,
      confirmPassword,
      validationError
    } = this.state;
    const { error } = this.props;

    return (
      <div>
        <h3>Register</h3>
        <form onSubmit={this.register} method="POST">
          <TextField
            fullWidth
            margin="normal"
            name="username"
            label="Username"
            type="text"
            required
            onChange={this.handleChange}
            value={username}
          />
          <TextField
            fullWidth
            margin="normal"
            name="email"
            label="Email"
            type="email"
            required
            onChange={this.handleChange}
            value={email}
          />
          <TextField
            fullWidth
            margin="normal"
            name="password"
            type="password"
            label="Password"
            required
            onChange={this.handleChange}
            value={password}
          />
          <TextField
            fullWidth
            margin="normal"
            name="confirmPassword"
            type="password"
            label="Confirm password"
            required
            onChange={this.handleChange}
            value={confirmPassword}
          />
          <br />
          <br />
          <Button variant="contained" color="primary" type="submit">
            Register
          </Button>
        </form>
        {error && error}
        {validationError && validationError}
      </div>
    );
  }
}

export default Register;

import { connect } from "react-redux";

import { getError } from "modules/Auth/AuthSelectors";
import { login } from "modules/Auth/AuthActions";

import Login from "./Login";

export const mapStateToProps = (state) => ({
  error: getError(state)
});

export default connect(mapStateToProps, { login })(Login);

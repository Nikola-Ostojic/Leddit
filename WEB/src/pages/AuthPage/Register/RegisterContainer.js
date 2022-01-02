import { connect } from "react-redux";

import { getError } from "modules/Auth/AuthSelectors";
import { register } from "modules/Auth/AuthActions";

import Register from "./Register";

export const mapStateToProps = (state) => ({
  error: getError(state)
});

export default connect(mapStateToProps, { register })(Register);

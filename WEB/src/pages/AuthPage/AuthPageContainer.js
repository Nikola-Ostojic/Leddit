import { connect } from "react-redux";

import { getUsername } from "modules/Auth/AuthSelectors";

import Auth from "./AuthPage";

export const mapStateToProps = (state) => ({
  user: getUsername(state)
});

export default connect(mapStateToProps)(Auth);

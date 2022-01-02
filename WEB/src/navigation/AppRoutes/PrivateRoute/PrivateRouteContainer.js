import { connect } from "react-redux";

import { getUsername } from "modules/Auth/AuthSelectors";

import PrivateRoute from "./PrivateRoute";

export const mapStateToProps = (state) => ({
  user: getUsername(state)
});

export default connect(mapStateToProps)(PrivateRoute);

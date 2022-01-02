import { connect } from "react-redux";

import { getUsername } from "modules/Auth/AuthSelectors";
import { logout } from "modules/Auth/AuthActions";

import Navbar from "./Navbar";

export const mapStateToProps = (state) => ({
  user: getUsername(state)
});

export default connect(mapStateToProps, { logout })(Navbar);

import { connect } from "react-redux";

import { getUserRole, getUsername } from "modules/Auth/AuthSelectors";

import ThreadDetailsPage from "./ThreadDetailsPage";

export const mapStateToProps = (state) => ({
  role: getUserRole(state),
  user: getUsername(state)
});

export default connect(mapStateToProps)(ThreadDetailsPage);

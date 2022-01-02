import { connect } from "react-redux";

import { getUserRole } from "modules/Auth/AuthSelectors";

import MovieDetailsPage from "./MovieDetailsPage";

export const mapStateToProps = (state) => ({
  role: getUserRole(state)
});

export default connect(mapStateToProps)(MovieDetailsPage);

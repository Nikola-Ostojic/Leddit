import { connect } from "react-redux";

import { getToken, checkUser } from "modules/Auth/AuthActions";

import AppNavigation from "./AppNavigation";

export default connect(null, { getToken, checkUser })(AppNavigation);

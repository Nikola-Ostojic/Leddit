import React from "react";
import { BrowserRouter } from "react-router-dom";

import axiosInstance, { cancelTokenSource } from "utils/ApiCall";
import Navbar from "navigation/Navbar";
import Drawer from "navigation/Drawer";
import AppRoutes from "navigation/AppRoutes";

class AppNavigation extends React.Component {
  state = {
    drawerOpen: false
  };

  componentDidMount() {
    const { checkUser, getToken } = this.props;
    checkUser();
    axiosInstance.interceptors.request.use(async (config) => {
      if (config.requiresAuth) {
        config.cancelToken = cancelTokenSource.token;
        const token = await getToken(cancelTokenSource.cancel);
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    });
  }

  render() {
    const { drawerOpen } = this.state;
    return (
      <BrowserRouter>
        <Navbar
          drawerOpen={drawerOpen}
          setDrawerOpen={(open) => this.setState({ drawerOpen: open })}
        />
        <Drawer
          drawerOpen={drawerOpen}
          setDrawerOpen={(open) => this.setState({ drawerOpen: open })}
        />
        <AppRoutes />
      </BrowserRouter>
    );
  }
}
export default AppNavigation;

import React from "react";
import { Provider } from "react-redux";

import { ToastContainer } from "react-toastify";

import AppNavigation from "navigation/AppNavigation";
import "react-toastify/dist/ReactToastify.css";
import store from "store";

export const App = () => (
  <Provider store={store}>
    <AppNavigation />
    <ToastContainer autoClose={2500} />
  </Provider>
);

export default App;

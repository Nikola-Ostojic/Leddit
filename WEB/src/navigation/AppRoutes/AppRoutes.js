import React from "react";
import { Switch, Route } from "react-router-dom";

import HomePage from "pages/HomePage";
import MoviesPage from "pages/MoviesPage";
import CreateThreadPage from "pages/CreateThreadPage";
import MovieDetailsPage from "pages/MovieDetailsPage";
import ThreadDetailsPage from "pages/ThreadDetailsPage";
import NotFoundPage from "pages/NotFoundPage";
import AuthPage from "pages/AuthPage";
import "./AppRoutes.css";

import PrivateRoute from "../AppRoutes/PrivateRoute";

const AppRoutes = () => (
  <div className="content">
    <Switch>
      <Route exact path="/" component={HomePage} />
      <Route path="/threadDetails/:id?" component={ThreadDetailsPage} />
      <PrivateRoute path="/movies" component={MoviesPage} />
      <PrivateRoute path="/movieDetails/:id?" component={MovieDetailsPage} />
      <PrivateRoute path="/createThread" component={CreateThreadPage} />
      <Route path="/auth" component={AuthPage} />
      <Route path="*" component={NotFoundPage} />
    </Switch>
  </div>
);

export default AppRoutes;

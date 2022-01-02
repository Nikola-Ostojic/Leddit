import { connect } from "react-redux";

import { fetchMovies } from "modules/Movies/MoviesActions";
import {
  getMoviesData,
  getMoviesError,
  getMoviesLoading
} from "modules/Movies/MoviesSelectors";
import { getUserRole } from "modules/Auth/AuthSelectors";

import MoviesPage from "./MoviesPage";

export const mapStateToProps = (state) => ({
  moviesData: getMoviesData(state),
  error: getMoviesError(state),
  loading: getMoviesLoading(state),
  userRole: getUserRole(state)
});

export default connect(mapStateToProps, { fetchMovies })(MoviesPage);

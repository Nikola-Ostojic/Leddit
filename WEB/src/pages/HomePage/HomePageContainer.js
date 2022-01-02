import { connect } from "react-redux";

import { getUsername } from "modules/Auth/AuthSelectors";

import { fetchThreads } from "modules/Threads/ThreadsActions";

import {
  getThreadsData,
  getThreadsError,
  getThreadsLoading
} from "modules/Threads/ThreadsSelectors";

import HomePage from "./HomePage";

export const mapStateToProps = (state) => ({
  user: getUsername(state),
  threadsData: getThreadsData(state),
  error: getThreadsError(state),
  loading: getThreadsLoading(state)
});

export default connect(mapStateToProps, { fetchThreads })(HomePage);

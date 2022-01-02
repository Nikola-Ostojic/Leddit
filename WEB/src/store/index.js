import { applyMiddleware, createStore, combineReducers } from "redux";
import thunk from "redux-thunk";

import reducers from "./reducers";

const rootReducer = combineReducers(reducers);

export default createStore(rootReducer, applyMiddleware(...[thunk]));

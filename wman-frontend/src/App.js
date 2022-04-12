import React from "react";
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect,
} from "react-router-dom";
import jwt_decode from "jwt-decode";

import { NormalLoginForm } from "./components/Login/login.component.jsx";
import Cookies from "js-cookie";
import PrintEvents from "./components/Print-functionality/Print-events/print-events.component.jsx";
import WorkerPage from "./components/Worker-page/worker-page.component.jsx";
import CalendarListComponent from "./components/Manager/calendar-list.component.jsx";
import AdminPage from './components/Admin-page/admin-page.component';

const axios = require("axios").default;

const ProtectedRoute = ({ component: Component, ...rest }) => {
  const token = Cookies.get("auth");
  if (token) {
    axios.defaults.headers.common["Authorization"] = "Bearer " + token;
    const decoded = jwt_decode(token);
    const username = decoded.sub;
    return (
      <Route {...rest} render={(props) => <Component {...rest} {...props} username={username} />} />
    );
  }
  return <Redirect to="/login" />;
};

function App() {
  return (
    <Router>
      <Switch>
        <Route path="/login" component={NormalLoginForm} />
        <ProtectedRoute path="/print" component={PrintEvents} />
        <ProtectedRoute path="/worker" component={WorkerPage} />
        <ProtectedRoute path="/admin" component={AdminPage} />
        <ProtectedRoute path="/calendar" component={CalendarListComponent} />
        <ProtectedRoute path="/" component={CalendarListComponent} />
      </Switch>
    </Router>
  );
}

export default App;

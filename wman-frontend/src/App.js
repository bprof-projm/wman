import React from "react";
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect,
} from "react-router-dom";
import { NormalLoginForm } from "./components/login/login.component.jsx"
import { Logout } from "./components/logout/logout.component.jsx";
import Cookies from "js-cookie";
import MyCalendar from "./components/manager/calendar.component.jsx";

const axios = require("axios").default;

const ProtectedRoute = ({ component: Component, ...rest }) => {
  const token = Cookies.get("auth");
  if (token) {
    axios.defaults.headers.common["Authorization"] = "Bearer " + token;
    return (
      <Route {...rest} render={(props) => <Component {...rest} {...props} />} />
    );
  }
  return <Redirect to="/login" />;
};

function App() {
  return (
    <Router>
      <Switch>
        <Route path="/login">
          <NormalLoginForm />
        </Route>
        <Route path="/teszt">
          <MyCalendar />
        </Route>         
          <ProtectedRoute path="/" component={Logout} />
          <ProtectedRoute path="/Calendar" component={MyCalendar} />
      </Switch>
    </Router>
  );
}

export default App;

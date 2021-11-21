import React from "react";
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect,
} from "react-router-dom";
import { NormalLoginForm } from "./components/Login/login.component.jsx"
import { Logout } from "./components/Logout/logout.component.jsx";
import Cookies from "js-cookie";
import { TempPage } from "./components/DemoPage/temp.component.jsx";
import PrintEvents from "./components/Print-functionality/Print-events/print-events.component.jsx";

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
        <Route path="/login" component={NormalLoginForm}/>
        <ProtectedRoute path="/print" component={PrintEvents}/>       
        <ProtectedRoute path="/" component={TempPage} />      
      </Switch>
    </Router>
  );
}

export default App;

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
import WorkerPage from "./components/Worker-page/base-worker-page/worker-page.component.jsx";
import DocumentToPrint from "./components/Worker-page/To-print/document-to-print.component.jsx";

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
        <Route path="/print" component={DocumentToPrint}/>        
        <Route path="/tempPage" component ={TempPage} />        
        <Route path="/worker" component={WorkerPage} />          
        <ProtectedRoute path="/" component={Logout} />      
      </Switch>
    </Router>
  );
}

export default App;

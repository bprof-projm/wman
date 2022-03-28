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

import travel01 from "./components/assets/excel-with-phone.jpg";
import travel02 from "./components/assets/businesswoman.jpg";
import travel03 from "./components/assets/big-data.jpg";

import Hero from "./components/LandingPage/hero";
import Slider from "./components/LandingPage/slider";
import Navbar from "./components/LandingPage/navbar";

const axios = require("axios").default;

const ProtectedRoute = ({ component: Component, ...rest }) => {
  const token = Cookies.get("auth");
  if (token) {
    axios.defaults.headers.common["Authorization"] = "Bearer " + token;
    const decoded = jwt_decode(token);
    const username = decoded.sub;
    return (
      <Route
        {...rest}
        render={(props) => (
          <Component {...rest} {...props} username={username} />
        )}
      />
    );
  }
  return <Redirect to="/login" />;
};

const navbarLinks = [
  { url: "#", title: "Home" },
  { url: "#about", title: "About" },
  { url: "#services", title: "Services" },
  { url: "/login", title: "Login" },
];

const LandingPage = () => {
  return (
    <div className="App">
      <Navbar navbarLinks={navbarLinks} />
      <Hero imageSrc={travel03} />
      <Slider
        imageSrc={travel01}
        title={"Fuck you!"}
        subtitle={
          "valami valami valami valami valami valami valami valami valami valami valami valami valami valami valami valami valami valami valami valami"
        }
        id="about"
      />
      <Slider
        imageSrc={travel02}
        title={"Fuck off!"}
        subtitle={"valami valami valami valami valami"}
        flipped={true}
        id="services"
      />
    </div>
  );
};

function App() {
  return (
    <Router>
      <Switch>
        <Route path="/" component={LandingPage} exact />
        <Route path="/login" component={NormalLoginForm} />
        <ProtectedRoute path="/print" component={PrintEvents} />
        <ProtectedRoute path="/worker" component={WorkerPage} />
        <ProtectedRoute path="/calendar" component={CalendarListComponent} />
        <ProtectedRoute path="/" component={CalendarListComponent} />
      </Switch>
    </Router>
  );
}

export default App;

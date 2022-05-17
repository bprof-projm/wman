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
        title={"About Us"}
        subtitle={
          "We first started out as a school project back in 2021, but with everyone's passion put into the initial Wman software, we couldn't let it be just another school project. We continued to refine each part of the whole platform and eventually started adding exciting new features. Today we have the trust of hundreds of companies throughout the world and we hope that we can convince you to do the same after trying out our free demo now!"
        }
        id="about"
      />
      <Slider
        imageSrc={travel02}
        title={"Services"}
        subtitle={"We offer a solution for your and your company's organizing challenges with our workforce management software. Our main focus was to create an out-of-the-box platform to make life easier for everyone in your company. Wman includes a platform for admins to check their workforce's data, managers can create events on an easy-to-use calendar-like interface and workers have access to a mobile application to check their work schedule, give indication about their start and finish of a particular job and in the end upload pictures as their proof of work. Moreover we have throughout notification and statitsics generation systems to assisst everyone."}
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
        <ProtectedRoute path="/admin" component={AdminPage} />
        <ProtectedRoute path="/calendar" component={CalendarListComponent} />
        <ProtectedRoute path="/" component={CalendarListComponent} />
      </Switch>
    </Router>
  );
}

export default App;

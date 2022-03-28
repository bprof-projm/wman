import React from "react";
import Hero from "./hero";
import Slider from "./slider";
import Navbar from "./navbar";
import { BrowserRouter } from "react-router-dom";
import travel01 from "../assets/excel-with-phone.jpg";
import travel02 from "../assets/businesswoman.jpg";
import travel03 from "../assets/big-data.jpg";

const navbarLinks = [
  { url: "#", title: "Home" },
  { url: "#about", title: "About" },
  { url: "#services", title: "Services" },
  { url: "/login", title: "Login" },
];

function LandingPage() {
  return (
    <BrowserRouter>
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
    </BrowserRouter>
  );
}

export default LandingPage;

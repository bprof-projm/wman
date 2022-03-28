import React from "react";
import "./hero.css";

const Hero = ({ imageSrc }) => {
  return (
    <div className="hero">
      <img src={imageSrc} alt="valami" className="hero__image" />
      <h1 className="hero__title">Your choice for workforce management</h1>
    </div>
  );
};

export default Hero;

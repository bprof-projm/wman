import React from "react";
import "./slider.css";
import { useInView } from "react-intersection-observer";

const Slider = ({ imageSrc, title, subtitle, flipped, id }) => {
  const { ref, inView } = useInView({
    threshold: 0.4,
  });

  const renderContent = () => {
    if (!flipped) {
      return (
        <>
          <img src={imageSrc} alt="valami" className="slider__image" />
          <div className="slider__content">
            <h1 className="slider__title">{title}</h1>
            <p className="slider__pcontent">{subtitle}</p>
          </div>
        </>
      );
    } else {
      return (
        <>
          <div className="slider__content">
            <h1 className="slider__title">{title}</h1>
            <p className="slider__pcontent">{subtitle}</p>
          </div>
          <img src={imageSrc} alt="valami" className="slider__image" />
        </>
      );
    }
  };

  return (
    <div
      className={inView ? "slider slider--zoom" : "slider"}
      ref={ref}
      id={id}
    >
      {renderContent()}
    </div>
  );
};

export default Slider;

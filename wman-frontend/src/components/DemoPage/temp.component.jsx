import React from "react";
import { Logout } from "../Logout/logout.component";
import PrintButton from "../Print-functionality/Print-button/print-button.component";
import ProgressMenu from "../Worker-load/Progress-menu/progress-menu.component";
import LabelsMenu from "../Labels/LabelMenu/labelMenu";
import "./temp.styles.css";

export const TempPage = () => {
  return (
    <div className="temp">
      <h1>Ideiglenes oldal bemutatáshoz</h1>
      <ProgressMenu /> <LabelsMenu />
      <br />
      <PrintButton />
      <br />
      <Logout />
    </div>
  );
};

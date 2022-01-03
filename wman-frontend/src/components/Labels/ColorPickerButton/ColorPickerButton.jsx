import React, { useState } from "react";
import { ChromePicker } from "react-color";
import { Form, Input, Button, Drawer, Radio, Space } from "antd";
import "./ColorPickerButton.css";

const ColorPickerButton = (props) => {
  const [displayColorPicker, setDisplayColorPicker] = useState(false);

  const handleClick = () => {
    setDisplayColorPicker(!displayColorPicker);
  };

  const handleClose = () => {
    setDisplayColorPicker(false);
  };

  return (
    <div>
      <Button className="colorPickBtn" onClick={handleClick}>
        Pick Color
      </Button>
      {displayColorPicker ? (
        <div className="popover">
          <div className="cover" onClick={handleClose} />
          <ChromePicker
            color={props.color}
            onChangeComplete={props.onChangeComplete}
          />
        </div>
      ) : null}
    </div>
  );
};

export default ColorPickerButton;

import React from "react";
import { useHistory } from "react-router-dom";
import { Button } from "antd";
import { PrinterOutlined } from "@ant-design/icons";
const PrintButton = () => {
  const history = useHistory();

  function handleClick() {
    window.open("/print");
  }

  return (
    <div>
      <Button
        type="text"
        icon={<PrinterOutlined />}
        text="Print"
        onClick={handleClick}
      >
        Print
      </Button>
    </div>
  );
};
export default PrintButton;

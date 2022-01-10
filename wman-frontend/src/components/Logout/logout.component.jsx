import Cookies from "js-cookie";
import React from "react";
import { useHistory } from "react-router-dom";
import "./logout.styles.css";
import { Button } from "antd";
import { LogoutOutlined } from "@ant-design/icons";

export const Logout = () => {
  const history = useHistory();

  function logOut() {
    Cookies.remove("auth");
    history.push("/login");
  }

  return (
    // <div className='logout'>
    //     <button onClick={logOut}>Log Out</button>
    // </div>
    <Button
      type="primary"
      shape="round"
      icon={<LogoutOutlined />}
      size="medium"
      onClick={logOut}
    >
      Logout
    </Button>
  );
};

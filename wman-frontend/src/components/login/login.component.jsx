import React, { useState } from "react";
import "antd/dist/antd.css";
import "./login.styles.css";
import { useHistory } from "react-router-dom";
import { Form, Input, Button, Layout } from "antd";
import { UserOutlined, LockOutlined } from "@ant-design/icons";
import Cookies from "js-cookie";
import axios from "axios";

var inFifteenMinutes = new Date(new Date().getTime() + 60 * 60 * 1000);

export const NormalLoginForm = () => {
  const [failedLogin, setFailedLogin] = useState(false);

  const history = useHistory();

  const onFinish = (values) => {
    axios
      .put("/Auth/login", {
        loginName: values.username,
        password: values.password,
      })
      .then((response) => {
        Cookies.set("auth", response.data.token, {
          expires: inFifteenMinutes,
        });
        console.log(response.data.token);
        history.push("/");
      })
      .catch(function (error) {
        setFailedLogin(true);
        console.log(error);
      });
  };

  const onFinishFailed = (errorInfo) => {
    console.log("Failed:", errorInfo);
  };

  return (
    <div className="login" style={{ height: "100vh" }}>
      <div className="logo-login">Wman</div>
      <div className="login-container">
        <Form
          name="normal_login"
          className="login-form"
          initialValues={{
            remember: true,
          }}
          onFinish={onFinish}
          onFinishFailed={onFinishFailed}
        >
          <Form.Item
            name="username"
            rules={[
              {
                required: true,
                message: "Please input your Username!",
              },
            ]}
          >
            <Input
              className="form-item"
              prefix={<UserOutlined className="site-form-item-icon" />}
              placeholder="Username"
            />
          </Form.Item>
          <Form.Item
            name="password"
            rules={[
              {
                required: true,
                message: "Please input your Password!",
              },
            ]}
          >
            <Input
              className="form-item"
              prefix={<LockOutlined className="site-form-item-icon" />}
              type="password"
              placeholder="Password"
            />
          </Form.Item>
          <Form.Item>
            <Button
              type="primary"
              htmlType="submit"
              className="login-form-button"
            >
              Log in
            </Button>
          </Form.Item>
          <Form.Item style={{ margin: "0px" }}>
            {failedLogin && (
              <p style={{ color: "red", fontSize: "16px" }}>
                Wrong username or password
              </p>
            )}
          </Form.Item>
        </Form>
      </div>
    </div>
  );
};

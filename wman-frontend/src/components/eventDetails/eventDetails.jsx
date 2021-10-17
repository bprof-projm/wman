import axios from "axios";
import React, { useState } from "react";
import { Form, Input, Button, Select, DatePicker } from "antd";
import "antd/dist/antd.css";
import "./eventDetails.css";

const { RangePicker } = DatePicker;
const rangeConfig = {
  rules: [{ type: "array", required: true, message: "Please select time!" }],
};
const EventDetails = () => {
  const [componentSize, setComponentSize] = useState("default");

  return (
    <div className="card-container">
      <div className="card">
        <Form
          labelCol={{
            span: 4,
          }}
          wrapperCol={{
            span: 14,
          }}
          layout="horizontal"
          initialValues={{
            size: componentSize,
          }}
          size={componentSize}
        >
          <Form.Item label="Description">
            <Input />
          </Form.Item>

          <Form.Item label="Workers">
            <Select>
              <Select.Option value="demo">Demo</Select.Option>
            </Select>
          </Form.Item>
          <Form.Item label="Labels">
            <Select>
              <Select.Option value="demo">Demo</Select.Option>
            </Select>
          </Form.Item>
          <Form.Item
            name="range-time-picker"
            label="RangePicker"
            {...rangeConfig}
          >
            <RangePicker showTime format="YYYY-MM-DD HH:mm:ss" />
          </Form.Item>

          <Form.Item label="Button">
            <Button>Button</Button>
          </Form.Item>
        </Form>
      </div>
    </div>
  );
};
export default EventDetails;

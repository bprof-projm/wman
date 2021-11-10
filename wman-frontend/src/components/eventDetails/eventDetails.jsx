import React, { useState } from "react";
import { Form, Input, Button, Select, DatePicker } from "antd";
import "antd/dist/antd.css";
import "./eventDetails.css";

const { RangePicker } = DatePicker;
const axios = require("axios").default;
const rangeConfig = {
  rules: [{ type: "array", required: true, message: "Please select time!" }],
};
const EventDetails = (props) => {
  const [componentSize, setComponentSize] = useState("default");
  const [description, setDescription] = useState(null);
  const [city, setCity] = useState(null);
  const [street, setStreet] = useState(null);
  const [zipCode, setZipCode] = useState(null);
  const [buildingNumber, setBuildingNumber] = useState(null);
  const [floorDoor, setFloorDoor] = useState(null);
  const [startDate, setStartDate] = useState(null);
  const [endDate, setEndDate] = useState(null);

  //todo
  if (!props) {
    const data = axios.get(`/CalendarEvent/WorkCard/${props}`);

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
            <Form.Item label="Description" name="Description">
              <Input />
            </Form.Item>

            <Form.Item label="Workers" name="workers">
              <Select>
                <Select.Option value="demo">Demo</Select.Option>
              </Select>
            </Form.Item>
            <Form.Item label="Labels" name="labels">
              <Select>
                <Select.Option value="demo">Demo</Select.Option>
              </Select>
            </Form.Item>
            <Form.Item name="rangePicker" label="RangePicker" {...rangeConfig}>
              <showTime format="YYYY-MM-DD HH:mm:ss" />
            </Form.Item>

            <Form.Item label="Address"></Form.Item>
            <Form.Item label="City" name="city">
              <Input />
            </Form.Item>
            <Form.Item label="Street" name="street">
              <Input />
            </Form.Item>
            <Form.Item label="Zip Code" name="zipCode">
              <Input />
            </Form.Item>
            <Form.Item label="Building Number" name="buildingNumber">
              <Input />
            </Form.Item>
            <Form.Item label="Floor and Door" name="floorDoor">
              <Input />
            </Form.Item>

            <Form.Item label="Button">
              <Button>Button</Button>
            </Form.Item>
          </Form>
        </div>
      </div>
    );
  } else {
    const onFinish = (e) => {
      console.log(e);
      console.log("geci");
    };

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
            onFinish={onFinish}
          >
            <Form.Item label="Description" name="Description">
              <Input />
            </Form.Item>

            <Form.Item label="Workers" name="workers">
              <Select>
                <Select.Option value="demo">Demo</Select.Option>
              </Select>
            </Form.Item>
            <Form.Item label="Labels" name="labels">
              <Select>
                <Select.Option value="demo">Demo</Select.Option>
              </Select>
            </Form.Item>
            <Form.Item name="rangePicker" label="RangePicker" {...rangeConfig}>
              <showTime format="YYYY-MM-DD HH:mm:ss" />
            </Form.Item>

            <Form.Item label="Address"></Form.Item>
            <Form.Item label="City" name="city">
              <Input />
            </Form.Item>
            <Form.Item label="Street" name="street">
              <Input />
            </Form.Item>
            <Form.Item label="Zip Code" name="zipCode">
              <Input />
            </Form.Item>
            <Form.Item label="Building Number" name="buildingNumber">
              <Input />
            </Form.Item>
            <Form.Item label="Floor and Door" name="floorDoor">
              <Input />
            </Form.Item>

            <Form.Item label="Button">
              <Button>Button</Button>
            </Form.Item>
          </Form>
        </div>
      </div>
    );
  }
};
export default EventDetails;

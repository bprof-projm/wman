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
            <Form.Item label="RangePicker" {...rangeConfig}>
              <RangePicker showTime format="YYYY-MM-DD HH:mm:ss" />
            </Form.Item>

            <Form.Item label="Address"></Form.Item>
            <Form.Item label="City">
              <Input />
            </Form.Item>
            <Form.Item label="Street">
              <Input />
            </Form.Item>
            <Form.Item label="Zip Code">
              <Input />
            </Form.Item>
            <Form.Item label="Building Number">
              <Input />
            </Form.Item>
            <Form.Item label="Floor and Door">
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
      const data = {
        jobDescription: e.description,
        estimatedStartDate: e.rangePicker[0]._d.toJSON(),
        estimatedFinishDate: e.rangePicker[1]._d.toJSON(),
        address: {
          city: e.city,
          street: e.street,
          zipCode: e.zipCode,
          buildingNumber: e.buildingNumber,
          floorDoor: e.floorDoor,
        },
        status: "awaiting",
      };

      axios
        .post("/CreateEvent", data)
        .then((response) => console.log(response));
    };
    console.log("create");
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
            <Form.Item label="Description" name="description">
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
            <Form.Item label="RangePicker" {...rangeConfig} name="rangePicker">
              <RangePicker showTime format="YYYY-MM-DD HH:mm:ss" />
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
              <Button type="primary" htmlType="submit">
                Button
              </Button>
            </Form.Item>
          </Form>
        </div>
      </div>
    );
  }
};
export default EventDetails;

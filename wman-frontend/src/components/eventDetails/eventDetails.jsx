import React, { useState, useEffect, setState } from "react";
import { Form, Input, Button, DatePicker } from "antd";
import "antd/dist/antd.css";
import "./eventDetails.css";
import moment from "moment";
import Select from "react-select";
import Cookies from "js-cookie";

const { RangePicker } = DatePicker;
const axios = require("axios").default;
const rangeConfig = {
  rules: [{ type: "array", required: true, message: "Please select time!" }],
};
const EventDetails = ({ workerEventId, workerEvent }) => {
  const [componentSize, setComponentSize] = useState("default");
  const [description, setDescription] = useState(null);
  const [city, setCity] = useState(null);
  const [street, setStreet] = useState(null);
  const [zipCode, setZipCode] = useState(null);
  const [buildingNumber, setBuildingNumber] = useState(null);
  const [floorDoor, setFloorDoor] = useState(null);
  const [startDate, setStartDate] = useState(null);
  const [endDate, setEndDate] = useState(null);
  const [selectedOption, setSelectedOption] = useState([]);
  const [options, setOptions] = useState([]);
  useEffect(() => {
    axios
      .get(`/GetAllLabel`, {
        headers: { Authorization: `Bearer ${Cookies.get("auth")}` },
      })
      .then((response) => {
        const ops = response.data.map((res) => ({
          value: res.id,
          label: res.content,
        }));
        console.log(ops);
        setOptions(ops); /*  setOptions(response.data) */
      })
      .catch((error) => console.log(error));
  }, [, axios]);

  const onFinish = (e) => {
    console.log(labelSelector.value);
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
    const data2 = {
      id: 3,
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
    if (workerEvent) {
      axios
        .put("/UpdateEvent", data2)
        .then((response) => console.log(response));
    } else {
      axios
        .post("/CreateEvent", data)
        .then((response) => console.log(response));
    }
  };

  // tesztelési szempont miatt hardCodeolni kellett a workerEvent értékét mert tomi mégmindig nincs kész a calendarral
  // , ezt később ki kell szedni és a komponens meghívásánál meg kell hívni a getWorkerEventet és átadni a workerEventet paraméterként
  workerEvent = {
    jobDescription: "React fejlesztés",
    estimatedStartDate: "2021-11-26T19:06:21.053Z",
    estimatedFinishDate: "2021-11-26T20:06:21.053Z",
    date: ["2021-11-12T19:06:21.053Z", "2021-11-12T20:06:21.053Z"],
    address: {
      city: "city",
      street: "street",
      zipCode: "1022",
      buildingNumber: "3",
      floorDoor: "",
    },
    status: "awaiting",
  };

  const SelectorOnChange = (e) => {
    setSelectedOption(e.value);
  };

  const AddLabel = () => {
    axios
      .post(
        `/AssignLabelToWorkEvent?eventId=` +
          1 /*  workerEventId */ + //itt most hardcodeolva van az event id , ha kész a fooldal, dinamikus lesz
          "&labelId=" +
          selectedOption,
        {
          headers: { Authorization: `Bearer ${Cookies.get("auth")}` },
        }
      )
      .then((response) => {
        const filtered = options.map((op) => {
          op.value !== selectedOption ? op : null;
        });
        console.log(filtered);
      });
  };

  if (workerEvent) {
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
            <Form.Item
              label="Description"
              name="description"
              initialValue={workerEvent.jobDescription}
              rules={[
                { required: true, message: "Please enter a description" },
              ]}
            >
              <Input />
            </Form.Item>

            <Form.Item
              label="From-To"
              {...rangeConfig}
              name="rangePicker"
              rules={[
                { required: true, message: "Please select a time range" },
              ]}
              initialValue={[
                moment(workerEvent.estimatedStartDate, "YYYY-MM-DD HH:mm:ss"),
                moment(workerEvent.estimatedFinishDate, "YYYY-MM-DD HH:mm:ss"),
              ]}
              initialValue={[
                moment(workerEvent.estimatedStartDate, "YYYY-MM-DD HH:mm:ss"),
                moment(workerEvent.estimatedFinishDate, "YYYY-MM-DD HH:mm:ss"),
              ]}
            >
              <RangePicker
                defaultValue={[
                  moment(workerEvent.estimatedStartDate, "YYYY-MM-DD HH:mm:ss"),
                  moment(
                    workerEvent.estimatedFinishDate,
                    "YYYY-MM-DD HH:mm:ss"
                  ),
                ]}
                showTime
                format={"YYYY-MM-DD HH:mm:ss"}
              />
            </Form.Item>

            <Form.Item label="Address"></Form.Item>
            <Form.Item
              label="City"
              name="city"
              initialValue={workerEvent.address.city}
              rules={[{ required: true, message: "Please enter your city" }]}
            >
              <Input />
            </Form.Item>
            <Form.Item
              label="Street"
              name="street"
              initialValue={workerEvent.address.street}
              rules={[{ required: true, message: "Please enter your street" }]}
            >
              <Input />
            </Form.Item>
            <Form.Item
              label="Zip Code"
              name="zipCode"
              initialValue={workerEvent.address.zipCode}
              rules={[
                { required: true, message: "Please enter your Zip code" },
              ]}
            >
              <Input />
            </Form.Item>
            <Form.Item
              label="Building Number"
              name="buildingNumber"
              initialValue={workerEvent.address.buildingNumber}
              rules={[
                {
                  required: true,
                  message: "Please enter your building bumber",
                },
              ]}
            >
              <Input />
            </Form.Item>
            <Form.Item
              label="Floor and Door"
              name="floorDoor"
              initialValue={workerEvent.address.floorDoor}
            >
              <Input />
            </Form.Item>

            <Form.Item label="Button">
              <Button type="primary" className="editEventBtn" htmlType="submit">
                Edit
              </Button>
            </Form.Item>
          </Form>
        </div>
        <div className="card">
          <Select
            name="labelSelector"
            options={options}
            onChange={SelectorOnChange}
          />

          <Button
            onClick={AddLabel}
            className="addLabelButton"
            type="primary"
            htmlType="submit"
          >
            Add
          </Button>
        </div>
      </div>
    );
  } else {
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
                Create
              </Button>
            </Form.Item>
          </Form>
        </div>
      </div>
    );
  }
};
export default EventDetails;

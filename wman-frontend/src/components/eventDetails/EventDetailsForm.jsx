import React from "react";
import { Form, Input, DatePicker, Typography, Select, Tag, Space } from "antd";
import moment from "moment";
import Label from "../Labels/Label";
import Avatar from "antd/lib/avatar/avatar";

const { RangePicker } = DatePicker;

function tagRender(props) {
  const { label, value, closable, onClose } = props;
  const onPreventMouseDown = (event) => {
    event.preventDefault();
    event.stopPropagation();
  };

  return (
    <Label
      key={value}
      backgroundColor={label.props ? label.props.backgroundColor : null}
      onMouseDown={onPreventMouseDown}
      closable={closable}
      onClose={onClose}
      name={label.props ? label.props.name : null}
    ></Label>
  );
}

const EventDetailsForm = ({ form, initialValues, labels, workers }) => {
  return (
    <Form
      form={form}
      layout="vertical"
    >
      <Form.Item
        label="Description"
        name="jobDescription"
        initialValue={initialValues.jobDescription}
        rules={[{ required: true, message: "Please enter a description" }]}
      >
        <Input />
      </Form.Item>

      <Form.Item
        label="From-To"
        name="rangePicker"
        rules={[{ required: true, message: "Please select a time range" }]}
        initialValue={
          initialValues.estimatedStartDate
            ? [
                moment(initialValues.estimatedStartDate, "YYYY-MM-DD HH:mm:ss"),
                moment(
                  initialValues.estimatedFinishDate,
                  "YYYY-MM-DD HH:mm:ss"
                ),
              ]
            : null
        }
      >
        <RangePicker
          showTime
          format={"YYYY-MM-DD HH:mm"}
          showTime={{ format: "HH:mm" }}
        />
      </Form.Item>

      <Form.Item
        name="labels"
        label="Labels"
        initialValue={initialValues.labels.map((l) => l.id)}
      >
        <Select mode="multiple" showArrow tagRender={tagRender}>
          {labels.map((label) => (
            <Option key={label.id} value={label.id}>
              <Label
                key={label.id}
                name={label.content}
                backgroundColor={label.backgroundColor}
              />
            </Option>
          ))}
        </Select>
      </Form.Item>

      <Form.Item
        name="assignedUsers"
        label="Workers"
        initialValue={initialValues.assignedUsers.map((l) => l.username)}
      >
        <Select mode="multiple" showArrow>
          {workers.map((worker) => (
            <Option key={worker.id} value={worker.username}>
              <Space>
                <Avatar size="small" src={worker.profilePicture.url} />
                <span>{`${worker.firstname} ${worker.lastname}`}</span>
              </Space>
            </Option>
          ))}
        </Select>
      </Form.Item>

      <Typography.Title level={4}>Address</Typography.Title>
      <Form.Item
        label="City"
        name="city"
        initialValue={initialValues.address.city}
        rules={[{ required: true, message: "Please enter your city" }]}
      >
        <Input />
      </Form.Item>
      <Form.Item
        label="Street"
        name="street"
        initialValue={initialValues.address.street}
        rules={[{ required: true, message: "Please enter your street" }]}
      >
        <Input />
      </Form.Item>
      <Form.Item
        label="Zip Code"
        name="zipCode"
        initialValue={initialValues.address.zipCode}
        rules={[{ required: true, message: "Please enter your Zip code" }]}
      >
        <Input />
      </Form.Item>
      <Form.Item
        label="Building Number"
        name="buildingNumber"
        initialValue={initialValues.address.buildingNumber}
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
        initialValue={initialValues.address.floorDoor}
      >
        <Input />
      </Form.Item>
    </Form>
  );
};

export default EventDetailsForm;

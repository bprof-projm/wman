import { Form, Input, Button, Drawer, Radio, Space } from "antd";
import React, { useEffect, useState, useRef } from "react";
import LabelsList from "../LabelList/labelList";
import { TagsOutlined } from "@ant-design/icons";
import axios from "axios";
import { SearchBox } from "../../SearchBox/search-box.component";
import "./labelMenu.css";
import Cookies from "js-cookie";
import ColorPickerButton from "../ColorPickerButton/ColorPickerButton";
const LabelsMenu = () => {
  /*Visibility*/
  const ref = useRef();

  const [visible, setVisible] = useState(false);

  const showMenu = () => {
    setVisible(true);
  };

  const closeMenu = () => {
    setVisible(false);
  };

  /*Placement*/
  const [placement, setPlacement] = useState("right");

  const changePlacement = (e) => {
    setPlacement(e.target.value);
  };

  const deleteLabel = (param) => (e) => {
    axios
      .delete(`/DeleteLabel/` + param, {
        headers: { Authorization: `Bearer ${Cookies.get("auth")}` },
      })
      .then((response) => {
        if (param === editLabelId) {
          setDisplayEditLabel(false);
        }
        axios
          .get(`/GetAllLabel`, {
            headers: { Authorization: `Bearer ${Cookies.get("auth")}` },
          })
          .then((response) => setLabels(response.data));
      })
      .catch((error) => console.log(error));
  };

  /*Get elements*/
  const [labels, setLabels] = useState([]);
  useEffect(() => {
    axios
      .get(`/GetAllLabel`, {
        headers: { Authorization: `Bearer ${Cookies.get("auth")}` },
      })
      .then((response) => setLabels(response.data))
      .catch((error) => console.log(error));
  }, [, axios, visible]);

  /*Search user by name*/
  const [searchField, setSearchField] = useState("");

  const handleChange = (e) => {
    setSearchField(e.target.value);
  };
  const filteredLabels = labels.filter((label) =>
    label.content.includes(searchField)
  );

  /*Create label*/

  const [displayAddLabelForm, setDisplayAddLabelForm] = useState(false);
  const [displayEditLabel, setDisplayEditLabel] = useState(false);
  const [editLabelContent, setEditLabelContent] = useState(null);
  const [editLabelBackgroundColor, setEditLabelBackgroundColor] =
    useState(null);
  const [addLabelContent, setAddLabelContent] = useState(null);
  const [addLabelBackgroundColor, setAddLabelBackgroundColor] = useState(null);
  const [editLabelId, setEditLabelId] = useState(null);
  const showEditLabel = (content, backgroundColor, id) => (e) => {
    setEditLabelContent(content);
    setEditLabelId(id);
    setEditLabelBackgroundColor(backgroundColor);
    setDisplayEditLabel(true);
    renderInputFormItem();
  };
  const handleColorChangeComplete = (color) => {
    setEditLabelBackgroundColor(color.hex);
  };

  const handleColorChangeCompleteAdd = (color) => {
    setAddLabelBackgroundColor(color.hex);
  };

  const updateLabel = (e) => {
    const data = {
      color: editLabelBackgroundColor,
      content: e.Content,
    };

    axios
      .put(`/UpdateLabel/` + editLabelId, data, {
        headers: { Authorization: `Bearer ${Cookies.get("auth")}` },
      })
      .then((res) =>
        axios
          .get(`/GetAllLabel`, {
            headers: { Authorization: `Bearer ${Cookies.get("auth")}` },
          })
          .then((response) => {
            setLabels(response.data);
            setDisplayEditLabel(false);
          })
      )
      .catch((error) => console.log(error));
  };

  const AddLabel = (e) => {
    const data = {
      color: addLabelBackgroundColor,
      content: e.Content,
    };
    axios
      .post(`/CreateLabel`, data, {
        headers: { Authorization: `Bearer ${Cookies.get("auth")}` },
      })
      .then((res) =>
        axios
          .get(`/GetAllLabel`, {
            headers: { Authorization: `Bearer ${Cookies.get("auth")}` },
          })
          .then((response) => {
            setLabels(response.data);
            setDisplayAddLabelForm(false);
          })
      )
      .catch((error) => console.log(error));
  };

  const renderInputFormItem = () => {
    console.log("called");
    return (
      <Form.Item
        label="Content"
        name="Content"
        rules={[
          {
            required: true,
            message: "Please input content!",
          },
        ]}
        initialValue={editLabelContent}
      >
        <Input />
      </Form.Item>
    );
  };

  const handleAddNewLabel = () => {
    setDisplayAddLabelForm(!displayAddLabelForm);
  };

  return (
    <div>
      <Button shape="circle" icon={<TagsOutlined />} onClick={showMenu} />

      <Drawer
        title="Labels"
        placement={placement}
        onClose={closeMenu}
        visible={visible}
      >
        <details>
          <summary>Placement Options</summary>
          <div>
            <Space>
              <Radio.Group value={placement} onChange={changePlacement}>
                <Radio value="left">left</Radio>
                <Radio value="right">right</Radio>
              </Radio.Group>
            </Space>
          </div>
        </details>
        <Button onClick={handleAddNewLabel} className="addNewLabelBtn">
          Add New Label
        </Button>
        <br />
        {displayAddLabelForm ? (
          <div>
            <Form
              name="addLabelForm"
              labelCol={{
                span: 8,
              }}
              wrapperCol={{
                span: 16,
              }}
              initialValues={{
                remember: true,
              }}
              autoComplete="off"
              onFinish={AddLabel}
            >
              <Form.Item
                label="Content"
                name="Content"
                rules={[
                  {
                    required: true,
                    message: "Please input content!",
                  },
                ]}
              >
                <Input />
              </Form.Item>

              <ColorPickerButton
                className="pickColor"
                color={setAddLabelBackgroundColor}
                onChangeComplete={handleColorChangeCompleteAdd}
              ></ColorPickerButton>

              <Form.Item
                wrapperCol={{
                  offset: 8,
                  span: 16,
                }}
              >
                <Button className="submitBtn" type="primary" htmlType="submit">
                  Submit
                </Button>
              </Form.Item>
            </Form>
          </div>
        ) : null}
        <div className="searchbox">
          <SearchBox placeholder="Search Label" handleChange={handleChange} />
        </div>
        <br />
        {/*props needed objects(list)*/}
        <div>
          <LabelsList
            objects={filteredLabels}
            deleteLabel={deleteLabel}
            showEditLabel={showEditLabel}
          />
        </div>
        {displayEditLabel ? (
          <div>
            <Form
              name="editLabelForm"
              labelCol={{
                span: 8,
              }}
              wrapperCol={{
                span: 16,
              }}
              initialValues={{
                remember: true,
              }}
              autoComplete="off"
              onFinish={updateLabel}
            >
              {renderInputFormItem()}

              <ColorPickerButton
                color={editLabelBackgroundColor}
                onChangeComplete={handleColorChangeComplete}
              ></ColorPickerButton>

              <Form.Item
                wrapperCol={{
                  offset: 8,
                  span: 16,
                }}
              >
                <Button className="submitBtn" type="primary" htmlType="submit">
                  Submit
                </Button>
              </Form.Item>
            </Form>
          </div>
        ) : null}
      </Drawer>
    </div>
  );
};

export default LabelsMenu;

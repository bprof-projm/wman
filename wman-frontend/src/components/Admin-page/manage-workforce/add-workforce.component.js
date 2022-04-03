import { Button, Form, Input } from "antd";
import './manage-workforce.styles.css';

import { Upload } from 'antd';
import ImgCrop from 'antd-img-crop';
import { useState } from "react";
import axios from "axios";

const AddWorkforce = () => {
  const [form] = Form.useForm();

  //picture
  const [fileList, setFileList] = useState([
    {
      uid: '-1',
      name: 'image.png',
      status: 'done',
      url: 'https://zos.alipayobjects.com/rmsportal/jkjgkEfvpUPVyRjUImniVslZfWPnJuuZ.png',
    },
  ]);

  const onChange = ({ fileList: newFileList }) => {
    setFileList(newFileList);
  };

  const onPreview = async file => {
    let src = file.url;
    if (!src) {
      src = await new Promise(resolve => {
        const reader = new FileReader();
        reader.readAsDataURL(file.originFileObj);
        reader.onload = () => resolve(reader.result);
      });
    }
    const image = new Image();
    image.src = src;
    const imgWindow = window.open(src);
    imgWindow.document.write(image.outerHTML);
  };

  const onFinish =(values) =>{
    console.log(values)
    axios
        .post("/Admin/Create", {
          username: values.username,
          email: values.email,
          password: values.password,
          role: values.role,
          firstname: values.firstname,
          lastname: values.lastname,
          phoneNumber: values.phoneNumber,
          photo: fileList[0],         
        })
        .then(() => console.log("User Created!"))
        .catch((err) => console.log(err));
  }

  return (
    <div>
      <div className="admin-form-container">
        <h1>Add Workforce</h1>
        <Form form={form} onFinish={(values) => onFinish(values)}>
          <Form.Item label="Username:" name="username" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item label="Email:" name="email" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item label="Password:" name="password" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item label="Role:" name="role" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item label="First name:" name="firstname" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item label="Last name:" name="lastname" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item label="Phone number:" name="phoneNumber" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item label="Photo:" name="photo">
                        <ImgCrop rotate>
                            <Upload
                                action="https://www.mocky.io/v2/5cc8019d300000980a055e76"
                                listType="picture-card"
                                fileList={fileList}
                                onChange={onChange}
                                onPreview={onPreview}
                            >
                                {fileList.length < 3 && '+ Upload'}
                            </Upload>
                        </ImgCrop>
                    </Form.Item>
          <div className="admin-form-button">
            <Button className="form-button" htmlType="submit">OK</Button></div>
        </Form>
      </div>
    </div>
  );
}
export default AddWorkforce;
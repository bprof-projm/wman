import { Button, Form, Input } from "antd";
import axios from "axios";
import { useEffect, useState } from "react";
import { Upload } from 'antd';
import ImgCrop from 'antd-img-crop';
import './manage-workforce.styles.css';
import Swal from "sweetalert2";

const UpdateWorkforce = (props) => {
    const [form] = Form.useForm();
    const { username, email, firstname, lastname, phoneNumber } = props.object;
    form.setFieldsValue({
        email: email,
        firstname: firstname,
        lastname: lastname,
        phoneNumber: phoneNumber,
    });

    useEffect(() => {
        axios.get(`/Auth/userrole?username=${username}`)
            .then((response) => form.setFieldsValue({
                role: response.data,
            }))
            .catch((error) => console.log(error))
    }, []);

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

    const onFinish = (values) => {
        const formData = new FormData();
        formData.append("email", values.email);
        formData.append("role", values.role);
        formData.append("firstname", values.firstname);
        formData.append("lastname", values.lastname);
        formData.append("phoneNumber", values.phoneNumber);
        formData.append("photo", fileList[0]);

        axios.put(
            `/Admin/Modify/${username}`,
            formData
        ).then(x => {            
            Swal.fire({
                icon: 'success',
                title: 'Success',
                text: 'User Updated!',
            });
        }).then(x =>             
            props.func()
        ).catch(x => Swal.fire({
            icon: 'error',
            title: 'Oops',
            text: 'Error updating user!',
        }));
    }
    return (
        <div>
            <div className="admin-form-container">
                <h1>Update Workforce</h1>
                <Form form={form} onFinish={(values) => onFinish(values)}>
                    <Form.Item label="Email:" name="email" rules={[{ required: true }]}>
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
                                {fileList.length < 5 && '+ Upload'}
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
export default UpdateWorkforce;
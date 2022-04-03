import { Button, Form, Input } from "antd";
import './manage-workforce.styles.css';

const AddWorkforce = () => {
    const [form] = Form.useForm();
  
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
              <Input />
            </Form.Item>
            <div className="admin-form-button">
            <Button className="form-button" htmlType="submit">OK</Button></div>
          </Form>
        </div>
    </div>
  );
}
export default AddWorkforce;
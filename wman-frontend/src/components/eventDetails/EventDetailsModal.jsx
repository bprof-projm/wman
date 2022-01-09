import { Form, message, Modal, Spin } from "antd";
import { LoadingOutlined } from "@ant-design/icons";
import { useEffect, useState } from "react";
import styled from "styled-components";
import EventDetailsForm from "./EventDetailsForm";
import axios from "axios";

const workers = [
  {
    username: "worker1",
    email: "sulaiman.eklund@gmail.com",
    firstname: "Sulaiman",
    lastname: "Eklund",
    profilePicture: {
      cloudPhotoID: "default_profile_picture",
      url: "https://res.cloudinary.com/wmanproj/image/upload/v1640774841/default_profile_picture.png",
      wManUserID: 4,
    },
    phoneNumber: "+3489717652",
  },
  {
    username: "worker2",
    email: "delfred@mail.com",
    firstname: "Delma",
    lastname: "Fredriksson",
    profilePicture: {
      cloudPhotoID: "default_profile_picture",
      url: "https://res.cloudinary.com/wmanproj/image/upload/v1640774841/default_profile_picture.png",
      wManUserID: 5,
    },
    phoneNumber: "+6287419537",
  },
];

const SpinnerContainer = styled.div`
  width: 100%;
  display: flex;
  justify-content: center;
`;

const initialState = {
  eventDetails: {
    address: {},
    labels: [],
    assignedUsers: [],
  },
  loading: false,
  openForm: false,
};

const EventDetailsModal = ({ eventId, onClose }) => {
  const [eventDetails, setEventDetails] = useState(initialState.eventDetails);
  const [loading, setLoading] = useState(initialState.loading);
  const [openForm, setOpenForm] = useState(initialState.openForm);
  const [availableLabels, setAvailableLabels] = useState([]);
  const [confirmLoading, setConfirmLoading] = useState(false);

  const [form] = Form.useForm();

  useEffect(async () => {
    setLoading(true);

    const requests = [axios.get("/GetAllLabel").then((res) => res.data)];

    if (eventId) {
      requests.push(
        axios.get(`/CalendarEvent/WorkCard/${eventId}`).then((res) => res.data)
      );
    }

    const [labels, event] = await Promise.all(requests);

    if (event) setEventDetails(event);
    setAvailableLabels(labels);
    setLoading(false);
    setOpenForm(true);
  }, []);

  const getTitle = () => {
    if (loading) return "Loading event details...";
    if (!eventId) return "Create a new event";

    return `Edit "${eventDetails.jobDescription}"`;
  };

  const mapFormValuesToEventDetails = (values) => {
    return {
      jobDescription: values.jobDescription,
      estimatedStartDate: values.rangePicker[0].format(),
      estimatedFinishDate: values.rangePicker[1].format(),
      address: {
        city: values.city,
        street: values.street,
        zipCode: values.zipCode,
        buildingNumber: values.buildingNumber,
        floorDoor: values.floorDoor,
      },
    };
  };

  return (
    <Modal
      destroyOnClose
      maskClosable={false}
      visible={true}
      title={getTitle()}
      confirmLoading={confirmLoading}
      okText={eventId ? "Edit" : "Create"}
      cancelText="Cancel"
      onCancel={onClose}
      onOk={() => {
        form.validateFields().then(async (values) => {
          const data = mapFormValuesToEventDetails(values);
          setConfirmLoading(true);

          try {
            let id = eventId;

            if (id) {
              await axios.put("/UpdateEvent", { id, ...data });
            } else {
              id = await axios.post("/CreateEvent", data);
            }

            const newLabels = values.labels.filter((id) =>
              !eventDetails.labels.map((l) => l.id).includes(id)
            );
            if (newLabels && newLabels.length > 0) {
              console.log('mass assign labels')
              await axios.post(
                `/MassAssignLabelToWorkEvent?eventId=${id}`,
                newLabels
              );
            }

            const newWorkers = values.assignedUsers.filter((username) =>
              !eventDetails.assignedUsers
                .map((u) => u.username)
                .includes(username)
            );
            if (newWorkers && newWorkers.length > 0) {
              await axios.post(
                `/Event/massAssign?eventid=${id}`,
                newWorkers
              );
            }

            onClose();
          } catch (err) {
            console.error(err)
            message.error("Failed to save event details");
          }

          setConfirmLoading(false);
        });
      }}
    >
      {loading && (
        <SpinnerContainer>
          <Spin indicator={<LoadingOutlined style={{ fontSize: 60 }} />} />
        </SpinnerContainer>
      )}

      {openForm && (
        <EventDetailsForm
          form={form}
          initialValues={eventDetails}
          labels={availableLabels}
          workers={workers}
        />
      )}
    </Modal>
  );
};

export default EventDetailsModal;

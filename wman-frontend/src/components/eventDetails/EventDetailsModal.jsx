import { Form, Modal, Spin } from "antd";
import { LoadingOutlined } from "@ant-design/icons";
import { useEffect, useState } from "react";
import styled from "styled-components";
import EventDetailsForm from "./EventDetailsForm";
import axios from 'axios';

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
  const [availableLabels, setAvailableLabels] = useState([])

  const [form] = Form.useForm();

  useEffect(async () => {
    setLoading(true);

    const requests = [
      axios.get('/GetAllLabel').then(res => res.data),
    ]

    if (eventId) {
      requests.push(axios.get(`/CalendarEvent/WorkCard/${eventId}`).then(res => res.data));
    }

    const [labels, event] = await Promise.all(requests)

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
      estimatedStartDate: values.rangePicker[0].format("YYYY-MM-DD HH:mm"),
      estimatedEndDate: values.rangePicker[1].format("YYYY-MM-DD HH:mm"),
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
      okText={eventId ? "Edit" : "Create"}
      cancelText="Cancel"
      onCancel={onClose}
      onOk={() => {
        form.validateFields().then((values) => {
          console.log("eventDetails:", mapFormValuesToEventDetails(values));
          console.log("labels:", values.labels);
          console.log("workers:", values.assignedUsers);

          // TODO: post event details
          // TODO: put labels on event
          // TODO: assig workers to event
          onClose();
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

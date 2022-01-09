import { Form, Modal, Spin } from "antd";
import { LoadingOutlined } from "@ant-design/icons";
import { useEffect, useState } from "react";
import styled from "styled-components";
import EventDetailsForm from "./EventDetailsForm";

const eventData = {
  id: 7,
  jobDescription: "Example event #6",
  estimatedStartDate: "2022-01-05T07:00:00",
  estimatedFinishDate: "2022-01-05T09:00:00",
  assignedUsers: [
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
  ],
  labels: [
    {
      id: 1,
      backgroundColor: "#E70000",
      textColor: "#18ffff",
      content: "ASAP",
    },
    {
      id: 2,
      backgroundColor: "#2986CC",
      textColor: "#d67933",
      content: "Plumber required",
    },
    {
      id: 3,
      backgroundColor: "#8FCE00",
      textColor: "#7031ff",
      content: "Gasfitter required",
    },
  ],
  address: {
    city: "Budapest",
    street: "Bécsi út",
    zipCode: 1034,
    buildingNumber: "104-108.",
    floorDoor: null,
  },
  workStartDate: "2022-01-05T07:30:00",
  workFinishDate: "2022-01-05T10:00:00",
  proofOfWorkPic: [],
  status: "finished",
};

const labels = [
  {
    id: 1,
    backgroundColor: "#E70000",
    textColor: "#18ffff",
    content: "ASAP",
  },
  {
    id: 2,
    backgroundColor: "#2986CC",
    textColor: "#d67933",
    content: "Plumber required",
  },
  {
    id: 3,
    backgroundColor: "#8FCE00",
    textColor: "#7031ff",
    content: "Gasfitter required",
  },
];
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

  const [form] = Form.useForm();

  useEffect(async () => {
    //fetch event data
    if (eventId) {
      setLoading(true);
      await new Promise((resolve) => setTimeout(resolve, 2000));
      setLoading(false);
      setEventDetails(eventData);
    }

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
          labels={labels}
          workers={workers}
        />
      )}
    </Modal>
  );
};

export default EventDetailsModal;

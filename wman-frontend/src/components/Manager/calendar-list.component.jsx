import React, { Component } from "react";
import ColumnComponent from "./column.component.jsx";
import { DragDropContext } from "react-beautiful-dnd";
import PrintButton from "../Print-functionality/Print-button/print-button.component";
import ProgressMenu from "../Worker-load/Progress-menu/progress-menu.component";
import LabelsMenu from "../Labels/LabelMenu/labelMenu";
import { Logout } from "../Logout/logout.component";
import axios from "axios";
import { Layout, Button, Avatar, Popover, message, Typography } from "antd";
import {
  LeftOutlined,
  RightOutlined,
  PlusCircleFilled,
} from "@ant-design/icons";
import moment from "moment";
import EventDetailsModal from "../eventDetails/EventDetailsModal.jsx";

import styled from "styled-components";
import "./calendar-list.styles.css";

import {  HubConnectionBuilder, LogLevel, HttpTransportType } from '@microsoft/signalr';
import Cookies from "js-cookie";
const { Header, Content } = Layout;
const { Title } = Typography;

const SiteLayout = styled(Content)`
  padding: 24px 10px;
  margin-top: 64px;
  min-height: 380px;
  display: flex;
  flex-direction: column;
  background-color: white;
`;
const Container = styled.div`
  height: 100%;
  display: flex;
  width: 100%;
`;
const HeaderItems = styled.div`
  display: flex;
  justify-content: space-between;
`;
const HeaderItemsLeftSide = styled.div`
  display: flex;
  align-items: center;

  & button {
    margin-right: 10px;
    color: white;
  }

  & button:hover,
  & button:active,
  & button:focus {
    color: #1890ff;
  }
`;
const HeaderItemsRightSide = styled.div`
  display: flex;
  align-items: center;

  & > * {
    margin-left: 10px;
  }
`;
const UserMenuContent = styled.div`
  text-align: center;
`;

const eventComparator = (a, b) =>
  new Date(a.estimatedStartDate) < new Date(b.estimatedStartDate) ? -1 : 1;

const getEventsForDay = (events, day) => {
  return events
    .filter((event) => new Date(event.estimatedStartDate).getDay() === day)
    .sort(eventComparator)
    .map((event) => event.id);
};

const initialData = {
  user: {},
  year: new Date().getFullYear(),
  week: moment().isoWeek(),
  modalVisible: false,
  eventId: null,
  events: {},
  columns: {
    monday: {
      id: "monday",
      title: "Monday",
      eventIds: [],
    },
    tuesday: {
      id: "tuesday",
      title: "Tuesday",
      eventIds: [],
    },
    wednesday: {
      id: "wednesday",
      title: "Wednesday",
      eventIds: [],
    },
    thursday: {
      id: "thursday",
      title: "Thursday",
      eventIds: [],
    },
    friday: {
      id: "friday",
      title: "Friday",
      eventIds: [],
    },
    saturday: {
      id: "saturday",
      title: "Saturday",
      eventIds: [],
    },
    sunday: {
      id: "sunday",
      title: "Sunday",
      eventIds: [],
    },
  },
};

class CalendarListComponent extends Component {
  
  state = initialData;
  componentDidMount() {
    this.fetchUsername();
    this.fetchEvents(moment().isoWeek());
/*
    let connection = new HubConnectionBuilder()
      .withUrl('https://localhost:5001/notify/', {
        skipNegotiation: true, accessTokenFactory: () => Cookies.get("auth"),
        withCredentials: true, transport: HttpTransportType.WebSockets
      })
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build();

      this.setState({connection}, ()=>{
        connection.start()
        
        connection.on('Connected', () => console.log('Connected to signalR'));
        connection.on('Disconnected', () => console.log('disconnected signalR'));

        connection.on('EventDeleted', (args) => this.fetchEvents(this.state.week));
        connection.on('EventChanged', (args) => this.fetchEvents(this.state.week));
        connection.on('EventStateChanged', (args) => this.fetchEvents(this.state.week));

        connection.on('UserAssignedCurrentDay', (args) => this.fetchEvents(this.state.week));
        connection.on('EventChangedForToday', (args) => this.fetchEvents(this.state.week));
        connection.on('EventChangedFromTodayToNotToday', (args) => this.fetchEvents(this.state.week));
      
      })     */

  }

  getUserMenu = () => {
    return (
      <UserMenuContent>
        <p>{this.state.user.firstname + " " + this.state.user.lastname}</p>
        <Logout />
      </UserMenuContent>
    );
  };

  onDragEnd = async (result) => {
    const { destination, source, draggableId } = result;

    if (!destination) {
      return;
    }

    if (
      result.destination.droppableId === result.source.droppableId &&
      destination.index === source.index
    ) {
      return;
    }

    const start = this.state.columns[source.droppableId];
    const finish = this.state.columns[destination.droppableId];

    if (start === finish) {
      return; //Disable moving cards inside column
    }

    //Moving from one list to another
    const startEventIds = Array.from(start.eventIds);
    startEventIds.splice(source.index, 1);
    const newStart = {
      ...start,
      eventIds: startEventIds,
    };

    const finishEventIds = Array.from(finish.eventIds);
    finishEventIds.splice(destination.index, 0, draggableId);
    const newFinish = {
      ...finish,
      eventIds: finishEventIds.sort((id1, id2) => {
        const a = this.state.events[id1];
        const b = this.state.events[id2];

        return eventComparator(a, b);
      }),
    };

    // console.log(eventIds);
    // console.log(finishEventIds);

    const newState = {
      ...this.state,
      columns: {
        ...this.state.columns,
        [newStart.id]: newStart,
        [newFinish.id]: newFinish,
      },
    };

    let curState = this.state;
    this.setState(newState);

    //this.state.events ben frissiteni a mozgatni kivant event start es finish datumat

    let eventSucces = false;

    try {
      await axios
        .put(`/DnDEvent/${draggableId}`, {
          estimatedStartDate: `${moment(
            this.state.events[draggableId].estimatedStartDate
          )
            .date(finish.date)
            .format()}`,
          estimatedFinishDate: `${moment(
            this.state.events[draggableId].estimatedFinishDate
          )
            .date(finish.date)
            .format()}`,
        })
        .then(() => (eventSucces = true));
    } catch (error) {
      message.error("Can not move event");
      this.setState(curState);
    }

    if (eventSucces) {
      this.fetchEvents(this.state.week);
      message.success("Event successfully moved");
    }
  };

  fetchUsername = () => {
    axios
      .get(`/Auth/username?username=${this.props.username}`)
      .then((res) => res.data)
      .then((user) => this.setState({ user }));
  };

  fetchEvents = async (week) => {
    let year = this.state.year;
    const maxWeeksInYear = moment().year(year).isoWeeksInYear();
    if (maxWeeksInYear < week) {
      year += 1;
      week = 1;
    }
    if (week < 1) {
      year -= 1;
      week = moment().year(year).isoWeeksInYear();
    }

    const events = await axios
      .get(`/CalendarEvent/GetWeekEvents/${year}/${week}`)
      .then((response) => response.data);

    this.setState((state) => ({
      year,
      week,
      events: events.reduce(
        (groupedEvents, event) => ({
          ...groupedEvents,
          [event.id]: event,
        }),
        {}
      ),
      columns: {
        monday: {
          ...state.columns.monday,
          date: moment().year(year).day(1).isoWeek(week).format("D"),
          eventIds: getEventsForDay(events, 1),
        },
        tuesday: {
          ...state.columns.tuesday,
          date: moment().year(year).day(2).isoWeek(week).format("D"),
          eventIds: getEventsForDay(events, 2),
        },
        wednesday: {
          ...state.columns.wednesday,
          date: moment().year(year).day(3).isoWeek(week).format("D"),
          eventIds: getEventsForDay(events, 3),
        },
        thursday: {
          ...state.columns.thursday,
          date: moment().year(year).day(4).isoWeek(week).format("D"),
          eventIds: getEventsForDay(events, 4),
        },
        friday: {
          ...state.columns.friday,
          date: moment().year(year).day(5).isoWeek(week).format("D"),
          eventIds: getEventsForDay(events, 5),
        },
        saturday: {
          ...state.columns.saturday,
          date: moment().year(year).day(6).isoWeek(week).format("D"),
          eventIds: getEventsForDay(events, 6),
        },
        sunday: {
          ...state.columns.sunday,
          date: moment().year(year).day(7).isoWeek(week).format("D"),
          eventIds: getEventsForDay(events, 0),
        },
      },
    }));
  };

  render() {
    return (
      <div>
        <Layout>
          <Header style={{ position: "fixed", zIndex: 1, width: "100%" }}>
            <div className="logoLeftSide" style={{ width: "200px" }}>
              <div className="logo" style={{ marginTop: "7px" }}>
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="50"
                  height="50"
                  viewBox="0 0 949 945"
                >
                  <g>
                    <g>
                      <path
                        fill="#00dcc7"
                        d="M619.626 23.108c-46.106 93.893-85.39 215.437-143.502 331.467-15.963-12.454-98.82-266.173-128.657-257.258-23.051-3.285-26.392 3.298-34.639 4.947-68.712 80.32-64.767 165.325-118.76 252.31-2.438-1.931-.893-.618-6.682-10.361-10.908-30.945-23.487-56.02-77.44-172.687C294.735-34.631 501.342-10.106 619.626 23.108zm79.173 34.63c287.213 134.043 289.748 530.096 192.986 628.304-100.4-231.39-91.273-265.292-158.347-242.416-19.439 19.375-25.64 20.099-59.38 113.787-69.923 193.506-42.454 174.232-148.45-79.157 45.977-111.163 113.817-278.71 173.191-420.517zM332.622 250.683c17.111 15.153 55.651 107.645 98.967 232.522-48.009 81.184-106.377 263.362-183.09 405.675C74.51 815.297-89.048 508.264 55.515 260.577c75.118 116.43 64.27 240.368 138.554 252.31 66.503 6.05 112.908-225.535 138.554-262.205zm138.553 341.362c60.486 97.757 55.872 129.174 108.864 237.468 8.029 15.645 40.658 38.833 74.225 4.948 77.744-176.827 80.447-192.805 103.915-242.416 45.49 114.962 55.313 138.11 79.174 192.943-35.455 40.362-175.67 213.585-499.783 143.47-8.213-20.844 132.595-285.673 133.605-336.413z"
                      />
                    </g>
                  </g>
                </svg>
              </div>
              <span style={{ marginLeft: "20px" }}>Wman</span>
            </div>
            <HeaderItems>
              <HeaderItemsLeftSide>
                <ProgressMenu />
                <LabelsMenu />
                <PrintButton />
              </HeaderItemsLeftSide>
              <HeaderItemsRightSide>
                <Button
                  type="primary"
                  shape="round"
                  icon={<PlusCircleFilled />}
                  onClick={() =>
                    this.setState({ modalVisible: true, eventId: null })
                  }
                >
                  Create event
                </Button>
                <Popover placement="bottomRight" content={this.getUserMenu()}>
                  <Avatar
                    src={`https://eu.ui-avatars.com/api?name=${encodeURIComponent(
                      this.state.user.firstname + " " + this.state.user.lastname
                    )}`}
                  />
                </Popover>
              </HeaderItemsRightSide>
            </HeaderItems>
          </Header>

          <SiteLayout>
            <Title level={2}>{`${this.state.year}. ${moment()
              .year(this.state.year)
              .day(1)
              .isoWeek(this.state.week)
              .format("MMMM")}`}</Title>
            <div style={{ display: "flex" }}>
              <Button
                shape="circle"
                size="large"
                icon={<LeftOutlined />}
                onClick={() => this.fetchEvents(this.state.week - 1)}
              />
              <DragDropContext onDragEnd={this.onDragEnd}>
                <Container>
                  {Object.keys(this.state.columns).map((columnId) => {
                    const column = this.state.columns[columnId];
                    const events = column.eventIds.map(
                      (eventId) => this.state.events[eventId]
                    );

                    return (
                      <ColumnComponent
                        key={column.id}
                        column={column}
                        events={events}
                        onCardClick={(eventId) =>
                          this.setState({ modalVisible: true, eventId })
                        }
                      />
                    );
                  })}
                </Container>
              </DragDropContext>
              <Button
                shape="circle"
                size="large"
                icon={<RightOutlined />}
                onClick={() => this.fetchEvents(this.state.week + 1)}
              />
            </div>
          </SiteLayout>
        </Layout>
        {this.state.modalVisible && (
          <EventDetailsModal
            eventId={this.state.eventId}
            onSuccess={() => {
              this.setState({ modalVisible: false, eventId: null });
              this.fetchEvents(this.state.week);
            }}
            onClose={() =>
              this.setState({ modalVisible: false, eventId: null })
            }
          />
        )}
      </div>
    );
  }
}

export default CalendarListComponent;

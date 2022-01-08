import React, { Component, useState, useEffect } from "react";
import ReactDOM from "react-dom";
import ColumnComponent from "./column.component.jsx";
import { DragDropContext } from "react-beautiful-dnd";
import PrintButton from "../Print-functionality/Print-button/print-button.component";
import ProgressMenu from "../Worker-load/Progress-menu/progress-menu.component";
import LabelsMenu from "../Labels/LabelMenu/labelMenu";
import EventDetails from "../eventDetails/eventDetails";
import { Logout } from "../Logout/logout.component";
import { Layout, Menu } from "antd";
import axios from "axios";

import styled from "styled-components";
import "./calendar-list.styles.css";

const { Header, Content, Footer } = Layout;

const Container = styled.div`
  height: 100%;
  display: flex;
`;

const getEventsForDay = (events, day) => {
  return events
    .filter((event) => new Date(event.estimatedStartDate).getDay() === day)
    .map((event) => event.id);
};

class CalendarListComponent extends Component {
  // constructor(props) {
  //   super(props);
  //   this.state = { initialData: initialData, currentWeek: [] };
  // }
  state = initialData;

  componentDidMount() {
    this.fetchEvents();
  }

  onDragEnd = (result) => {
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
    //TODO: order columns based on time

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
      eventIds: finishEventIds,
    };

    const newState = {
      ...this.state,
      columns: {
        ...this.state.columns,
        [newStart.id]: newStart,
        [newFinish.id]: newFinish,
      },
    };

    this.setState(newState);
  };

  fetchEvents = async () => {
    this.setState({ loading: true });
    const events = await axios
      .get(`/CalendarEvent/GetCurrentWeekEvents`)
      .then((response) => response.data);
    this.setState({ loading: false });
    this.setState({
      events: events.reduce(
        (groupedEvents, event) => ({
          ...groupedEvents,
          [event.id]: event,
        }),
        {}
      ),
    });
    this.setState((state) => ({
      columns: {
        monday: {
          ...state.columns.monday,
          eventIds: getEventsForDay(events, 1),
        },
        tuesday: {
          ...state.columns.tuesday,
          eventIds: getEventsForDay(events, 2),
        },
        wednesday: {
          ...state.columns.wednesday,
          eventIds: getEventsForDay(events, 3),
        },
        thursday: {
          ...state.columns.thursday,
          eventIds: getEventsForDay(events, 4),
        },
        friday: {
          ...state.columns.friday,
          eventIds: getEventsForDay(events, 5),
        },
        saturday: {
          ...state.columns.saturday,
          eventIds: getEventsForDay(events, 6),
        },
        sunday: {
          ...state.columns.sunday,
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
            <div className="logo">Wman</div>
            {/* <Menu theme="dark" mode="horizontal" defaultSelectedKeys={["1"]}>
              <Menu.Item key="1">nav 1</Menu.Item>
              <Menu.Item key="2">nav 2</Menu.Item>
              <Menu.Item key="3">nav 3</Menu.Item>
            </Menu> */}
            <div className="logoutButton">
              <Logout />
            </div>
          </Header>

          <div className="progressMenu">
            <ProgressMenu />
          </div>
          <div className="labelsMenu">
            <LabelsMenu />
          </div>
          <div className="printButton">
            <PrintButton />
          </div>

          <Content
            className="site-layout"
            style={{ padding: "0 50px", marginTop: 64 }}
          >
            <div
              className="site-layout-background"
              style={{ padding: 24, minHeight: 380 }}
            >
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
                      />
                    );
                  })}
                </Container>
              </DragDropContext>
            </div>
          </Content>
          {/* <Footer style={{ textAlign: "center" }}>
            Ant Design ©2018 Created by Ant UED
          </Footer> */}
        </Layout>
      </div>
    );
  }
}

const initialData = {
  loading: false,
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

export default CalendarListComponent;

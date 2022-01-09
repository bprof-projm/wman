import React, { Component } from "react";
import ColumnComponent from "./column.component.jsx";
import { DragDropContext } from "react-beautiful-dnd";
import PrintButton from "../Print-functionality/Print-button/print-button.component";
import ProgressMenu from "../Worker-load/Progress-menu/progress-menu.component";
import LabelsMenu from "../Labels/LabelMenu/labelMenu";
import { Logout } from "../Logout/logout.component";
import { Layout, Button } from "antd";
import axios from "axios";
import { LeftOutlined, RightOutlined } from "@ant-design/icons";
import moment from "moment";

import styled from "styled-components";
import "./calendar-list.styles.css";

const { Header, Content } = Layout;

const SiteLayout = styled(Content)`
  padding: 24px 50px;
  margin-top: 64px;
  min-height: 380px;
  display: flex;
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
const HeaderItemsRightSide = styled.div`
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
const HeaderItemsLeftSide = styled.div`
  display: flex;
  align-items: center;
`;

const getEventsForDay = (events, day) => {
  return events
    .filter((event) => new Date(event.estimatedStartDate).getDay() === day)
    .map((event) => event.id);
};

class CalendarListComponent extends Component {
  state = initialData;

  componentDidMount() {
    this.fetchEvents(moment().isoWeek());
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

  fetchEvents = async (week) => {
    const events = await axios
      .get(`/CalendarEvent/GetWeekEvents/${week}`)
      .then((response) => response.data);

    this.setState((state) => ({
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
            <HeaderItems>
              <HeaderItemsRightSide>
                <ProgressMenu />
                <LabelsMenu />
                <PrintButton />
              </HeaderItemsRightSide>
              <HeaderItemsLeftSide>
                <Logout />
              </HeaderItemsLeftSide>
            </HeaderItems>
          </Header>

          <SiteLayout>
            <Button shape="circle" size="large" icon={<LeftOutlined />} onClick={() => this.fetchEvents(this.state.week - 1)}/>
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
            <Button shape="circle" size="large" icon={<RightOutlined />} onClick={() => this.fetchEvents(this.state.week + 1)} />
          </SiteLayout>
        </Layout>
      </div>
    );
  }
}

const initialData = {
  week: null,
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

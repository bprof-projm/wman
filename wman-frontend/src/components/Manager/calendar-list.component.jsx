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

import styled from "styled-components";
import "./calendar-list.styles.css";

const { Header, Content, Footer } = Layout;

const Container = styled.div`
  height: 100%;
  display: flex;
`;

class CalendarListComponent extends Component {
  // constructor(props) {
  //   super(props);
  //   this.state = { initialData: initialData, currentWeek: [] };
  // }
  state = initialData;

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
      const newTaskIds = Array.from(start.taskIds);
      newTaskIds.splice(source.index, 1);
      newTaskIds.splice(destination.index, 0, draggableId);

      const newColumn = {
        ...start,
        taskIds: newTaskIds,
      };

      const newState = {
        ...this.state,
        columns: {
          ...this.state.columns,
          [newColumn.id]: newColumn,
        },
      };

      this.setState(newState);
      return;
    }

    //Moving from one list to another
    const startTaskIds = Array.from(start.taskIds);
    startTaskIds.splice(source.index, 1);
    const newStart = {
      ...start,
      taskIds: startTaskIds,
    };

    const finishTaskIds = Array.from(finish.taskIds);
    finishTaskIds.splice(destination.index, 0, draggableId);
    const newFinish = {
      ...finish,
      taskIds: finishTaskIds,
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

  // generateListOfCurrentWeek() {
  //   let curr = new Date();
  //   let week = [];

  //   for (let i = 1; i <= 7; i++) {
  //     let first = curr.getDate() - curr.getDay() + i;
  //     let day = new Date(curr.setDate(first)).toISOString().slice(0, 10);
  //     week.push(day);
  //   }

  //   this.setState({ currentWeek: week });
  // }

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
                  {this.state.columnOrder.map((columnId) => {
                    const column = this.state.columns[columnId];
                    const tasks = column.taskIds.map(
                      (taskId) => this.state.tasks[taskId]
                    );

                    return (
                      <ColumnComponent
                        key={column.id}
                        column={column}
                        tasks={tasks}
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
  tasks: {
    "task-1": { id: "task-1", content: "task" },
    "task-2": { id: "task-2", content: "task2" },
    "task-3": { id: "task-3", content: "task3" },
    "task-4": { id: "task-4", content: "task4" },
  },
  columns: {
    "column-1": {
      id: "column-1",
      title: "Monday",
      taskIds: ["task-1", "task-2", "task-3", "task-4"],
    },

    "column-2": {
      id: "column-2",
      title: "Tuesday",
      taskIds: [],
    },

    "column-3": {
      id: "column-3",
      title: "Wednesday",
      taskIds: [],
    },

    "column-4": {
      id: "column-4",
      title: "Thursday",
      taskIds: [],
    },

    "column-5": {
      id: "column-5",
      title: "Friday",
      taskIds: [],
    },

    "column-6": {
      id: "column-6",
      title: "Saturday",
      taskIds: [],
    },

    "column-7": {
      id: "column-7",
      title: "Sunday",
      taskIds: [],
    },
  },
  columnOrder: [
    "column-1",
    "column-2",
    "column-3",
    "column-4",
    "column-5",
    "column-6",
    "column-7",
  ],
};

export default CalendarListComponent;

import React, { Component } from "react";
import ReactDOM from "react-dom";
import ColumnComponent from "./column.component.jsx";
import { DragDropContext } from "react-beautiful-dnd";
import styled from "styled-components";

const Container = styled.div`
  height: 99vh;
  display: flex;
`;

class CalendarListComponent extends Component {
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

  render() {
    return (
      <DragDropContext onDragEnd={this.onDragEnd}>
        <Container>
          {this.state.columnOrder.map((columnId) => {
            const column = this.state.columns[columnId];
            const tasks = column.taskIds.map(
              (taskId) => this.state.tasks[taskId]
            );

            return (
              <ColumnComponent key={column.id} column={column} tasks={tasks} />
            );
          })}
        </Container>
      </DragDropContext>
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

import React, { Component } from "react";
import styled from "styled-components";
import Task from "./task.component.jsx";
import { Droppable } from "react-beautiful-dnd";

const Container = styled.div`
  margin: 8px;
  border: 1px solid lightgray;
  border-radius: 2px;
  width: calc(100% / 7);
  background-color: ${(props) =>
    props.droppableId === "column-6" ? "skyblue" : "lightgrey"};

  dispaly: flex;
  flex-direction: column;
`;
const Title = styled.h3`
  padding: 8px;
  text-align: center;
`;
const TaskList = styled.div`
  padding: 8px;
  transition: background-color 0.2s ease;
  background-color: ${(props) => (props.isDraggingOver ? "skyblue" : "white")};
  flex-grow: 1;
  height: 88vh;
`;

class ColumnComponent extends Component {
  render() {
    return (
      <Container>
        <Title>{this.props.column.title}</Title>
        <Droppable droppableId={this.props.column.id}>
          {(provided, snapshot) => (
            <TaskList
              ref={provided.innerRef}
              {...provided.droppableProps}
              isDraggingOver={snapshot.isDraggingOver}
            >
              {this.props.tasks.map((task, index) => (
                <Task key={task.id} task={task} index={index} />
              ))}
              {provided.placeholder}
            </TaskList>
          )}
        </Droppable>
      </Container>
    );
  }
}

export default ColumnComponent;

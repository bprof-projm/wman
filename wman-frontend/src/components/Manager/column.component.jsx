import React, { Component } from "react";
import styled from "styled-components";
import Event from "./event.component.jsx";
import { Droppable } from "react-beautiful-dnd";

const Container = styled.div`
  margin: 8px;
  border-radius: 2px;
  width: calc(100% / 7);

  dispaly: flex;
  flex-direction: column;
  height: 89vh;
  overflow: auto;
`;
const Title = styled.h3`
  text-align: center;
  font-size: 22px;
  font-weight: bold;
`;
const TaskList = styled.div`
  padding: 10px;
  transition: background-color 0.2s ease;
  background-color: rgb(245, 246, 248);
  flex-grow: 1;
  height: 83vh;
  border-radius: 16px;
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
              {this.props.events.map((event, index) => (
                <Event key={event.id} event={event} index={index} />
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

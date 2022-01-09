import React, { Component } from "react";
import styled from "styled-components";
import { Draggable } from "react-beautiful-dnd";
import { Card, Avatar, Tooltip } from "antd";
import Label from "../Labels/Label";
import Moment from "react-moment";

import "./event.styles.css";

// const Container = styled.div`
//   border: 1px solid lightgray;
//   border-radius: 2px;
//   padding: 8px;
//   margin-bottom: 8px;
//   background-color: ${(props) => (props.isDragging ? "lightgreen" : "white")};
// `;
const { Meta } = Card;

const LabelList = styled.div`
  margin-bottom: 15px;
  & span {
    margin-bottom: 8px;
  }
`;

class Event extends Component {
  render() {
    return (
      <Draggable
        draggableId={String(this.props.event.id)}
        index={this.props.index}
      >
        {(provided, snapshot) => (
          <div
            {...provided.draggableProps}
            {...provided.dragHandleProps}
            ref={provided.innerRef}
            isDragging={snapshot.isDragging}
          >
            <Card
              className="cardEvent"
              onClick={() => this.props.onCardClick(this.props.event.id)}
            >
              <Meta
                title={this.props.event.jobDescription}
                description={
                  <>
                    <Moment format="HH:mm">
                      {this.props.event.estimatedStartDate}
                    </Moment>
                    <span> - </span>
                    <Moment format="HH:mm">
                      {this.props.event.estimatedFinishDate}
                    </Moment>
                  </>
                }
                style={{ "margin-bottom": "15px" }}
              />
              <LabelList>
                {this.props.event.labels.map((label) => (
                  <Label
                    key={label.id}
                    name={label.content}
                    backgroundColor={label.backgroundColor}
                    textColor={label.textColor}
                  />
                ))}
              </LabelList>
              <Meta
                avatar={
                  <Avatar.Group
                    maxCount={2}
                    maxStyle={{
                      color: "#f56a00",
                      backgroundColor: "#fde3cf",
                    }}
                  >
                    {this.props.event.assignedUsers.map((user) => (
                      <Avatar
                        src={
                          user.profilePicture
                            ? user.profilePicture.url
                            : `https://eu.ui-avatars.com/api?name=${encodeURIComponent(
                                user.firstname + " " + user.lastname
                              )}`
                        }
                      />
                    ))}
                  </Avatar.Group>
                }
              />
            </Card>
          </div>
        )}
      </Draggable>
    );
  }
}

export default Event;

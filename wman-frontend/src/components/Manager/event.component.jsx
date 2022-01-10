import React, { Component } from "react";
import styled from "styled-components";
import { Draggable } from "react-beautiful-dnd";
import { Card, Avatar, Badge, Tooltip } from "antd";
import { SyncOutlined, CheckOutlined } from "@ant-design/icons";
import Label from "../Labels/Label";
import Moment from "react-moment";

import "./event.styles.css";

const { Meta } = Card;

const LabelList = styled.div`
  margin-bottom: 15px;
  & span {
    margin-bottom: 8px;
  }
`;

const Background = styled.div`
  background-color: ${(props) => props.color};
  color: white;
  border-radius: 100px;
  padding: 3px;
  display: flex;
  justify-content: center;
  align-items: center;
`;

class Event extends Component {
  getBadgeContent = () => {
    const status = this.props.event.status;
    const diplayName =
      status === "awaiting"
        ? "Awaiting"
        : status === "started"
        ? "Started"
        : status === "proofawait"
        ? "Wainting for proof"
        : status === "finished"
        ? "Finished"
        : "Unknown";
  
    switch (status) {
      case "started":
      case "proofawait":
        return (
          <Tooltip title={diplayName}>
            <Background color="#fa8c16">
              <SyncOutlined />
            </Background>
          </Tooltip>
        );
      case "finished":
        return (
          <Tooltip title={diplayName}>
            <Background color="#52c41a">
              <CheckOutlined />
            </Background>
          </Tooltip>
        );
      case "awaiting":
      default:
        return null;
    }
  };

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
            <Badge count={this.getBadgeContent()} offset={[-5, 5]}>
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
            </Badge>
          </div>
        )}
      </Draggable>
    );
  }
}

export default Event;

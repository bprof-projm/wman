import React from "react";
import { Tag } from "antd";

export default function Label(props) {
  return (
    <Tag
      color={props.backgroundColor}
      //   onClick={props.showEditLabel(props.name, props.backgroundColor, props.id)}
    >
      {props.name}
    </Tag>
  );
}

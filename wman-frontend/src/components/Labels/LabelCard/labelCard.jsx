import React, { useEffect } from "react";
import "./labelCard.css";
import { DeleteOutlined } from "@ant-design/icons";
import { Button } from "antd";
import axios from "axios";
import Cookies from "js-cookie";
//props needed: src, name, percent (workload)
const LabelCard = (props) => {
  return (
    <div className="label-card-container">
      <div
        className="label-card"
        style={{ backgroundColor: props.backgroundColor }}
        onClick={props.showEditLabel(
          props.name,
          props.backgroundColor,
          props.id
        )}
      >
        <div className="card-name">
          <h4 style={{ color: props.textColor }}>{props.name}</h4>
        </div>
      </div>
      <Button
        shape="circle"
        className="card-delete"
        icon={<DeleteOutlined />}
        onClick={props.deleteLabel(props.id)}
      />
    </div>
  );
};

export default LabelCard;

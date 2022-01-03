import React from "react";
import LabelCard from "../LabelCard/labelCard";
import "./labelList.css";
//props needed objects(list)
const LabelsList = (props) => {
  return (
    <div className="label-card-list">
      {props.objects.map((label) => (
        //props needed: src, name, percent (workload)
        <LabelCard
          key={label.id}
          name={label.content}
          backgroundColor={label.backgroundColor}
          textColor={label.textColor}
          id={label.id}
          deleteLabel={props.deleteLabel}
          showEditLabel={props.showEditLabel}
        />
      ))}
    </div>
  );
};

export default LabelsList;

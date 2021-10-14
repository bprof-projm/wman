import React from 'react';
import ReactDOM from 'react-dom';
import 'antd/dist/antd.css';

export const MyEvent = (props) => {
  console.log("MyEventConsoleLog");
  console.log(props); 

  return (
    <div>
      <div>
        <h2>{props.object.JobDescription}</h2>
        <p>{props.object.Id}</p>
        <p>{props.object.EstimatedStartDate}</p>
        <p>{props.object.EstimatedFinishDate}</p>
        <button>Modify</button>
        <button>Delete</button>
        <button>Cancel</button>
      </div>
    </div>
  );
}
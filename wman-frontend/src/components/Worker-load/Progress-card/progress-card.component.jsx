import { Avatar, Progress } from "antd";
import React, { useEffect } from "react";
import './progress-card.styles.css'
import logo from "../../../avatar-logo/wman_profpic_placeholder.png"

//props needed: src, name, percent (workload)
const ProgressCard = (props) => {
    let image = ""
    if( props.src.length == 0){
        image = logo
    }
    else{
        image = props.src;
    }

    return (
        <div className="progress-card-container">
            <div className="card-avatar">
                <img src= {image} />
            </div>

            <div className="card-name" >
                <h4>{props.name}</h4>
            </div>

            <div className="card-progress-circle">
                <Progress
                    strokeColor={{ '0%': '#108ee9', '100%': '#87d068', }}
                    type="circle"
                    percent={props.percent}
                    width={68}
                />
            </div>
        </div>
    )
}

export default ProgressCard
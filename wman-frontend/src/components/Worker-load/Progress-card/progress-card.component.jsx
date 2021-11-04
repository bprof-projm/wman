import { Avatar, Progress } from "antd";
import React from "react";
import './progress-card.styles.css'

//props needed: src, name, percent (workload)
const ProgressCard = (props) => {
    return (
        <div className="progress-card-container">
            <div>
                {/*<Avatar src="https://joeschmoe.io/api/v1/random" size="large"/>*/}
                <Avatar src={props.src} size="large"/>
            </div>

            <div>
                {props.name}
            </div>

            <div>
                <Progress
                    strokeColor={{ '0%': '#108ee9', '100%': '#87d068', }}
                    type="circle"
                    percent={props.percent}
                    width={50}
                     />
            </div>
        </div>
    )
}

export default ProgressCard
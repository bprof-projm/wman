import { Avatar, Progress } from "antd";
import React from "react";
import './progress-card.styles.css'

const ProgressCard = () => {
    return (
        <div className="progress-card-container">
            <div>
                <Avatar src="https://joeschmoe.io/api/v1/random" />
            </div>

            <div>
                John Doe
            </div>

            <div>
                <Progress
                    strokeColor={{ '0%': '#108ee9', '100%': '#87d068', }}
                    type="circle"
                    percent={70}
                    width={50}
                     />
            </div>
        </div>
    )
}

export default ProgressCard
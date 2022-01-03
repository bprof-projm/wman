import React, { useEffect } from "react";
import "./one-label.styles.css";

const OneLabel = (props) => {
    return (
        <div className="one-label"
            style={{ backgroundColor: props.backgroundColor }}>

            <div className="label-name">
                <h4 style={{ color: props.textColor }}>{props.name}</h4>
            </div>
        </div>
    );
};

export default OneLabel;
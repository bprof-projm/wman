import React from 'react';
import ProgressCard from '../Worker-load/Progress-card/progress-card.component';
import ProgressMenu from '../Worker-load/Progress-menu/progress-menu.component';
import "./temp.styles.css";

export const TempPage = () => {
    
    return (
        <div className="temp">
            <h1>Ideiglenes oldal bemutatáshoz</h1>
            <div className="small">
            <ProgressCard src="s"/></div>
            <ProgressMenu/>
        </div>
    )
};
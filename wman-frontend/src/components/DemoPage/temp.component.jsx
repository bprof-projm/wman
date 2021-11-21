import React from 'react';
import { Logout } from '../Logout/logout.component';
import PrintButton from '../Print-functionality/Print-button/print-button.component';
import ProgressCard from '../Worker-load/Progress-card/progress-card.component';
import ProgressMenu from '../Worker-load/Progress-menu/progress-menu.component';
import "./temp.styles.css";

export const TempPage = () => {
    
    return (
        <div className="temp">
            <h1>Ideiglenes oldal bemutatáshoz</h1>
            <ProgressMenu/>
            <br/>
            <PrintButton/>
            <br/>
            <Logout/>
        </div>
    )
};
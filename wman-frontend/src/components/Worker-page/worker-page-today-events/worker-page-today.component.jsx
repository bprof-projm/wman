import React from "react";
import Cookies from "js-cookie";
import jwt_decode from "jwt-decode";
import { useState, useEffect } from "react";
import axios from "axios";
import { SmileTwoTone } from '@ant-design/icons';
import "./worker-page-today.styles.css"
import OneDayEvents from "../one-day-event/one-day-events.component";
import ProgressCard from "../../Worker-load/Progress-card/progress-card.component";


const WorkerToday = () => {

    const token = Cookies.get("auth");
    const decoded = jwt_decode(token);
    const [calendarEvents, setCalendarEvents] = useState([]);
    const [workload, setWorkLoad] = useState();


    useEffect(() => {
        axios.get(`/Worker/myworkload`)
            .then(response => setWorkLoad(response.data))
            .catch(error => alert(error))
    }, [axios]);

    useEffect(() => {
        axios.get(`/CalendarEvent/GetCurrentDayEvents`)
            .then(response => setCalendarEvents(response.data))            
            .catch(error => alert(error))
    }, [axios]);


    const filteredEvents = calendarEvents.filter(
        event => event.assignedUsers.map(assigned => assigned.username).includes(decoded.sub));

    const showEmptyMessage = (filteredEvents.length === 0);
    

    return (
        <div>
            <div className="events-of-the-day">
                {(showEmptyMessage)
                    ? null
                    : <ProgressCard
                        key={workload.userID}
                        src={workload.profilePicUrl}
                        name={workload.username}
                        percent={workload.percent} />                        
                    }

                <div className="one-day">
                    {showEmptyMessage
                        ? <div><h2>There are no jobs for today, go home! </h2> <br /> <h1><SmileTwoTone twoToneColor="lightgreen" /></h1></div>
                        : filteredEvents.map(oneEvent => <OneDayEvents key={oneEvent.id} event={oneEvent} />)}
                </div>
            </div>
        </div>
    )
}
export default WorkerToday;
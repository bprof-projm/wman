import "./worker-page-this-week-events.styles.css"
import React from "react";
import Cookies from "js-cookie";
import jwt_decode from "jwt-decode";
import { useState, useEffect } from "react";
import axios from "axios";
const WorkerThisWeek = () => {
    const token = Cookies.get("auth");
    const decoded = jwt_decode(token);
    const [calendarEvents, setCalendarEvents] = useState([]);

    useEffect(() => {
        axios.get(`/CalendarEvent/GetCurrentDayEvents`)
            .then(response => setCalendarEvents(response.data))
            .then(sort())
            .catch(error => alert(error))
    }, [axios]);

    const filteredEvents = calendarEvents.filter(
        event => event.assignedUsers.map(assigned => assigned.username).includes(decoded.sub));

    const showEmptyMessage = (filteredEvents.length === 0);

    function sort(){
        if (showEmptyMessage === false) {

            const today = new Date(filteredEvents[0].estimatedStartDate);
            const dayName = today.toString().split(' ')[0];
            console.log(dayName); //Mon,Tue,Wed,Thu,Fri,Sat,Sun

            if (dayName == 'Wed') {
                const wednesdayEvents = 'valami'
            }

        }
    }


    return (
        <div>
            <div className="events-of-the-week">
                <div className="week-day">
                    Monday
                    <div className="weekday-events">
                        list of the events of the day
                    </div>
                </div>
                <div className="week-day">
                    Tuesday
                    <div className="weekday-events">
                        list of the events of the day
                    </div>
                </div>
                <div className="week-day">
                    Wednesday
                    <div className="weekday-events">
                        list of the events of the day
                        {showEmptyMessage
                            ? <div><h2>There are no jobs for today, go home! </h2> <br /> </div>
                            : <div>{wednesdayEvents[0]}</div>}
                            {sort()}
                    </div>
                </div>
                <div className="week-day">
                    Thursday
                    <div className="weekday-events">
                        list of the events of the day
                    </div>
                </div>
                <div className="week-day">
                    Friday
                    <div className="weekday-events">
                        list of the events of the day
                    </div>
                </div>
                <div className="week-day">
                    Saturday
                    <div className="weekday-events">
                        list of the events of the day
                    </div>
                </div>
                <div className="week-day">
                    Sunday
                    <div className="weekday-events">
                        list of the events of the day
                    </div>
                </div>
            </div>
        </div>
    )
}
export default WorkerThisWeek;
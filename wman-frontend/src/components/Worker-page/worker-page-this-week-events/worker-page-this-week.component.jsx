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
        axios.get(`/CalendarEvent/GetCurrentWeekEvents`)
            .then(response => setCalendarEvents(response.data))
            .catch(error => alert(error))
    }, [axios]);


    const filteredEvents = calendarEvents.filter(
        event => event.assignedUsers.map(assigned => assigned.username).includes(decoded.sub));

    const mondayEvents = filteredEvents.filter(event => isItThisDay(event, "Mon"));
    const tuesdayEvents = filteredEvents.filter(event => isItThisDay(event, "Tue"));
    const wednesdayEvents = filteredEvents.filter(event => isItThisDay(event, "Wed"));
    const thursdayEvents = filteredEvents.filter(event => isItThisDay(event, "Thu"));
    const fridayEvents = filteredEvents.filter(event => isItThisDay(event, "Fri"));
    const saturdayEvents = filteredEvents.filter(event => isItThisDay(event, "Sat"));
    const sundayEvents = filteredEvents.filter(event => isItThisDay(event, "Sun"));

    function isItThisDay(eventToExamine, dayYouWant) {

        const today = new Date(eventToExamine.estimatedStartDate);
        const dayName = today.toString().split(' ')[0];
        console.log(dayName); //Mon,Tue,Wed,Thu,Fri,Sat,Sun

        if (dayName == dayYouWant) {
            return true;
        }
        else {
            
            console.log(dayYouWant + " not good");
            return false;
            
        }
    }
    


    return (
        <div>
            <div className="events-of-the-week">
                <div className="week-day">
                    Monday
                    <div className="weekday-events">
                        {(mondayEvents.length ===  0)
                            ? <div><h2>There are no jobs for today, go home! </h2> <br /> </div>
                            : mondayEvents.map(event => <div key={event.id}>{event.jobDescription}</div>)}
                    </div>
                </div>
                <div className="week-day">
                    Tuesday
                    <div className="weekday-events">
                        {(tuesdayEvents.length ===  0)
                            ? <div><h2>There are no jobs for today, go home! </h2> <br /> </div>
                            : tuesdayEvents.map(event => <div key={event.id}>{event.jobDescription}</div>)}
                    </div>
                </div>
                <div className="week-day">
                    Wednesday
                    <div className="weekday-events">
                        {(wednesdayEvents.length ===  0)
                            ? <div><h2>There are no jobs for today, go home! </h2> <br /> </div>
                            : wednesdayEvents.map(event => <div key={event.id}>{event.jobDescription}</div>)}
                    </div>
                </div>
                <div className="week-day">
                    Thursday
                    <div className="weekday-events">
                        {(thursdayEvents.length ===  0)
                            ? <div><h2>There are no jobs for today, go home! </h2> <br /> </div>
                            : thursdayEvents.map(event => <div key={event.id}>{event.jobDescription}</div>)}
                    </div>
                </div>
                <div className="week-day">
                    Friday
                    <div className="weekday-events">
                        {(fridayEvents.length ===  0)
                            ? <div><h2>There are no jobs for today, go home! </h2> <br /> </div>
                            : fridayEvents.map(event => <div key={event.id}>{event.jobDescription}</div>)}
                    </div>
                </div>
                <div className="week-day">
                    Saturday
                    <div className="weekday-events">
                        {(saturdayEvents.length ===  0)
                            ? <div><h2>There are no jobs for today, go home! </h2> <br /> </div>
                            : saturdayEvents.map(event => <div key={event.id}>{event.jobDescription}</div>)}
                    </div>
                </div>
                <div className="week-day">
                    Sunday
                    <div className="weekday-events">
                        {(sundayEvents.length ===  0)
                            ? <div><h2>There are no jobs for today, go home! </h2> <br /> </div>
                            : sundayEvents.map(event => <div key={event.id}>{event.jobDescription}</div>)}
                    </div>
                </div>
            </div>
        </div>
    )
}
export default WorkerThisWeek;
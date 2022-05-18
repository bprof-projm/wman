import "./worker-page-this-week-events.styles.css"
import React from "react";
import Cookies from "js-cookie";
import jwt_decode from "jwt-decode";
import { useState, useEffect } from "react";
import axios from "axios";
import OneDayEvents from "../one-day-event/one-day-events.component";

import {  HubConnectionBuilder, LogLevel, HttpTransportType } from '@microsoft/signalr';

const WorkerThisWeek = (props) => {
    const token = Cookies.get("auth");
    const decoded = jwt_decode(token);
    const [calendarEvents, setCalendarEvents] = useState([]);
    const [update,setUpdate] = useState(false);
    const [connection, setConnection] = useState();

    useEffect(() => {
        axios.get(`/CalendarEvent/GetCurrentWeekEvents`)
            .then(response => setCalendarEvents(response.data))
            .catch(error => alert(error))
    }, [update]);

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('https://localhost:5001/notify/', { skipNegotiation:true ,accessTokenFactory: () => Cookies.get("auth"),
             withCredentials: true, transport: HttpTransportType.WebSockets })
            .configureLogging(LogLevel.Information)
            .withAutomaticReconnect()
            .build();
        console.log(newConnection);
        setConnection(newConnection);
    }, [update]);

    useEffect(() => {
        if (connection) {
            connection.start()
                .then(result => {
                    
                    connection.on('Connected', () => console.log('Connected to signalR'));
                    connection.on('Disconnected', () => console.log('disconnected signalR'));
                    
                    connection.on('UserAssigned', (args) => notifyUser(args));
                    connection.on('EventChanged', (args) => notifyUser(args));
                    connection.on('EventStateChanged', (args) => notifyUser(args));

                    connection.on('UserAssignedCurrentDay', (args) => notifyUses(args));
                    connection.on('EventChangedForToday', (args) => notifyUser(args));
                    connection.on('EventChangedFromTodayToNotToday', (args) => notifyUser(args));
                })
                .catch(e => console.log('Connection failed: ', e));
        }
    }, [connection]);

    const notifyUser = (args) => {
        setUpdate(!update);
        console.log(calendarEvents)
        console.log(args)
        //setCalendarEvents(arr => [...arr,args])
    }

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
        const dayName = today.toString().split(' ')[0]; //Mon,Tue,Wed,Thu,Fri,Sat,Sun

        if (dayName == dayYouWant) {
            return true;
        }
        else {
            return false;
        }
    }

    return (
        <div>
            <div className="events-of-the-week">
                <div className="week-day">
                    Monday
                    <div className="weekday-events">
                        {(mondayEvents.length === 0)
                            ? <div><h2>There are no jobs for this day! </h2> <br /> </div>
                            : mondayEvents.map(event => <OneDayEvents key={event.id} event={event} />)}
                    </div>
                </div>
                <div className="week-day">
                    Tuesday
                    <div className="weekday-events">
                        {(tuesdayEvents.length === 0)
                            ? <div><h2>There are no jobs for this day! </h2> <br /> </div>
                            : tuesdayEvents.map(event => <OneDayEvents key={event.id} event={event} />)}
                    </div>
                </div>
                <div className="week-day">
                    Wednesday
                    <div className="weekday-events">
                        {(wednesdayEvents.length === 0)
                            ? <div><h2>There are no jobs for this day! </h2> <br /> </div>
                            : wednesdayEvents.map(event => <OneDayEvents key={event.id} event={event} />)}
                    </div>
                </div>
                <div className="week-day">
                    Thursday
                    <div className="weekday-events">
                        {(thursdayEvents.length === 0)
                            ? <div><h2>There are no jobs for this day! </h2> <br /> </div>
                            : thursdayEvents.map(event => <OneDayEvents key={event.id} event={event} />)}
                    </div>
                </div>
                <div className="week-day">
                    Friday
                    <div className="weekday-events">
                        {(fridayEvents.length === 0)
                            ? <div><h2>There are no jobs for this day! </h2> <br /> </div>
                            : fridayEvents.map(event => <OneDayEvents key={event.id} event={event} />)}
                    </div>
                </div>
                <div className="week-day">
                    Saturday
                    <div className="weekday-events">
                        {(saturdayEvents.length === 0)
                            ? <div><h2>There are no jobs for this day! </h2> <br /> </div>
                            : saturdayEvents.map(event => <OneDayEvents key={event.id} event={event} />)}
                    </div>
                </div>
                <div className="week-day">
                    Sunday
                    <div className="weekday-events">
                        {(sundayEvents.length === 0)
                            ? <div><h2>There are no jobs for this day! </h2> <br /> </div>
                            : sundayEvents.map(event => <OneDayEvents key={event.id} event={event} />)}
                    </div>
                </div>
            </div>
        </div>
    )
}
export default WorkerThisWeek;
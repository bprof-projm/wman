import React, { useEffect, useState } from 'react';
import { Cascader } from 'antd';
import jwt_decode from 'jwt-decode';
import axios from 'axios';
import DocumentToPrint from '../Document-to-print/document-to-print.component';
import Cookies from "js-cookie";
import './print-events.styles.css';

const PrintEvents = () => {
    const [users, setUsers] = useState([]);

    const [calendarEvents, setCalendarEvents] = useState([]);    
    const [searched, setSearched] = useState("");

    useEffect(() => {
        axios.get(`/User/workload`)
            .then(response => setUsers(response.data))
            .catch(error => console.log(error));
    }, [axios]);

    useEffect(() => {
        axios.get(`/CalendarEvent/GetCurrentDayEvents`)
            .then(response => setCalendarEvents(response.data))
            .catch(error => console.log(error))
    }, [axios]);

   const handleChange = (e) => {
        setSearched(e.target.value);       
    }
    
    const filteredEvents = calendarEvents.filter(        
        event => event.assignedUsers[0].username.includes(searched)
    );
    return (
        <div className='print-events'>
            <div className='print-events-prewiew'>
                <DocumentToPrint eventlist={filteredEvents} />
            </div>
            <div className='print-events-instructions'>
                <br />
                <br />
                <label>Choose the user!</label><br/>
                <select onChange={handleChange} name="subject" id="subject">
                    <option value="" selected disabled hidden>Every worker</option>
                    {users.map(user =>
                        <option key={user.userID} value={user.username}>{user.username}</option>
                    )}
                </select>
            </div>
        </div>
    )
}

export default PrintEvents;


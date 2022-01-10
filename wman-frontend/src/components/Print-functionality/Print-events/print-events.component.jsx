import React, { useEffect, useState } from 'react';
import { Button } from 'antd';
import axios from 'axios';
import DocumentToPrint from '../Document-to-print/document-to-print.component';
import './print-events.styles.css';
import { PrinterOutlined } from '@ant-design/icons';

const PrintEvents = () => {
    const [users, setUsers] = useState([]);

    const [calendarEvents, setCalendarEvents] = useState([]);
    const [searched, setSearched] = useState("");

    useEffect(() => {
        axios.get(`/User/workload`)
            .then(response => setUsers(response.data))
            .catch(error => alert(error));
    }, [axios]);

    useEffect(() => {
        axios.get(`/CalendarEvent/GetCurrentDayEvents`)
            .then(response => setCalendarEvents(response.data))
            .catch(error => alert(error))
    }, [axios]);

    const handleChange = (e) => {
        setSearched(e.target.value);
    }

    const filteredEvents = calendarEvents.filter(
        event => event.assignedUsers.map(assigned => assigned.username).includes(searched)
    );
    const printWindow = ()=>{
        window.print();
    }

    return (
        <div className='print-events'>
            <div className='print-events-prewiew'>
                <DocumentToPrint eventlist={filteredEvents} />
            </div>
            <div className='print-events-instructions'>
                <br />
                <br />
                <label>Choose the user!</label><br />
                <select onChange={handleChange} name="subject" id="subject">
                    <option value="" selected disabled hidden>choose here</option>
                    {console.log(filteredEvents)}
                    {users.map(user =>
                        <option key={user.userID} value={user.username}>{user.username}</option>
                    )}
                </select>                
                <Button shape='circle' icon={<PrinterOutlined />} text="Print" onClick ={printWindow}/>
            </div>
        </div>
    )
}

export default PrintEvents;


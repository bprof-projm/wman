import React, { useEffect, useState } from 'react';
import { Cascader } from 'antd';
import jwt_decode from 'jwt-decode';
import axios from 'axios';
import DocumentToPrint from '../To-print/document-to-print.component';
import Cookies from "js-cookie";
import './print-events.styles.css';

const PrintEvents = () => {

    const [events, setEvents] = useState([]);
    const [username, setUsername] = useState("");
    const [users, setUsers] = useState([]);

    useEffect(() => {
        axios.get(`/User/workload`)
            .then(response => setUsers(response.data))
            .catch(error => console.log(error));
    }, [axios]);

    const handleChange = (e) => {
        setUsername(e.target.value);
        console.log(e.target.value);
        axios.get(`/User/workEvents?username=${e.target.value}`)
            .then(response => setEvents(response))
            .catch(error => console.log(error))
    }


    return (
        <div className='print-events'>
            <div className='print-events-instructions'>
                <br />                
                <br />
                <select onChange={handleChange} name="subject" id="subject">
                    <option value="" selected disabled hidden>Choose the user!</option>
                    {users.map(user =>
                        <option key={user.userID} value={user.username}>{user.username}</option>
                    )}
                </select>
            </div>
            <div className='print-events-prewiew'>
                <DocumentToPrint username={username} events={events}/>
            </div>
        </div>
    )
}

export default PrintEvents;


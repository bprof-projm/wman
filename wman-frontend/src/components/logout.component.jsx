import Cookies from 'js-cookie';
import React from 'react';
import { Redirect } from 'react-router-dom';
import { useState } from 'react';




export const Logout = () => {

    function logOut() {
        Cookies.remove("auth");

        //location.href = './login.component.jsx';
        //history.push("/login");
        return (<Redirect to='/./src/components/login.component.jsx' />)
    }

    return (
        <div>
            <h1>valami</h1>
            <button onClick={logOut}>Log Out</button>
        </div>
    )
};
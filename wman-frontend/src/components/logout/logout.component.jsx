import Cookies from 'js-cookie';
import React from 'react';
import { Redirect } from 'react-router-dom';
import { useState } from 'react';
import { useHistory } from 'react-router-dom';

export const Logout = () => {
  const history = useHistory();

  function logOut() {
    Cookies.remove('auth');
    history.push('/login');
  }

  return (
    <div>
      <h1>valami</h1>
      <button onClick={logOut}>Log Out</button>
    </div>
  );
};

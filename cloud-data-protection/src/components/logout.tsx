import React from "react";
import './logout.css';
import {useDispatch} from "react-redux";
import {login, logout} from "../features/userSlice";

const Logout = () => {
    const dispatch = useDispatch();

    const handleLogout = () => {
        dispatch(logout());
    }

    return (
        <div className='logout'>
            <h1>Welcome, <span className='user__name'>Olivier</span></h1>
            <button className='logout__button' onClick={(e) => handleLogout()}>
                Log out
            </button>
        </div>
    )
}

export default Logout;
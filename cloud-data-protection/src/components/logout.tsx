import React, {useEffect} from "react";
import {AuthService} from "../services/authService";
import {useSnackbar} from 'notistack';

const Logout = () => {
    const authService = new AuthService();

    const { enqueueSnackbar } = useSnackbar();

    const handleLogout = () => {
        authService.logout();

        enqueueSnackbar('You are now logged out', { autoHideDuration: 6000 })
    }

    useEffect(() => {
        handleLogout();
    })

    return (
        <div className='logout'>
        </div>
    )
}

export default Logout;
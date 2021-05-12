import React, {useEffect, useState} from "react";
import {AuthService} from "services/authService";
import {Button, Input, Typography} from "@material-ui/core";
import {useSnackbar} from 'notistack';
import {http} from "common/http";
import snackbarOptions from "common/snackbar/options";
import {CancelTokenSource} from "axios";
import {startLoading, stopLoading} from "common/progress/helper";
import { useHistory } from "react-router-dom";
import './login.css';

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const { enqueueSnackbar } = useSnackbar();

    const history = useHistory();

    const authService = new AuthService();

    let cancelTokenSource: CancelTokenSource;

    useEffect(() => {
        return () => {
            cancelTokenSource?.cancel();
        }
    })

    const handleSubmit = async (e: any) => {
        e.preventDefault();

        startLoading();

        cancelTokenSource = http.CancelToken.source();

        const input = { email, password };

        await authService.login(input, cancelTokenSource.token)
            .then(() => onSuccess())
            .catch((e: any) => onError(e))
            .finally(() => onFinish());
    }

    const onSuccess = () => {
        setEmail('');

        history.push('/');
    }

    const onError = (e: any) => {
        enqueueSnackbar(e, snackbarOptions);
    }

    const onFinish = () => {
        setPassword('');

        stopLoading();
    }

    return (
        <div className='login'>
            <Typography variant='h1' className='login__header'>Log in</Typography>
            <form className='login__form' onSubmit={(e) => handleSubmit(e)}>
                <Input className='login__form__input' autoFocus={true} type="email" placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)}/>
                <Input className='login__form__input' type="password" placeholder="Password" value={password}
                       onChange={(e) => setPassword(e.target.value)}/>
                <Button className='login__form__submit' type="submit" variant='contained' color='primary'>Log in</Button>
            </form>
        </div>
    )
}

export default Login;
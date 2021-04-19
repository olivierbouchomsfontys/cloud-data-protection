import React, {useState} from "react";
import "./register.css";
import {AuthService} from "../services/authService";
import {Button, Input, Typography} from "@material-ui/core";
import {useSnackbar} from 'notistack';
import {http} from "../common/http";
import snackbarOptions from "../common/snackbar/options";
import { useHistory } from "react-router-dom";

const Register = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [repeatPassword, setRepeatPassword] = useState('');

    const { enqueueSnackbar } = useSnackbar();

    const history = useHistory();

    const authService = new AuthService();

    const handleSubmit = async (e: any) => {
        e.preventDefault();

        const cancelToken = http.CancelToken.source().token;

        const input = { email, password };

        if (password !== repeatPassword) {
            onError('Entered passwords do not match');
            return;
        }

        await authService.register(input, cancelToken)
            .then(() => onSuccess())
            .catch((e: any) => onError(e))
            .finally(() => onFinish());
    }

    const onSuccess = () => {
        setEmail('');

        enqueueSnackbar('Your account has been created. You can now log in using the specified credentials', snackbarOptions);

        history.push("/login");
    }

    const onError = (e: any) => {
        enqueueSnackbar(e, snackbarOptions);
    }

    const onFinish = () => {
        setPassword("");
        setRepeatPassword('');
    }

    return (
        <div className='register'>
            <Typography variant='h1' className='register__header'>Create an account</Typography>
            <form className='register__form' onSubmit={(e) => handleSubmit(e)}>
                <Input className='register__form__input' autoFocus={true} type="email" placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)}/>
                <Input className='register__form__input' type="password" placeholder="Password" value={password}
                       onChange={(e) => setPassword(e.target.value)} autoComplete="new-password"/>
                <Input className='register__form__input' type="password" placeholder="Repeat password" value={repeatPassword}
                       onChange={(e) => setRepeatPassword(e.target.value)}/>
                <Button className='register__form__submit' type="submit" color='primary'>Register</Button>
            </form>
        </div>
    )
}

export default Register;

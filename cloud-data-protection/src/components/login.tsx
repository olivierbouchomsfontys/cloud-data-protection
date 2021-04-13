import React, {useState} from "react";
import {useDispatch} from "react-redux";
import { login } from '../features/userSlice';
import './login.css';

const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const dispatch = useDispatch();

    const handleSubmit = (e: any) => {
        e.preventDefault();

        dispatch(login({
            email: email,
            password: password,
            loggedIn: true
        }))
    }

    return (
        <div className='login'>
            <h1>Log in</h1>
            <form className='login__form' onSubmit={(e) => handleSubmit(e)}>
                <input type="email" placeholder="email" value={email} onChange={(e) => setEmail(e.target.value)}/>
                <input type="password" placeholder="password" value={password}
                       onChange={(e) => setPassword(e.target.value)}/>
                <button type="submit">Log in</button>
            </form>
        </div>
    )
}

export default Login;
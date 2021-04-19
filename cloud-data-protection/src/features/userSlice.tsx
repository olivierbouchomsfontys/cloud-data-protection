import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {LoginResult} from "../services/result/loginResult";
import User from "../entities/user";

interface UserSliceState {
    user?: User;
    token?: string;
}

const initialState: UserSliceState = ({
    user: undefined,
    token: undefined
})

const user = localStorage.getItem('user') as string;
const token = localStorage.getItem('token')

if (user && token) {
    initialState.user = JSON.parse(user);
    initialState.token = JSON.parse(token).token;
}

export const userSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
        login: ((state, action: PayloadAction<LoginResult>) => {
            state.user = action.payload.user;
            state.token = action.payload.token;

            const tokenObj = { token: action.payload.token };

            localStorage.setItem('user', JSON.stringify(action.payload.user));
            localStorage.setItem('token', JSON.stringify(tokenObj));
        }),
        logout: (state) => {
            state.user = undefined;
            state.token = undefined;

            localStorage.clear();
        }
    }
})

export const {login, logout} = userSlice.actions;
export const selectUser = (state: any) => state.user.user;
export const selectToken = (state: any) => state.user.token;
export const selectAuthenticated = (state: any) => state.user.user?.id !== undefined;

export default userSlice.reducer;

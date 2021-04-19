import {createSlice, PayloadAction} from "@reduxjs/toolkit";
import {LoginResult} from "../services/result/loginResult";

export const userSlice = createSlice({
    name: 'user',
    initialState: {
        user: null as any,
        token: null as any
    },
    reducers: {
        login: ((state, action: PayloadAction<LoginResult>) => {
            state.user = action.payload.user;
            state.token = action.payload.token;
        }),
        logout: (state) => {
            state.user = null;
            state.token = null;
        }
    }
})

export const {login, logout} = userSlice.actions;
export const selectUser = (state: any) => state.user.user;
export const selectToken = (state: any) => state.user.token;
export const selectAuthenticated = (state: any) => state.user.user !== null;

export default userSlice.reducer;

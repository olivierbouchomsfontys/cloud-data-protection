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

export const userSlice = createSlice({
    name: 'user',
    initialState,
    reducers: {
        login: ((state, action: PayloadAction<LoginResult>) => {
            state.user = action.payload.user;
            state.token = action.payload.token;
        }),
        logout: (state) => {
            state.user = undefined;
            state.token = undefined;
        }
    }
})

export const {login, logout} = userSlice.actions;
export const selectUser = (state: any) => state.user.user;
export const selectToken = (state: any) => state.user.token;
export const selectAuthenticated = (state: any) => state.user.user?.id !== undefined;

export default userSlice.reducer;

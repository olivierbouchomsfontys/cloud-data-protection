import {configureStore} from "@reduxjs/toolkit";
import userReducer from 'features/userSlice';
import progressReducer from 'features/progressSlice';

const store = configureStore({
    reducer: {
        progress: progressReducer,
        user: userReducer
    }
});

export default store;

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
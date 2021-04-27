import {createSlice} from "@reduxjs/toolkit";

interface ProgressSliceState {
    loading: boolean;
}

const initialState: ProgressSliceState = ({
    loading: false
})

export const progressSlice = createSlice({
    name: 'progress',
    initialState,
    reducers: {
        start: ((state) => {
            state.loading = true;
        }),
        complete: ((state) => {
            state.loading = false;
        })
    }
})

export const {start, complete} = progressSlice.actions;
export const selectLoading = (state: any) => state.progress.loading;

export default progressSlice.reducer;
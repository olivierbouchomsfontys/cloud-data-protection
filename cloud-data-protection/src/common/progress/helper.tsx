import store from 'stores/Store';
import {start, complete} from 'features/progressSlice';

export const startLoading = () => {
    store.dispatch(start());
}

export const stopLoading = () => {
    store.dispatch(complete());
}


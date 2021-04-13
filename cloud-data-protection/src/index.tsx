import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import './index.css';

import {Provider} from 'react-redux';
import store from './stores/Store';
import {SnackbarProvider} from "notistack";

ReactDOM.render(
    <SnackbarProvider maxSnack={2}>
        <Provider store={store}>
            <BrowserRouter>
                <App />
            </BrowserRouter>
        </Provider>
    </SnackbarProvider>,
  document.getElementById('root')
);

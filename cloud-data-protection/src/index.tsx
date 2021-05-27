import React from 'react';
import ReactDOM from 'react-dom';
import {HashRouter} from 'react-router-dom';
import App from './App';
import {Provider} from 'react-redux';
import store from './stores/Store';
import {SnackbarProvider} from "notistack";
import './index.css';
import './mobile_portrait.css';
import './tablet_portrait.css';
import './tablet_landscape.css';

ReactDOM.render(
    <SnackbarProvider maxSnack={1}>
        <Provider store={store}>
            <HashRouter>
                <App />
            </HashRouter>
        </Provider>
    </SnackbarProvider>,
  document.getElementById('root')
);

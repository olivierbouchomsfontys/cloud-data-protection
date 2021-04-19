import React from 'react';
import {Layout} from './components/layout/layout';
import './App.css';
import Router from "./router";

function App() {
    return (
        <React.Fragment>
            <Layout>
                <Router />
            </Layout>
        </React.Fragment>
    );
}

export default App;

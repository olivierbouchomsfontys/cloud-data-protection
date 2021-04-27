import React from 'react';
import './App.css';
import Router from "./router";
import Layout from "./components/layout/layout";

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

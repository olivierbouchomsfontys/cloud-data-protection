import React from 'react';
import {Layout} from './components/layout';
import './App.css';
import {selectUser} from "./features/userSlice";
import {useSelector} from "react-redux";
import Router from "./router";

function App() {
    const user = useSelector(selectUser);

    return (
        <React.Fragment>
            <Layout>
                <Router />
                {/*<Route exact path='/' component={Home}/>*/}
                {/*<Route exact path='/login' component={Login}/>*/}
                {/*<Route exact path='/logout' component={Logout}/>*/}
            </Layout>
        </React.Fragment>
    );
}

export default App;

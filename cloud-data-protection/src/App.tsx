import React from 'react';
import {Route} from 'react-router';
import {Layout} from './components/layout';
import {Home} from './components/home';
import Login from "./components/login";
import './App.css';
import {selectUser} from "./features/userSlice";
import {useSelector} from "react-redux";
import Logout from "./components/logout";

function App() {
    const user = useSelector(selectUser);

    return (
        <React.Fragment>
            <Layout>
                <Route exact path='/' component={Home}/>
                <Route exact path='/login' component={Login}/>
                <Route exact path='/logout' component={Logout}/>
            </Layout>
        </React.Fragment>
    );
}

export default App;

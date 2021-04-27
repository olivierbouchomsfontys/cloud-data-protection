import React, {Fragment} from "react";
import {Route} from "react-router";
import {Redirect} from "react-router-dom";
import Home from "components/home/home";
import Logout from "components/logout/logout";

const EmployeeRouter = () => {
    return (
        <Fragment>
            <Route exact path='/' component={Home}/>
            <Route exact path='/logout' component={Logout}/>
            <Redirect to="/" />
        </Fragment>
    )
}

export default EmployeeRouter;
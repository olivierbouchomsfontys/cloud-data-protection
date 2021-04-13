import React, {Fragment} from "react";
import {Route} from "react-router";
import {Home} from "../../components/home";
import Logout from "../../components/logout";
import {Redirect} from "react-router-dom";

const EmployeeRouter = () => {
    return (
        <Fragment>
            <Route exact path='/' component={Home}/>
            <Route exact path='/logout' component={Logout}/>
            <Redirect from='*' to='/'/>
        </Fragment>
    )
}

export default EmployeeRouter;
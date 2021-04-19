import React, {Fragment, useEffect} from "react";
import {Route} from "react-router";
import { Redirect } from "react-router-dom";
import {Home} from "../../components/home";
import Logout from "../../components/logout";
import Onboarding from "../../components/onboarding";

const EmployeeRouter = () => {
    useEffect(() => {
        console.log('EmployeeRouter')
    })

    return (
        <Fragment>
            <Route exact path='/' component={Home}/>
            <Route exact path='/logout' component={Logout}/>
            <Redirect to="/" />
        </Fragment>
    )
}

export default EmployeeRouter;
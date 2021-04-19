import React, {Fragment, useEffect} from "react";
import {Route} from "react-router";
import {Home} from "../../components/home";
import Logout from "../../components/logout";
import {Redirect} from "react-router-dom";
import Onboarding from "../../components/onboarding";

const ClientRouter = () => {
    useEffect(() => {
        console.log('ClientRouter')
    })

    return (
        <Fragment>
            <Route exact path='/' component={Home} />
            <Route exact path='/logout' component={Logout} />
            <Route exact path='/onboarding' component={Onboarding} />
            <Redirect to="/" />
        </Fragment>
    )
}

export default ClientRouter;
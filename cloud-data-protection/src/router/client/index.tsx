import React, {Fragment, useEffect} from "react";
import {Route} from "react-router";
import {Home} from "../../components/home/home";
import Logout from "../../components/logout/logout";
import {Redirect} from "react-router-dom";
import OnboardingComponent from "../../components/onboarding/onboarding";

const ClientRouter = () => {
    return (
        <Fragment>
            <Route exact path='/' component={Home} />
            <Route exact path='/logout' component={Logout} />
            <Route exact path='/onboarding' component={OnboardingComponent} />
            <Redirect to="/" />
        </Fragment>
    )
}

export default ClientRouter;
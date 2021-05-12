import React, {Fragment} from "react";
import {Route} from "react-router";
import Home from "components/home/home";
import Logout from "components/logout/logout";
import OnboardingComponent from "components/onboarding/onboarding";
import Demo from "components/demo/demo";

const ClientRouter = () => {
    return (
        <Fragment>
            <Route exact path='/logout' component={Logout} />
            <Route exact path='/onboarding' component={OnboardingComponent} />
            <Route exact path='/demo' component={Demo} />
            <Route exact path='/' component={Home}/>
        </Fragment>
    )
}

export default ClientRouter;
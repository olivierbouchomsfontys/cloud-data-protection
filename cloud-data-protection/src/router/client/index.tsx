import React, {Fragment} from "react";
import {Route} from "react-router";
import Home from "components/home/home";
import Logout from "components/logout/logout";
import Onboarding from "components/onboarding/onboarding";
import Demo from "components/demo/demo";
import Settings from "components/settings/settings";

const ClientRouter = () => {
    return (
        <Fragment>
            <Route exact path='/logout' component={Logout} />
            <Route exact path='/onboarding' component={Onboarding} />
            <Route exact path='/demo' component={Demo} />
            <Route exact path='/settings' component={Settings} />
            <Route exact path='/' component={Home}/>
        </Fragment>
    )
}

export default ClientRouter;
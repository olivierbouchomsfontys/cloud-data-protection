import React, {Fragment} from "react";
import {Route} from "react-router";
import Home from "components/home/home";
import Login from "components/login/login";
import Register from "components/register/register";
import Loading from "components/loading";

const AnonymousRouter = () => {
    return (
        <Fragment>
            <Route exact path='/login' component={Login}/>
            <Route exact path='/register' component={Register}/>
            <Route exact path='/onboarding' component={Loading} />
            <Route exact path='/' component={Home}/>
        </Fragment>
    )
}

export default AnonymousRouter;
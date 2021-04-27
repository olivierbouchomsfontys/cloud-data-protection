import React, {Fragment} from "react";
import {Route} from "react-router";
import Home from "components/home/home";
import Login from "components/login/login";
import Register from "components/register/register";
import {Redirect} from "react-router-dom";

const AnonymousRouter = () => {
    return (
        <Fragment>
            <Route exact path='/' component={Home}/>
            <Route exact path='/login' component={Login}/>
            <Route exact path='/register' component={Register}/>
            <Redirect to="/" />
        </Fragment>
    )
}

export default AnonymousRouter;
import React, {Fragment} from "react";
import {Route} from "react-router";
import {Home} from "../../components/home";
import Login from "../../components/login";
import {Redirect} from "react-router-dom";

const AnonymousRouter = () => {
    return (
        <Fragment>
            <Route exact path='/' component={Home}/>
            <Route exact path='/login' component={Login}/>
            <Redirect from='*' to='/'/>
        </Fragment>
    )
}

export default AnonymousRouter;
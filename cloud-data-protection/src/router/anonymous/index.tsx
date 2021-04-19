import React, {Fragment, useEffect} from "react";
import {Route} from "react-router";
import {Home} from "../../components/home";
import Login from "../../components/login";
import Register from "../../components/register";
import {Redirect} from "react-router-dom";

const AnonymousRouter = () => {
    useEffect(() => {
        console.log('AnonymousRouter')
    })

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
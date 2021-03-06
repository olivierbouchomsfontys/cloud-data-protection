import React, { Fragment } from "react";
import {useSelector} from "react-redux";
import {selectAuthenticated, selectUser} from "features/userSlice";
import ClientRouter from "./client";
import EmployeeRouter from "./employee";
import AnonymousRouter from "./anonymous";
import UserRole from "entities/userRole";
import {Switch} from "react-router-dom";
import {Route} from "react-router";
import ConfirmEmailChange from "components/auth/changeEmail/confirmChangeEmail";

const Router = () => {
    const user = useSelector(selectUser);
    const authenticated = useSelector(selectAuthenticated);

    return (
        <Fragment>
                <Switch>
                    <Route exact path='/ConfirmEmailChange' component={ConfirmEmailChange}/>
                    {
                        authenticated ?
                            user.role === UserRole.Client ?
                                <ClientRouter /> :
                                <EmployeeRouter /> :
                            <AnonymousRouter />
                    }
                </Switch>
        </Fragment>
    )
}

export default Router;
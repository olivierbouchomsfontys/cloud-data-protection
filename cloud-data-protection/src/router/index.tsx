import React, { Fragment } from "react";
import {useSelector} from "react-redux";
import {selectAuthenticated, selectUser} from "../features/userSlice";
import ClientRouter from "./client";
import EmployeeRouter from "./employee";
import AnonymousRouter from "./anonymous";

const Router = () => {
    const user = useSelector(selectUser);
    const authenticated = useSelector(selectAuthenticated);

    return (
        <Fragment>
            {
                authenticated ?
                    user.roleName === 'client' ?
                        <ClientRouter /> :
                        <EmployeeRouter /> :
                    <AnonymousRouter />
            }
        </Fragment>
    )
}

export default Router;
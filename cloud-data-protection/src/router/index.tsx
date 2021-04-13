import React, { Fragment } from "react";
import {useSelector} from "react-redux";
import {selectUser} from "../features/userSlice";
import ClientRouter from "./client";
import EmployeeRouter from "./employee";
import AnonymousRouter from "./anonymous";
import {Layout} from "../components/layout";

const Router = () => {
    const user = useSelector(selectUser);

    return (
        <Fragment>
            {
                user?.loggedIn ?
                    user.roleName === 'client' ?
                        <ClientRouter /> :
                        <EmployeeRouter /> :
                    <AnonymousRouter />
            }
        </Fragment>
    )
}

export default Router;
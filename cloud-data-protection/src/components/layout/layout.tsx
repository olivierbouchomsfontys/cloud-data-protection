import React from 'react';
import NavMenu from "components/navMenu/navMenu";
import {LinearProgress} from "@material-ui/core";
import {useSelector} from "react-redux";
import {selectLoading} from "features/progressSlice";

const Layout : React.FunctionComponent = (props) => {
    const loading = useSelector(selectLoading);

    return (
        <div style={{display: 'flex'}}>
            <NavMenu />
            <div className='app__wrapper'>
                <LinearProgress className='app__content__progress'
                                color='secondary'
                                style={loading ? {visibility: 'visible'} : {visibility: 'hidden'}} />
                <div className='app__content'>
                    {props.children}
                </div>
            </div>
        </div>
    )
}

export default Layout;
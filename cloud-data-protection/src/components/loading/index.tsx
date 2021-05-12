import React, {Fragment, useEffect} from "react";
import {startLoading, stopLoading} from "common/progress/helper";

const Loading = () => {
    useEffect(() => {
        startLoading();

        return () => {
            stopLoading();
        }
    }, [])

    return (
        <Fragment/>
    )
}

export default Loading;
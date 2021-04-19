import {Typography} from "@material-ui/core";
import React from "react";

const Onboarding = () => {
    const status = 'Unknown';

    return (
        <div className='onboarding'>
            <Typography variant='h1' className='onboarding__header'>Onboarding</Typography>
            <Typography variant='subtitle2' className='onboarding__status'>Status: {status}</Typography>
        </div>
    )

}

export default Onboarding;
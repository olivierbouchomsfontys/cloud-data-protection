import {Typography} from "@material-ui/core";
import React, {useEffect, useState} from "react";
import OnboardingService from "../../services/onboardingService";
import Onboarding from "../../entities/onboarding";
import snackbarOptions from "common/snackbar/options";
import {useSnackbar} from "notistack";
import OnboardingStatus from "../../entities/onboardingStatus";

const OnboardingComponent = () => {
    const [onboarding, setOnboarding] = useState<Onboarding>();

    const onboardingService = new OnboardingService();

    const { enqueueSnackbar } = useSnackbar();

    useEffect(() => {
        const fetchData = async () => {
            await onboardingService.get()
                .then(result => setOnboarding(result))
                .catch((e: any) => onError(e))
        };

        fetchData();
    }, []);

    const onError = (e: any) => {
        enqueueSnackbar(e.toString(), snackbarOptions);
    }

    return (
        <div className='onboarding'>
            <Typography variant='h1' className='onboarding__header'>Onboarding</Typography>
            {onboarding ?
                <Typography variant='subtitle2' className='onboarding__status'>Status: {OnboardingStatus[onboarding!.status].toLowerCase()}</Typography> :
                <Typography variant='subtitle2' className='onboarding__status'>Status: loading</Typography>
            }
        </div>
    )

}

export default OnboardingComponent;
import {Typography} from "@material-ui/core";
import React, {useEffect, useState} from "react";
import OnboardingService from "../../services/onboardingService";
import Onboarding from "../../entities/onboarding";
import snackbarOptions from "common/snackbar/options";
import {useSnackbar} from "notistack";
import OnboardingStatus from "../../entities/onboardingStatus";
import axios, {CancelTokenSource} from "axios";

const OnboardingComponent = () => {
    const [onboarding, setOnboarding] = useState<Onboarding>();

    const onboardingService = new OnboardingService();

    const { enqueueSnackbar } = useSnackbar();

    let cancelTokenSource: CancelTokenSource;

    const fetchData = async () => {
        cancelTokenSource = axios.CancelToken.source();

        await onboardingService.get(cancelTokenSource.token)
            .then(result => setOnboarding(result))
            .catch((e: string) => onError(e))
    };

    useEffect(() => {
        fetchData();

        return () => {
            cancelTokenSource.cancel();
        }
    }, []);

    const onError = (e: string) => {
        enqueueSnackbar(e, snackbarOptions);
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
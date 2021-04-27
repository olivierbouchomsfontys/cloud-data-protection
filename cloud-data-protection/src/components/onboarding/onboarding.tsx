import {Button, Typography} from "@material-ui/core";
import React, {useEffect, useState} from "react";
import OnboardingService from "services/onboardingService";
import Onboarding from "entities/onboarding";
import snackbarOptions from "common/snackbar/options";
import {useSnackbar} from "notistack";
import OnboardingStatus from "entities/onboardingStatus";
import axios, {CancelTokenSource} from "axios";
import {GoogleButton, IAuthorizationOptions} from "react-google-oauth2";
import {startLoading, stopLoading} from "common/progress/helper";
import './onboarding.css';

const OnboardingComponent = () => {
    const [onboarding, setOnboarding] = useState<Onboarding>();
    const [options, setOptions] = useState<IAuthorizationOptions>({
        includeGrantedScopes: true,
        accessType: 'offline',
        scopes: [],
        clientId: '',
        redirectUri: '',
        state: ''
    });

    const onboardingService = new OnboardingService();

    const { enqueueSnackbar } = useSnackbar();

    let cancelTokenSource: CancelTokenSource;

    const fetchData = async () => {
        cancelTokenSource = axios.CancelToken.source();

        const response = await onboardingService.get(cancelTokenSource.token)
            .catch((e: string) => onError(e));

        if (response) {
            setOnboarding(response);
            setOptions({...response.loginInfo});
        }
    };

    useEffect(() => {
        startLoading();

        fetchData()
            .finally(() => stopLoading());

        return () => {
            cancelTokenSource?.cancel();
        }
    }, []);

    const onError = (e: string) => {
        enqueueSnackbar(e, snackbarOptions);
    }

    return (
        <div className='onboarding'>
            <Typography variant='h1' className='onboarding__header'>Onboarding</Typography>

            {onboarding &&
                <Typography variant='subtitle2'
                            className='onboarding__status'>Status: {OnboardingStatus[onboarding!.status].toLowerCase()}</Typography>
            }
            {onboarding?.status === OnboardingStatus.None &&
                <div className='onboarding__connect__container'>
                    Get started by connecting your company Google Workspace account.

                    <GoogleButton
                        className='onboarding__btn--connect MuiButtonBase-root MuiButton-root MuiButton-text MuiButton-textPrimary MuiButton-contained MuiButton-containedPrimary'
                        options={options}
                        apiUrl='localhost:5000/Onboarding/GoogleLogin'
                        defaultStyle={false}>
                        Authenticate
                    </GoogleButton>

                    {/* Workaround to correctly style the GoogleButton */}
                    <Button style={{display: "none"}} />
                </div>
            }
            {onboarding?.status === OnboardingStatus.AccountConnected &&
                <p>
                Congratulations! Your Google Workspace account is connected. Please choose a backup scheme that fits your business.
                </p>
            }
            {onboarding?.status === OnboardingStatus.Complete &&
                <p>
                Congratulations! Your data is being secured by Cloud Data Protection.
                </p>
            }
        </div>
    )

}

export default OnboardingComponent;
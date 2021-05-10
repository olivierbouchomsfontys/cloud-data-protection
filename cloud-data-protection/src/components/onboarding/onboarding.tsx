import {
    Button,
    FormControlLabel,
    List,
    ListItem,
    ListItemIcon,
    ListItemText,
    Radio,
    RadioGroup,
    Typography
} from "@material-ui/core";
import React, {FormEvent, Fragment, useEffect, useState} from "react";
import OnboardingService from "services/onboardingService";
import Onboarding from "entities/onboarding";
import snackbarOptions from "common/snackbar/options";
import {useSnackbar} from "notistack";
import OnboardingStatus from "entities/onboardingStatus";
import axios, {CancelTokenSource} from "axios";
import {GoogleButton, IAuthorizationOptions} from "react-google-oauth2";
import {startLoading, stopLoading} from "common/progress/helper";
import BackupSchemeService from "services/backupSchemeService";
import BackupSchemeResult from "services/result/backupScheme/backupSchemeResult";
import BackupConfigurationService from "services/backupConfigurationService";
import BackupConfigurationResult from "services/result/backupConfiguration/backupConfigurationResult";
import {BackupFrequency} from "entities/backupFrequency";
import {formatDate, formatTime} from "common/formatting/timeFormat";
import './onboarding.css';
import {Autorenew, Edit, Schedule} from "@material-ui/icons";
import CreateBackupConfigurationInput from "services/input/backupConfiguration/createBackupConfigurationInput";

const OnboardingComponent = () => {
    const [onboarding, setOnboarding] = useState<Onboarding>();
    const [schemes, setSchemes] = useState<BackupSchemeResult[]>();
    const [schemeId, setSchemeId] = useState<number>();
    const [configuration, setConfiguration] = useState<BackupConfigurationResult>();
    const [options, setOptions] = useState<IAuthorizationOptions>({
        includeGrantedScopes: true,
        accessType: 'offline',
        scopes: [],
        clientId: '',
        redirectUri: '',
        state: ''
    });

    const onboardingService = new OnboardingService();
    const backupSchemeService = new BackupSchemeService();
    const backupConfigurationService = new BackupConfigurationService();

    const { enqueueSnackbar } = useSnackbar();

    let cancelTokenSource: CancelTokenSource;

    useEffect(() => {
        startLoading();

        fetchData()
            .finally(() => stopLoading());

        return () => {
            cancelTokenSource?.cancel();
        }
    }, []);

    const fetchData = async () => {
        cancelTokenSource = axios.CancelToken.source();

        const onboarding = await onboardingService.get(cancelTokenSource.token)
            .catch((e: string) => onError(e));

        if (onboarding) {
            setOnboarding(onboarding);

            if (onboarding.status === OnboardingStatus.None) {
                setOptions({...onboarding.loginInfo});
            }

            if (onboarding.status === OnboardingStatus.AccountConnected) {
                await handleOnboardingAccountConnected();
            }

            if (onboarding.status === OnboardingStatus.SchemeEntered) {
                await handleOnboardingSchemeEntered();
            }
        }
    };

    const handleOnboardingAccountConnected = async () => {
        const schemes = await backupSchemeService.get(cancelTokenSource.token)
            .catch((e: string) => onError(e));

        if (schemes) {
            setSchemeId(schemes[0].id)
            setSchemes(schemes);
        }
    }

    const handleOnboardingSchemeEntered = async () =>  {
        const configuration = await backupConfigurationService.get(cancelTokenSource.token);

        if (configuration) {
            setConfiguration(configuration);
        }
    }

    const handleOnboardingSchemeSubmit = async (e: FormEvent) => {
        e.preventDefault();

        if (!schemeId) {
            enqueueSnackbar('Please select a backup scheme');
            return;
        }

        startLoading();

        const data: CreateBackupConfigurationInput = { backupSchemeId: schemeId };

        cancelTokenSource = axios.CancelToken.source();

        const configuration = await backupConfigurationService.create(data, cancelTokenSource.token)
            .catch((e: string) => onError(e));

        if (configuration) {
            await fetchData();
        }

        stopLoading();
    }

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
                        // TODO Get url from service
                        apiUrl='localhost:5000/Onboarding/GoogleLogin'
                        defaultStyle={false}>
                        Authenticate
                    </GoogleButton>

                    {/* Workaround to correctly style the GoogleButton */}
                    <Button style={{display: "none"}} />
                </div>
            }
            {onboarding?.status === OnboardingStatus.AccountConnected &&
                <div className='onboarding__scheme'>
                    Congratulations! Your Google Workspace account is connected. Please choose a backup scheme that fits your business.
                    {schemes?.length &&
                        <form className='onboarding__scheme__form' onSubmit={(e) => handleOnboardingSchemeSubmit(e)}>
                            <RadioGroup className='onboarding__scheme__select' onChange={(e) => setSchemeId(parseInt(e.target.value))} value={schemeId}>
                                {schemes.map(scheme =>
                                    <FormControlLabel key={scheme.id.toString()} value={scheme.id} control={<Radio />} label={scheme.description} />
                                )}
                            </RadioGroup>
                            <Button className='onboarding__scheme__form__submit' type='submit' color='primary' variant='contained' disabled={schemeId === undefined}>
                                Set backup scheme
                            </Button>
                        </form>
                    }
                </div>
            }
            {onboarding?.status === OnboardingStatus.SchemeEntered &&
                <Fragment>
                    <p>
                        Congratulations! Your data is being secured by Cloud Data Protection. Your backup configuration is displayed below.
                    </p>

                    {configuration &&
                        <div className='onboarding__backup-config'>
                            <Typography variant='h2'>Backup configuration</Typography>
                            <List className='onboarding__backup-config__list' dense={true}>
                                <ListItem>
                                    <ListItemIcon>
                                        <Autorenew />
                                    </ListItemIcon>
                                    <ListItemText>
                                        Frequency: {BackupFrequency[configuration.frequency].toLowerCase()}
                                    </ListItemText>
                                </ListItem>
                                <ListItem>
                                    <ListItemIcon>
                                        <Schedule />
                                    </ListItemIcon>
                                    <ListItemText>
                                        Time: {formatTime(configuration.hour, configuration.minute)}
                                    </ListItemText>
                                </ListItem>
                                <ListItem>
                                    <ListItemIcon>
                                        <Edit />
                                    </ListItemIcon>
                                    <ListItemText>
                                        Created: {formatDate(configuration.createdAt)}
                                    </ListItemText>
                                </ListItem>
                            </List>
                        </div>
                    }
                </Fragment>
            }
        </div>
    )

}

export default OnboardingComponent;
import {Button, Typography} from "@material-ui/core";
import React, {useState} from "react";
import DeleteAccount from "components/settings/account/modal/deleteAccount";
import './accountSettings.css';
import {startLoading, stopLoading} from "common/progress/helper";
import {AccountService} from "services/accountService";
import {useHistory} from "react-router-dom";
import {useSnackbar} from "notistack";
import snackbarOptions from "common/snackbar/options";
import ChangeEmail from "./modal/changeEmail";
import ChangeEmailInput from "../../../services/input/account/changeEmailInput";

const AccountSettings = () => {
    const [isDeleteAccountVisible, setDeleteAccountVisible] = useState(false);
    const [isDeleteAccountLoading, setDeleteAccountLoading] = useState(false);
    
    const [isChangeEmailVisible, setChangeEmailVisible] = useState(false);
    const [isChangeEmailLoading, setChangeEmailLoading] = useState(false);

    const history = useHistory();

    const {enqueueSnackbar} = useSnackbar();

    const onDeleteAccountClick = () => {
        setDeleteAccountVisible(true);
    }

    const onDeleteAccountClose = () => {
        setDeleteAccountVisible(false);
    }

    const onDeleteAccountSubmit = async () => {
        startLoading();

        setDeleteAccountLoading(true);

        await accountService.delete()
            .then(() => onDeleteAccountSuccess())
            .catch((e: any) => onError(e))
            .finally(() => onDeleteAccountComplete());
    }

    const onDeleteAccountSuccess = () => {
        enqueueSnackbar('Your account has been deleted. We hope to see you again in the future.', snackbarOptions);
        history.push('/');
    }

    const onDeleteAccountComplete = () => {
        setDeleteAccountLoading(false);
        stopLoading();
    }

    const onChangeEmailClick = () => {
        setChangeEmailVisible(true);
    }

    const onChangeEmailClose = () => {
        setChangeEmailVisible(false);
    }

    const onChangeEmailSubmit = async (input: ChangeEmailInput) => {
        startLoading();

        setChangeEmailLoading(true);

        await accountService.changeEmail(input)
            .then(() => onChangeEmailSuccess(input.email))
            .catch((e: any) => onError(e))
            .finally(() => onChangeEmailComplete());
    }

    const onChangeEmailSuccess = (email: string) => {
        enqueueSnackbar(`A confirmation link to confirm the change been sent to ${email}.`, snackbarOptions)
    }

    const onChangeEmailComplete = () => {
        setChangeEmailLoading(false);
        stopLoading();
    }

    const onError = (e: any) => {
        enqueueSnackbar(e, snackbarOptions);
    }

    const accountService = new AccountService();

    return (
        <div className='account-settings'>
            <div className='account-settings__userinfo'>
                <Typography variant='h5'>Update account information</Typography>
                <p>If you want to migrate to another email address, you can change it here.</p>
                <Button className='account-settings__userinfo__btn' variant='contained' color='secondary' onClick={() => onChangeEmailClick()} disabled={isChangeEmailLoading}>
                    Change my email address
                </Button>
                {isChangeEmailVisible &&
                    <ChangeEmail onClose={onChangeEmailClose} onSubmit={onChangeEmailSubmit} loading={isChangeEmailLoading} />
                }
            </div>
            <div className='account-settings__delete'>
                <Typography variant='h5'>Delete account</Typography>
                <p>If you don't want to use the services of Cloud Data Protection, you can delete your account. All your personal data will be deleted, leaving no trace at all.</p>
                <Button className='account-settings__delete__btn' variant='contained' color='secondary' onClick={() => onDeleteAccountClick()} disabled={isDeleteAccountLoading}>
                    Delete my account
                </Button>
                {isDeleteAccountVisible &&
                    <DeleteAccount onClose={onDeleteAccountClose} onSubmit={onDeleteAccountSubmit} loading={isDeleteAccountLoading} />
                }
            </div>
        </div>
    )
}

export default AccountSettings;
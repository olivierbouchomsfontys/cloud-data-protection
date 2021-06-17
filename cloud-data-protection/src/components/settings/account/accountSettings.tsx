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
import ChangeEmailInput from "services/input/account/changeEmailInput";
import {useSelector} from "react-redux";
import {selectLoading} from "features/progressSlice";
import ChangePassword from "components/settings/account/modal/changePassword";
import ChangePasswordInput from "services/input/account/changePasswordInput";

const AccountSettings = () => {
    const [isDeleteAccountVisible, setDeleteAccountVisible] = useState(false);
    const [isDeleteAccountLoading, setDeleteAccountLoading] = useState(false);
    
    const [isChangeEmailVisible, setChangeEmailVisible] = useState(false);
    const [isChangeEmailLoading, setChangeEmailLoading] = useState(false);
    
    const [isChangePasswordVisible, setChangePasswordVisible] = useState(false);
    const [isChangePasswordLoading, setChangePasswordLoading] = useState(false);

    const history = useHistory();

    const loading = useSelector(selectLoading);

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
        enqueueSnackbar(`A confirmation link has been sent to ${email}.`, snackbarOptions)
    }

    const onChangeEmailComplete = () => {
        setChangeEmailLoading(false);
        setChangeEmailVisible(false);
        stopLoading();
    }

    const onChangePasswordClick = () => {
        setChangePasswordVisible(true);
    }

    const onChangePasswordClose = () => {
        setChangePasswordVisible(false);
    }

    const onChangePasswordSubmit = async (input: ChangePasswordInput) => {
        startLoading();

        setChangePasswordLoading(true);

        await accountService.changePassword(input)
            .then(() => onChangePasswordSuccess())
            .catch((e: any) => onError(e))
            .finally(() => onChangePasswordComplete());
    }

    const onChangePasswordSuccess = () => {
        setChangePasswordVisible(false);
        enqueueSnackbar('Your password has been updated.', snackbarOptions)
    }

    const onChangePasswordComplete = () => {
        setChangePasswordLoading(false);
        stopLoading();
    }

    const onError = (e: any) => {
        enqueueSnackbar(e, snackbarOptions);
    }

    const accountService = new AccountService();

    return (
        <div className='account-settings'>
            <div className='account-settings__userinfo'>
                <Typography variant='h5'>Update email address</Typography>
                <p>If you want to migrate to another email address, you can change it here.</p>
                <Button className='account-settings__userinfo__btn' variant='contained' color='secondary' onClick={onChangeEmailClick} disabled={loading}>
                    Change my email address
                </Button>
                {isChangeEmailVisible &&
                    <ChangeEmail onClose={onChangeEmailClose} onSubmit={onChangeEmailSubmit} loading={isChangeEmailLoading} />
                }
                <Typography variant='h5'>Update password</Typography>
                <p>If you want to update your password, you can change it here.</p>
                <Button className='account-settings__userinfo__btn' variant='contained' color='secondary' onClick={onChangePasswordClick} disabled={loading}>
                    Change my password
                </Button>
                {isChangePasswordVisible &&
                    <ChangePassword onClose={onChangePasswordClose} onSubmit={onChangePasswordSubmit} loading={isChangePasswordLoading} />
                }
            </div>
            <div className='account-settings__delete'>
                <Typography variant='h5'>Delete account</Typography>
                <p>If you don't want to use the services of Cloud Data Protection, you can delete your account. All your personal data will be deleted, leaving no trace at all.</p>
                <Button className='account-settings__delete__btn' variant='contained' color='secondary' onClick={onDeleteAccountClick} disabled={loading}>
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
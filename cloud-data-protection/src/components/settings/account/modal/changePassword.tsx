import React, {useState} from "react";
import {Button, Dialog, DialogActions, DialogContent, DialogTitle, Input, LinearProgress} from "@material-ui/core";
import ChangePasswordInput from "services/input/account/changePasswordInput";
import './changePassword.css';
import {useSnackbar} from "notistack";

export interface ChangePasswordProps {
    onClose: () => void;
    onSubmit: (input: ChangePasswordInput) => void;
    loading: boolean;
}

const ChangePassword = (props: ChangePasswordProps) => {
    const [currentPassword, setCurrentPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [repeatNewPassword, setRepeatNewPassword] = useState('');

    const { enqueueSnackbar } = useSnackbar();

    const canSubmit = () => {
        if (!currentPassword.length || !newPassword.length) {
            return false;
        }

        return newPassword === repeatNewPassword;
    }

    const onSubmit = () => {
        if (newPassword !== repeatNewPassword) {
            enqueueSnackbar('Entered passwords do not match.')
        }

        const input: ChangePasswordInput = { currentPassword, newPassword };

        props.onSubmit(input);
    }

    return (
        <Dialog onClose={props.onClose} open={true}>
            <div className='dialog__content change-password-wizard'>
                {props.loading &&
                    <LinearProgress color='secondary' />
                }
                <DialogTitle className='dialog__title'>Change password</DialogTitle>
                <DialogContent>
                    You can change your password in order to increase the security of your account.
                    <Input autoFocus className='change-password-wizard__input'
                           type='password'
                           placeholder='Current password'
                           onChange={(e) => setCurrentPassword(e.target.value)} disabled={props.loading} />
                    <Input className='change-password-wizard__input'
                           type='password'
                           autoComplete='new-password'
                           placeholder='New password'
                           onChange={(e) => setNewPassword(e.target.value)} disabled={props.loading} />
                    <Input className='change-password-wizard__input'
                           type='password'
                           placeholder='Repeat password'
                           onChange={(e) => setRepeatNewPassword(e.target.value)} disabled={props.loading} />
                </DialogContent>
                <DialogActions>
                    <Button onClick={props.onClose} color='primary' disabled={props.loading}>Cancel</Button>
                    <Button onClick={onSubmit} color='secondary' disabled={!canSubmit() || props.loading}>
                        Yes
                    </Button>
                </DialogActions>
            </div>
        </Dialog>
    )
}
export default ChangePassword;
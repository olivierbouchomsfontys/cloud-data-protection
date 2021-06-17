import React, {useState} from "react";
import {Button, Dialog, DialogActions, DialogContent, DialogTitle, Input, LinearProgress} from "@material-ui/core";
import ChangeEmailInput from "services/input/account/changeEmailInput";
import {useSelector} from "react-redux";
import {selectUser} from "features/userSlice";
import './changeEmail.css';
import {isValidEmail} from "../../../../common/validator/simpleEmailValidator";

export interface ChangeEmailProps {
    onClose: () => void;
    onSubmit: (input: ChangeEmailInput) => void;
    loading: boolean;
}

const ChangeEmail = (props: ChangeEmailProps) => {
    const [email, setEmail] = useState('');

    const user = useSelector(selectUser);

    const canSubmit = () => {
        return email !== user.email && isValidEmail(email);
    }

    const onSubmit = () => {
        const input: ChangeEmailInput = { email };

        props.onSubmit(input);
    }

    return (
        <Dialog onClose={props.onClose} open={true}>
            <div className='dialog__content change-email-wizard'>
                {props.loading &&
                    <LinearProgress color='secondary' />
                }
                <DialogTitle className='dialog__title'>Change email address</DialogTitle>
                <DialogContent>
                    Your current email address is <span className='change-email-wizard__old-email'>{user.email}</span>. You can enter your new email address. A confirmation link will be sent to the new email address.
                    <Input autoFocus className='change-email-wizard__input'
                           type='email'
                           placeholder='Enter your new e-mail address here'
                           onChange={(e) => setEmail(e.target.value.trim())} disabled={props.loading} />
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
export default ChangeEmail;
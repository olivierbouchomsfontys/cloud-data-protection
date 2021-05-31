import React, {useState} from "react";
import './deleteAccount.css';
import {Button, Dialog, DialogActions, DialogContent, DialogTitle, Input, LinearProgress} from "@material-ui/core";

export interface DeleteAccountProps {
    onClose: () => void;
    onDelete: () => void;
    loading: boolean;
}

const DeleteAccount = (props: DeleteAccountProps) => {
    const [enteredCode, setEnteredCode] = useState('');

    const confirmCode = 'delete my account';

    const canDelete = () => {
        return confirmCode === enteredCode;
    }

    return (
        <Dialog onClose={props.onClose} open={true}>
            <div className='dialog__content delete-account-wizard'>
                {props.loading &&
                    <LinearProgress color='secondary' />
                }
                <DialogTitle className='dialog__title'>Are you sure?</DialogTitle>
                <DialogContent>
                    This action cannot the undone. Please type <span
                    className='delete-account-wizard__confirm-code'>{confirmCode}</span> to confirm.
                    <Input autoFocus className='delete-account-wizard__input'
                           onChange={(e) => setEnteredCode(e.target.value)} disabled={props.loading} />
                </DialogContent>
                <DialogActions>
                    <Button onClick={props.onClose} color='primary' disabled={props.loading}>Cancel</Button>
                    <Button onClick={props.onDelete} color='secondary' disabled={!canDelete() || props.loading}>
                        Yes
                    </Button>
                </DialogActions>
            </div>
        </Dialog>
    )
}

export default DeleteAccount;
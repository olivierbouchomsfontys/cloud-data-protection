import {RouteComponentProps} from "react-router";
import {useHistory} from "react-router-dom";
import React, {Fragment, useEffect} from "react";
import {startLoading, stopLoading} from "common/progress/helper";
import snackbarOptions from "common/snackbar/options";
import {useSnackbar} from "notistack";
import {CancelTokenSource} from "axios";
import {http} from "common/http";
import ConfirmChangeEmailResult from "services/result/account/confirmChangeEmailResult";
import {AccountService} from "services/accountService";
import ConfirmChangeEmailInput from "services/input/account/confirmChangeEmailInput";

interface ConfirmChangeEmailProps extends RouteComponentProps {

}

const ConfirmChangeEmail = (props: ConfirmChangeEmailProps) => {
    let cancelTokenSource: CancelTokenSource;

    const {enqueueSnackbar} = useSnackbar();

    const history = useHistory();

    const accountService = new AccountService();

    useEffect(() => {
        if (props.location.search) {
            const params = new URLSearchParams(props.location.search);

            const token = params.get('token');

            if (token) {
                startLoading();

                confirmChangeEmail(token)
                    .then((result: ConfirmChangeEmailResult) => onConfirmChangeEmailSuccess(result))
                    .catch((e: string) => onConfirmChangeEmailError(e))
                    .finally(() => stopLoading());
            } else {
                history.push('/');
            }
        } else {
            history.push('/');
        }

        return () => {
            cancelTokenSource?.cancel();
        }
    }, [props]);

    const confirmChangeEmail = async (token: string) => {
        cancelTokenSource = http.CancelToken.source();

        const input: ConfirmChangeEmailInput = {token: token};

        return accountService.confirmChangeEmail(input);
    }

    const onConfirmChangeEmailError = (error: string) => {
        enqueueSnackbar(error, snackbarOptions);
        history.push('/');
    }

    const onConfirmChangeEmailSuccess = (result: ConfirmChangeEmailResult) => {
        enqueueSnackbar(`Your email address has been updated to ${result.email}.`, snackbarOptions);
        history.push('/');
    }

    return (
        <Fragment />
    )
}

export default ConfirmChangeEmail;
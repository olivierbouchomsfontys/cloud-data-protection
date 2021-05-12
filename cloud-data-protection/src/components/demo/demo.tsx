import {Button, Typography} from "@material-ui/core";
import React, {FormEvent, useEffect, useState} from "react";
import {formatBytes} from "common/formatting/fileFormat";
import {CancelTokenSource} from "axios";
import {http} from "common/http";
import DemoService from "services/demoService";
import {useSnackbar} from "notistack";
import {startLoading, stopLoading} from "common/progress/helper";
import snackbarOptions from "common/snackbar/options";
import FileUploadResult from "services/result/demo/fileUploadResult";
import './demo.css';

const Demo = () => {
    const [selectedFile, setSelectedFile] = useState<File>();
    const [uploadedFile, setUploadedFile] = useState<FileUploadResult>();

    const { enqueueSnackbar } = useSnackbar();

    const demoService = new DemoService();

    let cancelTokenSource: CancelTokenSource;

    useEffect(() => {
        return () => {
            cancelTokenSource?.cancel();
        }
    })

    const onSubmit = async (e: FormEvent) => {
        e.preventDefault();

        if (!selectedFile) {
            enqueueSnackbar('Please upload a file', snackbarOptions);
            return;
        }

        if (selectedFile.size > DemoService.maxFileSize) {
            enqueueSnackbar('The selected file is too big. The upload limit is 25MB.', snackbarOptions);
            return;
        }

        cancelTokenSource = http.CancelToken.source();

        startLoading();

        await demoService.upload(selectedFile, cancelTokenSource.token)
            .then((result) => setUploadedFile(result))
            .then(() => enqueueSnackbar('File upload succeeded', snackbarOptions))
            .then(() => setSelectedFile(undefined))
            .catch((e) => onError(e))
            .finally(() => stopLoading());
    }

    const onFileSelect = (e: any) => {
        const file: File = e.target.files[0];

        if (file.size > DemoService.maxFileSize) {
            enqueueSnackbar('The selected file is too big. The upload limit is 25MB.', snackbarOptions);

            setSelectedFile(undefined);

            e.target.value = null;

            return;
        }

        setSelectedFile(e.target.files[0]);
    }

    const copyToClipboard = async (e: any) => {
        await navigator.clipboard.writeText(e.target.innerText);

        enqueueSnackbar('Code has been copied to the clipboard', { ...snackbarOptions, autoHideDuration: 2500 });
    }

    const onError = (e: any) => {
        enqueueSnackbar(e, snackbarOptions);
    }

    return (
        <div className='backup-demo'>
            <Typography variant='h1' className='backup-demo__header'>Backup demo</Typography>
            <div className='backup-demo__upload'>
                <Typography variant='h2' className='backup-demo__header'>Upload file</Typography>
                <p>
                    Get a taste of the performance and security Cloud Data Protection offers. Upload a file to get started.
                </p>
                <form  onSubmit={(e) => onSubmit(e)}>
                    <Button className='backup-demo__upload__form__select-file' variant='contained' component="label">
                        {selectedFile ?
                            <span>{selectedFile.name} ({formatBytes(selectedFile.size)})</span> :
                            <span>Select a file</span>
                        }
                        <input type="file" hidden onChange={(e) => onFileSelect(e)} />
                    </Button>

                    {uploadedFile &&
                        <div className='backup-demo__uploaded-file'>
                            Your file has been uploaded. You can access it by saving this code to the clipboard: <code className='backup-demo__uploaded-file__id' onClick={e => copyToClipboard(e)}>{uploadedFile.storageId}</code>
                        </div>
                    }

                    <Button className='backup-demo__form__submit' type='submit' color='primary' variant='contained' disabled={selectedFile === undefined}>
                        Submit
                    </Button>
                </form>
            </div>
        </div>
    )

}

export default Demo;
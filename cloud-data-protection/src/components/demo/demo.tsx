import {Button, Typography} from "@material-ui/core";
import React, {FormEvent, useEffect, useState} from "react";
import './demo.css';
import {formatBytes} from "common/formatting/fileFormat";
import {CancelTokenSource} from "axios";
import {http} from "common/http";
import DemoService from "services/demoService";
import {useSnackbar} from "notistack";
import {startLoading, stopLoading} from "common/progress/helper";
import snackbarOptions from "common/snackbar/options";

const Demo = () => {
    const [selectedFile, setSelectedFile] = useState<File>();

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

    return (
        <div className='backup-demo'>
            <Typography variant='h1' className='backup-demo__header'>Backup demo</Typography>
            <p>
                Get a taste of the performance and security Cloud Data Protection offers. Upload a file to get started.
            </p>
            <form  onSubmit={(e) => onSubmit(e)}>
                <Button className='backup-demo__form__upload' variant='contained' component="label">
                    {selectedFile ?
                        <span>{selectedFile.name} ({formatBytes(selectedFile.size)})</span> :
                        <span>Select a file</span>
                    }
                    <input type="file" hidden onChange={(e) => onFileSelect(e)} />
                </Button>
                <Button className='backup-demo__form__submit' type='submit' color='primary' variant='contained' disabled={selectedFile === undefined}>
                    Submit
                </Button>
            </form>
        </div>
    )

}

export default Demo;
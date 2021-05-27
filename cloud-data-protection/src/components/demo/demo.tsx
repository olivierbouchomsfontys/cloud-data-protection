import {Button, Input, List, ListItem, ListItemIcon, ListItemText, Typography} from "@material-ui/core";
import React, {FormEvent, useEffect, useState} from "react";
import {formatBytes} from "common/formatting/fileFormat";
import {CancelTokenSource} from "axios";
import {http} from "common/http";
import DemoService from "services/demoService";
import {useSnackbar} from "notistack";
import {startLoading, stopLoading} from "common/progress/helper";
import snackbarOptions from "common/snackbar/options";
import FileUploadResult from "services/result/demo/fileUploadResult";
import FileInfoResult from "services/result/demo/fileInfoResult";
import {Crop, Description, Info} from "@material-ui/icons";
import './demo.css';

const Demo = () => {
    const [selectedFile, setSelectedFile] = useState<File>();
    const [uploadedFile, setUploadedFile] = useState<FileUploadResult>();

    const [fileId, setFileId] = useState('');
    const [fileInfo, setFileInfo] = useState<FileInfoResult>();

    const { enqueueSnackbar } = useSnackbar();

    const demoService = new DemoService();

    let cancelTokenSource: CancelTokenSource;

    useEffect(() => {
        return () => {
            cancelTokenSource?.cancel();
        }
    })

    useEffect(() => {
        startLoading();

        onFileIdChange();
    }, [fileId])

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

    const onFileIdChange = async () => {
        cancelTokenSource = http.CancelToken.source();

        startLoading();

        await demoService.getFileInfo(fileId, cancelTokenSource.token)
            .then((result) => setFileInfo(result))
            .catch((e) => setFileInfo(undefined))
            .finally(() => stopLoading());
    }

    const copyToClipboard = async (e: any) => {
        await navigator.clipboard.writeText(e.target.innerText);

        enqueueSnackbar('Code has been copied to the clipboard', { ...snackbarOptions, autoHideDuration: 2500 });
    }

    const download = async (decrypt: boolean) => {
        if (!fileId) {
            enqueueSnackbar('Please enter a file code.');
            return;
        }

        cancelTokenSource = http.CancelToken.source();

        startLoading();

        await demoService.downloadFile(fileId, decrypt, cancelTokenSource.token)
            .catch((e) => onError(e))
            .finally(() => stopLoading());
    }

    const onError = (e: any) => {
        if (e instanceof String) {
            enqueueSnackbar(e, snackbarOptions);
        }
    }

    return (
        <div className='backup-demo'>
            <Typography variant='h1'>Backup demo</Typography>
            <div className='backup-demo__upload'>
                <Typography variant='h2'>Upload file</Typography>
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
                            Your file has been uploaded. You can access it later by saving this code. Click it to copy to the clipboard: <code className='backup-demo__uploaded-file__id' onClick={(e) => copyToClipboard(e)}>{uploadedFile.storageId}</code>
                        </div>
                    }

                    <div className='backup-demo__upload__btn-container'>
                        <Button className='backup-demo__form__submit' type='submit' color='primary' variant='contained' disabled={selectedFile === undefined}>
                            Upload file
                        </Button>
                    </div>
                </form>
            </div>
            <div className='backup-demo__retrieve'>
                <Typography variant='h2'>Retrieve file</Typography>
                <p>
                    Retrieve a file by entering a code in the text field below.
                </p>
                <Input className='backup-demo__retrieve__input' type="text" placeholder="Enter code" value={fileId} onChange={(e) => setFileId(e.target.value)}/>
                {fileInfo &&
                    <div className='backup-demo__retrieve__file-info'>
                        <Typography variant='h5'>File info</Typography>
                        <List className='onboarding__backup-config__list' dense={true}>
                            <ListItem>
                                <ListItemIcon>
                                    <Description />
                                </ListItemIcon>
                                <ListItemText>
                                    Name: {fileInfo.name}
                                </ListItemText>
                            </ListItem>
                            <ListItem>
                                <ListItemIcon>
                                    <Crop />
                                </ListItemIcon>
                                <ListItemText>
                                    Size: {formatBytes(fileInfo.bytes)}
                                </ListItemText>
                            </ListItem>
                            <ListItem>
                                <ListItemIcon>
                                    <Info />
                                </ListItemIcon>
                                <ListItemText>
                                    Type: {fileInfo.contentType}
                                </ListItemText>
                            </ListItem>
                        </List>
                    </div>
                }
                <div className='backup-demo__retrieve__btn-container'>
                    <Button className='backup-demo__retrieve' color='primary' variant='contained' disabled={fileInfo === undefined} onClick={() => download(true)}>
                        Download file
                    </Button>
                </div>
            </div>
        </div>
    )

}

export default Demo;
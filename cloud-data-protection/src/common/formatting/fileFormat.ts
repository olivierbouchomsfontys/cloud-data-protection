export const formatBytes = (bytes: number) => {
    if (bytes === 0) {
        return '0 bytes';
    }

    const factor = 1024;
    const sizes = ['bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

    const sizeIndex = Math.floor(Math.log(bytes) / Math.log(factor));

    return parseFloat((bytes / Math.pow(factor, sizeIndex)).toFixed(2)) + ' ' + sizes[sizeIndex];
}
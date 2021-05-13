export const readFileName = (input: string): string => {
    return input.split('filename=')[1].split(';')[0];
}
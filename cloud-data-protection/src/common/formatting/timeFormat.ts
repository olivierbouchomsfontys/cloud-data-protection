export const formatDate = (input: Date | string): string => {
    input = new Date(input);

    const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: 'long', day: 'numeric', hour: "2-digit", minute: "2-digit" };

    const language = 'en';

    // @ts-ignore
    return input.toLocaleDateString(language, options).toLowerCase();
}

export const formatTime = (hour: number, minute: number): string => {
    let hourStr = hour.toString().padStart(2, '0');
    let minuteStr = minute.toString().padStart(2, '0');

    return `${hourStr}:${minuteStr}`;
}
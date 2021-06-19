export const shorten = (email: string) => {
    const parts: string[] = email.split('@');

    parts[0] = parts[0].substring(0, Math.min(3, parts[0].length)).concat('***');

    return parts.join('@');
}
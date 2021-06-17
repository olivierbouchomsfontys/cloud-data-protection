export const isValidEmail = (email: string) => {
    const minimumLength = 5;

    if (!email.length || email.length < minimumLength) {
        return false;
    }

    const dotIndex = email.lastIndexOf('.');

    const atIndex = email.lastIndexOf('@');

    if (dotIndex === -1 || atIndex === -1) {
        return false;
    }

    if (dotIndex < atIndex) {
        return false;
    }

    const expectedTldIndex = dotIndex + 1;

    const hasTld = !!email.charAt(expectedTldIndex);

    return hasTld;
}
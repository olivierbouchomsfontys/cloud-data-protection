export interface RegisterResult {
    token: string;
    user: RegisterUserResult;
}

export interface RegisterUserResult {
    id: number;
    email: string;
    role: string;
}
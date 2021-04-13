export interface LoginResult {
    token: string;
    user: LoginUserResult;
}

export interface LoginUserResult {
    id: number;
    email: string;
    role: string;
}
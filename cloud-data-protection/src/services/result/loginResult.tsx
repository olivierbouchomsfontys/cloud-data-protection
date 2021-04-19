export interface LoginResult {
    token: string;
    user: LoginUserResult;
}

export interface LoginUserResult {
    id: number;
    email: string;
    role: UserRole;
}

export enum UserRole {
    Client = 0,
    Employee = 1
}
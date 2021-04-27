import UserRole from "entities/userRole";

export interface LoginResult {
    token: string;
    user: LoginUserResult;
}

export interface LoginUserResult {
    id: number;
    email: string;
    role: UserRole;
}

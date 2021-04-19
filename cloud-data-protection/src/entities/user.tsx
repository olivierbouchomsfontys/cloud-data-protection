import {UserRole} from "../services/result/loginResult";

interface User {
    id: number;
    email: string;
    role: UserRole;
}

export default User;
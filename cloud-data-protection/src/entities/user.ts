import UserRole from "./userRole";

interface User {
    id: number;
    email: string;
    role: UserRole;
}

export default User;
export interface User {
    id: number;
    name: string;
    email: string;
    adminPermission: boolean;
    modPermission: boolean;
    isActivated: boolean;
}
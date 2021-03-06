import { AccessPoint } from "./access-point.model";

export interface User {
    id: number;
    name: string;
    email: string;
    adminPermission: boolean;
    modPermission: boolean;
    isActivated: boolean;
    addedAccessPoints: Array<AccessPoint>;
    modifiedAccessPoints: Array<AccessPoint>;
}
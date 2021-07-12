import { User } from "./user.model";

export interface AccessPoint {
    id: number;
    addDate: Date;
    editDate: Date;
    bssid: string;
    ssid: string;
    frequency: string;
    maxSignalLevel: number;
    maxSignalLongitude: number;
    maxSignalLatitude: number;
    minSignalLevel: number;
    minSignalLongitude: number;
    minSignalLatitude: number;
    signalRadius: number;
    signalArea: number;
    serializedSecurityData: string;
    manufacturer: string;
    deviceType: string;
    masterGroup: boolean;
    display: boolean;
    note: string;
    isSecure: boolean;
    isHidden: boolean;
    userAdded: User;
    userModified: User;
}
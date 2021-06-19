import { User } from "./user.model";

export interface AccessPoint {
    dd: number;
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
    userAdded: User;
    userModified: User;
}
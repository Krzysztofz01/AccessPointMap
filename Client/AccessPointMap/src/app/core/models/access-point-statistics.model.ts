import { AccessPoint } from "./access-point.model";

export interface AccessPointStatistics {
    allRecords: number;
    insecureRecords: number;
    topBrands: Array<[ string, number ]>;
    topAreaAccessPoints: Array<AccessPoint>;
    topSecurityTypes: Array<[ string, number ]>;
    topFrequencies: Array<[ number, number ]>;
}
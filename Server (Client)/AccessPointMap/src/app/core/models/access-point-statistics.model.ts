import { AccessPoint } from "./access-point.model";
import { Pair } from "./pair.model";

export interface AccessPointStatistics {
    allRecords: number;
    insecureRecords: number;
    topBrands: Array<Pair>;
    topAreaAccessPoints: Array<AccessPoint>;
    topSecurityTypes: Array<Pair>;
    topFrequencies: Array<Pair>;
}
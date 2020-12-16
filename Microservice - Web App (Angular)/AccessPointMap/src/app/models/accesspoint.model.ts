export interface Accesspoint {
    id: number
    bssid: string
    ssid: string
    frequency: number
    highSignalLevel: number
    highLongitude: number
    highLatitude: number
    lowSignalLevel: number
    lowLongitude: number
    lowLatitude: number
    signalRadius: number
    signalArea: number
    securityData: string
    securityDataRaw: string
    brand: string
    deviceType: string
    display: boolean
    postedBy: string
    createDate: Date
    updateDate: Date
}
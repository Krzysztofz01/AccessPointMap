export interface AccesspointQueue {
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
    securityDataRaw: string
    deviceType: string
    postedBy: string
    createDate: Date
}
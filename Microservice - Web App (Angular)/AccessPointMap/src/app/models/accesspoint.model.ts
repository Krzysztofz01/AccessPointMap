export interface Accesspoint {
    Id: number
    Bssid: string
    Ssid: string
    Frequency: number
    HighSignalLevel: number
    HighLongitude: number
    HighLatitude: number
    LowSignalLevel: number
    LowLongitude: number
    LowLatitude: number
    SignalRadius: number
    SignalArea: number
    SecurityData: string
    SecurityDataRaw: string
    Brand: string
    DeviceType: string
    Display: boolean
    PostedBy: string
    CreateDate: Date
    UpdateDate: Date
}
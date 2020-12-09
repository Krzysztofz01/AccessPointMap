export interface User {
    Id: number
    Password: string
    Email: string
    TokenExpiration: number
    WritePermission: boolean
    ReadPermission: boolean
    AdminPermission: boolean
    CreateDate: Date
    LastLoginDate: Date
    LastLoginIp: string
    Active: boolean
}
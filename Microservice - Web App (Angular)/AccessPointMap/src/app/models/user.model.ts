export interface User {
    id: number
    password: string
    email: string
    tokenExpiration: number
    writePermission: boolean
    readPermission: boolean
    adminPermission: boolean
    createDate: Date
    lastLoginDate: Date
    lastLoginIp: string
    active: boolean
}
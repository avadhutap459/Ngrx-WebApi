export interface User {
    UserId: number;
    FirstName: string;
    LastName: string;
    EmailId: string;
    MobileNo: string;
    IsActive: string;
    CreatedAt: Date;
    CreatedBy: number;
    LastModifiedAt: Date
    LastModifiedBy: number;
    Password: string;
    RefreshToken: string;
    RefreshTokenExpiryTime: Date
}
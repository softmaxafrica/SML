export class CompanyPayload {
    companyId?:string;
    companyTinNumber?: string;
    companyName?: string;
    adminFullName?: string;
    adminEmail?: string;
    adminContact?: string;
    companyAddress?: string;
    companyDescription?: string;
    companyLatitude?: number;
    companyLongitude?: number;

    constructor(
        companyId?: string,
        companyTinNumber?: string,
        companyName?: string,
        adminFullName?: string,
        adminEmail?: string,
        adminContact?: string,
        companyAddress?: string,
        companyDescription?: string,
        companyLatitude?: number,
        companyLongitude?: number
    ) {
        this.companyId=companyId;
        this.companyTinNumber = companyTinNumber;
        this.companyName = companyName;
        this.adminFullName = adminFullName;
        this.adminEmail = adminEmail;
        this.adminContact = adminContact;
        this.companyAddress = companyAddress;
        this.companyDescription = companyDescription;
        this.companyLatitude = companyLatitude;
        this.companyLongitude = companyLongitude;
    }
}

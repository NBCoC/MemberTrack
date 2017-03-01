export interface TokenDto {
    access_token: string;
    expires_in: number;
    token_type: string;
}

export interface LookupItemDto {
    id: number;
    name: string;
}

export interface LookupResultDto {
    roles: LookupItemDto[];
    states: LookupItemDto[];
    personStatus: LookupItemDto[];
    genders: LookupItemDto[];
    checkListItemTypes: LookupItemDto[];
    ageGroups: LookupItemDto[];
}

export interface SignInDto {
    email: string;
    password: string;
}

export interface UpdatePasswordDto {
    oldPassword: string;
    newPassword: string;
}

export interface UserDto {
    id: number;
    displayName: string;
    email: string;
    role: number;
    roleName: string;
    isAdmin: boolean;
    isEditor: boolean;
    isSystemAdmin: boolean;
}

export interface AddressDto {
    state: number;
    stateName: string;
    street: string;
    city: string;
    zipCode: number;
}

export interface ChildrenInfoDto {
    hasInfantKids: boolean;
    hasToddlerKids: boolean;
    hasElementaryKids: boolean;
    hasJuniorHighKids: boolean;
    hasHighSchoolKids: boolean;
}

export interface PersonCheckListItemDto {
    id: number;
    description: string;
    type: number;
    typeName: string;
    isSelected: boolean;
    note: string;
    date: Date;
    sortOrder: number;
}

export interface DatesDto {
    baptismDate?: Date;
    firstVisitDate?: Date;
    membershipDate?: Date;
}

export interface PersonDto {
    id: number;
    firstName: string;
    middleName?: string;
    lastName: string;
    email?: string;
    contactNumber?: string;
    status: number;
    statusName: string;
    gender: number;
    genderName: string;
    ageGroup?: number;
    ageGroupName: string;
    dates: DatesDto;
    address: AddressDto;
    childrenInfo: ChildrenInfoDto;
    checkListItems: PersonCheckListItemDto[];
}

export interface SearchResultDto {
    count: number;
    totalCount: number;
    data: any[];
}

export interface PersonReportItemDto {
    memberCount: number;
    visitorCount: number;
    month: number;
    monthName: string;
}

export interface PersonReportDto {
    items: PersonReportItemDto[];
}

let getIndex = (array: any[], value: string | number): number => {
    let index = -1, count = array.length;

    for (let i = 0; i < count; i++) {
        let item = array[i];

        if (item.id !== value) {
            continue;
        }

        index = i;

        break;
    }

    return index;
};

export class DtoHelper {

    public insertOrUpdate(array: any[], dto: any): any[] {
        if (!array || !dto) {
            return;
        }

        let data: any[] = [];

        Object.assign(data, array);

        let index = getIndex(array, dto.id);

        if (index !== -1) {
            data[index] = dto;
        } else {
            data.push(dto);
        }

        return data;
    }

    public remove(array: any[], id: string | number): void {
        if (!array || !id) {
            return;
        }

        let index = getIndex(array, id);

        if (index < 0) {
            return;
        }

        array.splice(index, 1);
    }

    public getIndexOf(array: any[], value: string | number): number {
        return getIndex(array, value);
    }
}
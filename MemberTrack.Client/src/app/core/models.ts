export enum UserRoles {
    Viewer = 1,
    Editor = 2,
    Admin = 3,
    SystemAdmin = 4
}

export enum PersonStatus {
    Visitor = 1,
    Member
}

export enum Gender {
    Male = 0,
    Female
}

export enum AgeGroup {
    Unknown = 0,
    Group1,
    Group2,
    Group3,
    Group4,
    Group5,
    Infant,
    Toddler,
    Elemtary,
    JuniorHigh,
    HighSchool
}

export enum State {
    TX = 43
}

export interface TokenResponse {
    access_token: string;
    expires_in: number;
    token_type: string;
}

export interface LookupItem {
    key: number;
    value: string;
}

export interface LookupResult {
    roles: Array<LookupItem>;
}

export interface DeleteItem {
    id: number;
    name: string;
}

export interface SignIn {
    email: string;
    password: string;
}

export interface UpdatePassword {
    oldPassword: string;
    newPassword: string;
}

export interface RouteData {
    title: string;
    isAdmin: boolean;
}

export interface ContextUser extends User { }

export interface User {
    id: number;
    displayName: string;
    email: string;
    role: UserRoles;
    roleName: string;
}

export interface Address {
    state: State;
    stateName: string;
    street: string;
    city: string;
    zipCode: number;
}

export interface ChildrenInfo {
    ageGroups: AgeGroup[];
}

export interface Visit {
    note: string;
    date: Date;
    checkListItems: VisitCheckListItem[];
}

export interface VisitCheckListItem {
    id: number;
    description: string;
    group: number;
}

export interface Person {
    id: number;
    firstName: string;
    middleName?: string;
    lastName: string;
    email?: string;
    contactNumber?: string;
    status: PersonStatus;
    statusName: string;
    gender: Gender;
    genderName: string;
    ageGroup: AgeGroup;
    ageGroupName: string;
    address: Address;
    childrenInfo: ChildrenInfo;
    visits: Visit[];
}

let getIndex = (array: any[], value: string | number): number => {
    let index = -1, count = array.length;

    for (let i = 0; i < count; i++) {
        let item = array[i];

        if (item['id'] !== value) {
            continue;
        }

        index = i;

        break;
    }

    return index;
};

export class ModelHelper {

    public insertOrUpdate(array: any[], model: any): void {
        if (!array || !model) {
            return;
        }

        let data: any[] = [];

        Object.assign(data, array);

        let index = getIndex(array, model['id']);

        if (index !== -1) {
            data[index] = model;
        } else {
            data.push(model);
        }

        Object.assign(array, data);
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
}
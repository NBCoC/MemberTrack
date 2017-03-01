import { UserDto, PersonDto } from './dtos';

export class PromptEvent {
    public data: number;

    constructor(id?: number) {
        this.data = id;
    }
}

export class UserEvent {
    public data: UserDto;

    constructor(message: UserDto) {
        this.data = message;
    }
}

export class SnackbarEvent {
    public data: string;

    constructor(message: string) {
        this.data = message;
    }
}

export class IsLoadingEvent {
    public data: boolean;

    constructor(message: boolean) {
        this.data = message;
    }
}

export class PersonEvent {
    public data: PersonDto;

    constructor(message: PersonDto) {
        this.data = message;
    }
}

export interface CreateUserDto {
    name: string;
    email: string;
    password: string;
}

export interface LoginUserDto {
    email: string;
    password: string;
}

export interface LoginUserResponseDto {
    userId: number;
    name: string;
    email: string;
    accessToken: string;
}

export class LoggedUserDto {
    userId: number = 0;
    name: string = "";
    email: string =  "";
}

export class UserDto {
    userId: number = 0;
    name: string = "";
    email: string =  "";
}
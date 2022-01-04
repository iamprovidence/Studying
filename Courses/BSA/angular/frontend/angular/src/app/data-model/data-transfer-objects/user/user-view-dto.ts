export interface UserViewDTO
{
    id: number;

    firstName : string;
    lastName : string;
    email : string;

    birthday : Date;
    registeredAt : Date;

    teamId? : number;
    teamName? : string;
}
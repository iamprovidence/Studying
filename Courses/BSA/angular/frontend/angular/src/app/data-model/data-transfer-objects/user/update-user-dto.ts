export interface UpdateUserDTO
{
    id : number;

    firstName : string;
    lastName : string;
    email : string;

    birthday : Date;

    teamId? : number;
}
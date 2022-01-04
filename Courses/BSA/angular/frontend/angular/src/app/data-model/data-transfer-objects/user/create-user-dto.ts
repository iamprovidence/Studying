export class CreateUserDTO
{
    firstName: string;
    lastName: string;

    email: string;
    birthday: Date;

    teamId? : number;
}
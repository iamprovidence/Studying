export class CreateProjectDTO
{
    name : string;
    description : string;

    deadline : Date;

    authorId : number;
    teamId? : number;
}
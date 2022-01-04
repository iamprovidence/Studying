export interface UpdateProjectDTO
{
    id : number;

    name : string;
    description : string;

    deadline : Date;

    authorId : number;
    teamdId? : number;
}
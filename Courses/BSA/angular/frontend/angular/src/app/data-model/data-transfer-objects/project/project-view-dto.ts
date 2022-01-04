export interface ProjectViewDTO
{
    id : number;

    name : string;
    description : string;

    createdAt : Date;
    deadline : Date;

    authorId : number;
    authorName : string;

    teamId? : number;
    teamName? : string;
}
export class CreateTaskDTO
{
    name : string;
    description : string;

    finishedAt : Date;

    projectId : number;
    performerId? : number;
    taskStateId : number = 1;
}
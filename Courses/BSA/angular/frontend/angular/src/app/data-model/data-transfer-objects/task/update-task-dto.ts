export interface UpdateTaskDTO
{
    id : number;

    name : string;
    description : string;

    finishedAt : Date;

    projectId : number;
    performerId? : number;
    taskStateId : number;
}
export interface TaskViewDTO
{
    id : number;

    name : string;
    description : string;

    createdAt : Date;
    finishedAt : Date;

    projectId : number;
    projectName : string;

    performerId? : number;
    performerName? : string;

    taskStateId : number;
    stateName : string;
}
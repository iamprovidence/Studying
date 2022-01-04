import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { ICrudServerRoute } from "src/app/interfaces";
import { RequestResult } from 'src/app/data-model';

export abstract class CrudServiceBase 
{
    // fields
    protected http : HttpClient;
    private crudRoute : ICrudServerRoute;

    // constructors
    constructor(http: HttpClient, crudRoute : ICrudServerRoute) 
    {
        this.http = http;
        this.crudRoute = crudRoute;
    }

    // methods
    protected stringFormat(strFormat:string, ...args:any[]) : string
    {
        return strFormat.replace(/{(\d+)}/g, function(match, number) 
        { 
            return typeof args[number] != 'undefined' ? args[number] : match;
        });
    }

    public getSingle<ViewDTO>(entityId : number) : Observable<ViewDTO>
    {
        return this.http.get<ViewDTO>(this.stringFormat(this.crudRoute.SINGLE_FORMAT, entityId));
    }
    public getAll<ListDTO>() : Observable<ListDTO[]>
    {
        return this.http.get<ListDTO[]>(this.crudRoute.ALL);
    }
    public update<UpdateDTO>(updateTeamDTO : UpdateDTO) : Observable<RequestResult>
    {
        return this.http.patch<RequestResult>(this.crudRoute.UPDATE, updateTeamDTO);
    }
    public create<CreateDTO>(createTeamDTO : CreateDTO) : Observable<RequestResult>
    {
        return this.http.post<RequestResult>(this.crudRoute.CREATE, createTeamDTO);
    }
    public delete(entityId: number) : Observable<RequestResult>
    {
        return this.http.delete<RequestResult>(this.stringFormat(this.crudRoute.DELETE_FORMAT, entityId));
    }
}
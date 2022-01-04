import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { TaskStateListDTO } from '../data-model';
import { API } from '../app.settings';

export class TaskStateService
{

    // fields
    protected http : HttpClient;

    // constructors
    constructor(http: HttpClient) 
    {
        this.http = http;
    }

    // methods
    public getAll() : Observable<TaskStateListDTO[]>
    {
        return this.http.get<TaskStateListDTO[]>(API.TASK_STATE.ALL);
    }
}

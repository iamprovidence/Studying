import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { API } from '../app.settings';
import { CrudServiceBase } from './abstract/crud-service-base';

@Injectable()
export class TeamService extends CrudServiceBase
{
    constructor(http: HttpClient) 
    {
        super(http, API.TEAM);
    }
}
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { API } from '../app.settings';
import { CrudServiceBase } from './abstract/crud-service-base';

@Injectable()
export class UserService extends CrudServiceBase
{
    // constructors
    constructor(http: HttpClient) 
    {
        super(http, API.USER);
    }
}
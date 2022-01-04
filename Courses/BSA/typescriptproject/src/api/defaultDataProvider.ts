import config from '../settings';
import { FightersFile } from '../models/fighter/fightersFile';

export class DataProvider
{
    // methods    
    public async callApi(endpoind: string, method: string):Promise<FightersFile> 
    {
        const url:string = config.API_URL + endpoind;
        const options:RequestInit = { method };

        // fetch data
        return fetch(url, options)
        .then(response => response.ok ? response.json() : Promise.reject(Error('Failed to load')))
        .catch(error => { throw error; });
    }
}
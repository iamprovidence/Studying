import { DataProvider } from '../api/defaultDataProvider';
import { FighterShortInfo, FighterFullInfo, FightersFile } from '../models/index';
import config from '../settings';

export class FighterService 
{
    // fields
    private dataProvider: DataProvider;

    // constructors
    constructor()
    {
        this.dataProvider = new DataProvider();
    }

    // methods
    public async getFighters() : Promise<FighterShortInfo[]>
    {
        const { endpoint, method } = config.FIGHTHERS_API;
        const apiResult:FightersFile = await this.dataProvider.callApi(endpoint, method);

        return JSON.parse(atob(apiResult.content));
    }

    public async getFighterDetails(fighterId:string): Promise<FighterFullInfo>
    {
        const { endpoint_format, method } = config.FIGHTHER_DETAIL_API;
        const endpoint = this.stringFormat(endpoint_format, fighterId);
        
        const apiResult:FightersFile = await this.dataProvider.callApi(endpoint, method);

        return JSON.parse(atob(apiResult.content));
    }
    private stringFormat(strFormat:string, ...args:string[]) : string
    {
        return strFormat.replace(/{(\d+)}/g, function(match, number) 
        { 
            return typeof args[number] != 'undefined' ? args[number] : match;
        });
    }
}

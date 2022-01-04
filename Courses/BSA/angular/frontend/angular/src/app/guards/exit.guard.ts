import { CanDeactivate } from "@angular/router";

import { ComponentCanDeactivate } from 'src/app/interfaces';
 
export class ExitGuard implements CanDeactivate<ComponentCanDeactivate>
{
    canDeactivate(component: ComponentCanDeactivate) : boolean
    {         
        return component.canDeactivate ? component.canDeactivate() : true;
    }
}
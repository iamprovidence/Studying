import { ViewBase } from "./abstract/viewBase";
import { ClickEventHandler, FighterShortInfo } from '../models/index';

export class MenuView extends ViewBase
{
    public getSelectedFighterId(selectId:string) : string
    {
        const select:HTMLSelectElement = document.getElementById(selectId) as HTMLSelectElement;
        return select.options[select.selectedIndex].value;
    }

    public createMenuContainer(fighters:FighterShortInfo[], clickHandler: ClickEventHandler<null>)
    {
        const container = this.createElement({ tagName:"div", className:"menu" });
        
        // selects
        const leftFighterSelect: HTMLSelectElement = this.createFighterSelect(fighters, "left-fighter");
        const vsImageElement: HTMLElement = this.createVsImage();
        const rightFighterSelect: HTMLSelectElement = this.createFighterSelect(fighters, "right-fighter");

        // start button
        const startBtn: HTMLButtonElement = this.createButton("Start", "blue", clickHandler);

        container.append(leftFighterSelect, vsImageElement, rightFighterSelect, startBtn);
        return container;
    }

    public createFighterSelect(fightersInfo: FighterShortInfo[], selectId:string) : HTMLSelectElement
    {
        const fighterSelectElement:HTMLSelectElement = document.createElement("select");

        fighterSelectElement.id = selectId;

        // add options
        fightersInfo.forEach(fighter => 
        {
            fighterSelectElement.appendChild(this.createElement(
            {
                tagName: "option", 
                content: fighter.name,
                attributes: { value: fighter._id }
            }));
        })
        
        // select first option
        fighterSelectElement.firstElementChild.setAttribute("selected", "true");

        return fighterSelectElement;
    }
    public createVsImage()
    {
        return this.createElement(
        {
            tagName:"img", 
            className: 'vs-img', 
            attributes: { src: '/resources/vs.png' 
        }});
    }
}
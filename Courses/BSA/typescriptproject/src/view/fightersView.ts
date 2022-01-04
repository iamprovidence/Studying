import { ViewBase } from './abstract/viewBase';
import { FighterShortInfo, FighterFullInfo, UpdateFighterInfo, ClickEventHandler } from '../models/index';

export class FightersView extends ViewBase 
{
    // methods


    // create fighters
    public createFighters(fighters:FighterShortInfo[], clickHandler:ClickEventHandler<string>) : HTMLElement
    {
        const fighterElements = fighters.map(fighter => this.createFighter(fighter, clickHandler));

        const divFightersContainer:HTMLElement = this.createElement({ tagName: 'div', className: 'fighters' });
        divFightersContainer.append(...fighterElements);

        return divFightersContainer;
    }
    public createFighter(fighter: FighterShortInfo, handleClick: ClickEventHandler<string>) : HTMLElement
    {
        const { name, source } = fighter;
        const nameElement = this.createName(name);
        const imageElement = this.createFighterImage(source);

        // fighter
        const divFighterElement = this.createElement({ tagName: 'div', className: 'fighter' });
        divFighterElement.append(imageElement, nameElement);
        divFighterElement.addEventListener('click', event => handleClick(event, fighter._id), false);

        return divFighterElement;
    }

    private createName(name: string) : HTMLElement
    {
        return this.createElement({ tagName: 'span', className: 'name', content: name });
    }
    
    // get fighters info
    public getFightersInfo() : UpdateFighterInfo
    {
        const fighterInfoHtmlElement: Element = document.querySelector("#modal > div.modal-info");
        const inputs: HTMLCollectionOf<HTMLInputElement> = fighterInfoHtmlElement.getElementsByTagName("input");

        // get value from all inputs
        return {
            fighterId: fighterInfoHtmlElement.getAttribute("data-fighter-id"),
            attack:  this.getInputValue(inputs[0]),
            defense: this.getInputValue(inputs[1]),
            health:  this.getInputValue(inputs[2]),
        };
    }
    private getInputValue(input:HTMLInputElement) : number
    {
        let value: number = +input.value;

        if (value < +input.min)      value = +input.min;
        else if (value > +input.max) value = +input.max;

        return value;
    }

    // create fighter info
    public createFightersDetails(fighterDetail:FighterFullInfo, clickHandler: ClickEventHandler<any>) : HTMLElement
    {        
        const divModalFighterInfo:HTMLElement = this.createElement(
        { 
            tagName: 'div', 
            className: 'modal-info', 
            attributes: { "data-fighter-id": String(fighterDetail._id) } 
        });
        
        // add table info
        divModalFighterInfo.appendChild(this.createFightersDetailTable(fighterDetail));
        
        // add ok button
        const divModalOkBtn:HTMLElement = this.createButton("ok", "blue", clickHandler)
        divModalFighterInfo.appendChild(divModalOkBtn);

        return divModalFighterInfo;
    }
    private createFightersDetailTable(fighterDetail:FighterFullInfo)
    {
        const tableFighterInfo:HTMLElement = this.createElement({ tagName: 'table' });
        
        // table caption        
        tableFighterInfo.appendChild(this.createElement({tagName: "caption", content: fighterDetail.name}));        

        // rows
        tableFighterInfo.appendChild(this.createTableRowWithInput("attack", fighterDetail.attack));
        tableFighterInfo.appendChild(this.createTableRowWithInput("defense", fighterDetail.defense));
        tableFighterInfo.appendChild(this.createTableRowWithInput("health", fighterDetail.health));
        
        return tableFighterInfo;
    }
    private createTableRowWithInput(rowName: string, inputValue: number) : HTMLElement
    {
        const tr = this.createElement({tagName: "tr"});

        // row name
        tr.appendChild(this.createElement({tagName: "td", content: rowName}));
        
        // input
        const td = this.createElement({tagName: "td"});
        td.appendChild(this.createNumberInput(inputValue));
        tr.appendChild(td);

        return tr;
    }
    
    public createNumberInput(inputValue:number) : HTMLElement
    {        
        return this.createElement(
        {
            tagName:"input",
            attributes:
            {
                type: "number",
                value: String(inputValue),
                min: "1",
                max: "100"
            }
        })
    }
}
import { ViewBase } from './abstract/viewBase';
import { Fighter, Fight } from '../domainModels/index';
import { FightResultDTO, ClickEventHandler } from '../models/index';

export class FightView extends ViewBase
{
    public createFight(fight: Fight, fightResult: FightResultDTO, clickHandler: ClickEventHandler<null>) : HTMLElement 
    {   
        // round or winner name
        const roundValue = this.createElement(
        {
            tagName: "span", 
            content: fightResult.value, 
            className: fightResult.done ? "winner" : "round" 
        });     

        // fighters
        const leftFighter: HTMLElement = this.createFighter(fight.getLeftFighter);
        const rightFighter: HTMLElement = this.createFighter(fight.getRightFighter, true);
        
        // create button depending on fight result
        let nextRoundBtn: HTMLButtonElement = null;
        if (fightResult.done)
        {
            nextRoundBtn = this.createButton("start new fight", "orange", clickHandler);
        }
        else
        {
            nextRoundBtn = this.createButton("next round", "blue", clickHandler);
        }

        // container
        const divFightersContainer:HTMLElement = this.createElement({ tagName: 'div', className: 'fight' });
        divFightersContainer.append(roundValue, leftFighter, rightFighter, nextRoundBtn);

        return divFightersContainer;
    }
    public createFighter(fighter: Fighter, isRotated:boolean = false) : HTMLElement
    {
        // attributes
        const nameElement =     this.createElement({tagName: "span", content: fighter.info.name});
        const atackElement =    this.createElement({tagName: "span", content: "Attack: " + fighter.info.attack});
        const defenseElement =  this.createElement({tagName: "span", content: "Defense: " + fighter.info.defense});

        // image
        const imageElement = this.createFighterImage(fighter.info.source);
        if (isRotated)
        {
            imageElement.style.transform = "scaleX(-1)";
        }

        // health
        const healthElement = this.createHealthBar(fighter.lifePercentage);

        // container
        const divFighterElement = this.createElement({ tagName: 'div', className: 'fighter' });
        divFighterElement.append(nameElement, healthElement, imageElement, atackElement, defenseElement);

        return divFighterElement;
    }
    
    public createHealthBar(health: number): HTMLElement
    {
        const divProgressElement: HTMLElement = this.createElement({ tagName: "div", className: "progress" });

        // bar
        const divBarElement: HTMLElement = this.createElement({ tagName: "div", className: "bar"});
        divBarElement.style.width = health + '%';

        divProgressElement.appendChild(divBarElement);
        return divProgressElement;
    }
}
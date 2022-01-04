import { Fighter } from './fighter';
import { FighterFullInfo } from '../models/index';

export class Fight
{
    // fields
    private leftFighter: Fighter;
    private rightFighter: Fighter;

    // constructors
    public constructor(leftFighterInfo: FighterFullInfo, rightFighterInfo: FighterFullInfo) 
    {        
        this.leftFighter = new Fighter(leftFighterInfo);
        this.rightFighter = new Fighter(rightFighterInfo);
    }

    // PROPERTIES
    public get getLeftFighter() : Fighter
    {
        return this.leftFighter;
    }
    
    public get getRightFighter() : Fighter
    {
        return this.rightFighter;
    }

    // methods
    // there are better solutions,
    // but I want to try generator
    public *start() : IterableIterator<string>
    {   
        let round:number = 1;

        do
        {                
            // get round number
            yield String(round++)

            // calc damage
            const leftFighterProduceDamage = this.leftFighter.getHitPower() - this.rightFighter.getBlockPower();
            const rightFighterProduceDamage = this.rightFighter.getHitPower() - this.leftFighter.getBlockPower();
            
            // they can dodge damage
            this.leftFighter.setDamage(rightFighterProduceDamage);
            this.rightFighter.setDamage(leftFighterProduceDamage);

        } while(this.leftFighter.isAlive && this.rightFighter.isAlive);

        // get winner name
        if (!this.leftFighter.isAlive && !this.rightFighter.isAlive) return "a draw";
        else if (!this.leftFighter.isAlive)  return this.rightFighter.info.name;
        else if (!this.rightFighter.isAlive) return this.leftFighter.info.name;        
    }
}
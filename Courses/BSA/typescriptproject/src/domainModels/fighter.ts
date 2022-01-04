import { FighterFullInfo } from '../models/index'

export class Fighter
{
    // fields
    private maxHealth: number;
    private fighterData: FighterFullInfo;

    // constructors
    public constructor(fighterData: FighterFullInfo)
    {
        this.maxHealth = fighterData.health;
        this.fighterData = { ...fighterData };
    }

    // properties
    public get isAlive(): boolean
    {
        return this.fighterData.health > 0;
    }
    public get info(): FighterFullInfo
    {
        return this.fighterData;
    }
    public get lifePercentage(): number
    {
        if (!this.isAlive) return 0;
        return Math.round(this.fighterData.health * 100.0 / this.maxHealth);
    }

    // methods    
    public getHitPower() : number
    {
        const criticalHitChance:number = this.randomInt(1, 3);
        return Math.round(this.fighterData.attack * criticalHitChance);
    }

    public getBlockPower() : number
    {
        const dodgeChance:number = this.randomInt(1, 3);
        return Math.round(this.fighterData.defense * dodgeChance);
    }
    
    public setDamage(damage: number) : void
    {
        // set damage
        // only if not completly blocked
        if (damage > 0)
        {
            this.fighterData.health -= damage;
        }
    }

    private randomInt(min: number, max: number) : number
    {
        return Math.floor(Math.random() * (max - min)) + min;
    }
}
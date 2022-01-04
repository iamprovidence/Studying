import { FighterShortInfo } from './fighterShortInfo';

export interface FighterFullInfo extends FighterShortInfo 
{
    health: number;
    attack: number;
    defense: number;
}
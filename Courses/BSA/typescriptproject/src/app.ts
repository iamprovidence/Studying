import { ViewBase, FightersView, FightView, MenuView } from './view/index';
import { FighterService } from './services/fighterService';
import { FighterShortInfo, FighterFullInfo, UpdateFighterInfo, ClickEventHandler } from './models/index';
import { Fight } from './domainModels/index';

export default class App 
{
    // consts
    static rootElement = document.getElementById('root');
    static modalElement = document.getElementById('modal');
    static loadingElement = document.getElementById('loading-overlay');
    
    // fields
    private fightersDetailsMap:Map<string, FighterFullInfo>;

    private fight:Fight;    
    private roundResult:IterableIterator<string>;

    private fighterService: FighterService;

    private menuView: MenuView;
    private fightersView: FightersView;
    private fightView: FightView;

    private handleClick: ClickEventHandler<string>;
    private handleUpdateClick: ClickEventHandler<null>
    private handleStartFightClick : ClickEventHandler<null>;
    private handleNextRoundClick : ClickEventHandler<null>;
    private handleStartNewFightClick : ClickEventHandler<null>;

    // constructors
    constructor() 
    {
        this.fight = null;
        this.roundResult = null;

        this.fighterService = new FighterService();

        this.menuView = new MenuView();
        this.fightersView = new FightersView();        
        this.fightView = new FightView();

        this.fightersDetailsMap = new Map<string, FighterFullInfo>();

        this.handleClick = this.handlerFighterClick.bind(this);
        this.handleUpdateClick = this.handlerUpdateFighterClick.bind(this);
        this.handleStartFightClick = this.handlerStartFightClick.bind(this);
        this.handleNextRoundClick = this.handlerNextRoundClick.bind(this);
        this.handleStartNewFightClick = this.handlerStartNewFightClick.bind(this);
    }

    // properties
    public async getFighter(fighterId: string) : Promise<FighterFullInfo>
    {
        // load fighter if not exist
        if (!this.fightersDetailsMap.has(fighterId))
        {
            const fighterInfo: FighterFullInfo = await this.fighterService.getFighterDetails(fighterId);
            this.fightersDetailsMap.set(fighterId, fighterInfo);            
        }

        // get fighter
        return this.fightersDetailsMap.get(fighterId);
    }

    // methods
    public async run() 
    {
        App.loadingElement.style.visibility = 'visible';

        try 
        {
            await this.showAllFighters();
        }
        catch (error) 
        {
            console.warn(error);
            App.rootElement.innerText = 'Failed to load data';
        } 
        finally
        {
            App.loadingElement.style.visibility = 'hidden';
        }
    }
    private async showAllFighters(): Promise<void>
    {       
        const fighters:FighterShortInfo[] = await this.fighterService.getFighters();
        
        // fighters
        const fightersElement = this.fightersView.createFighters(fighters, this.handleClick);
        
        // menu
        const menuElement = this.menuView.createMenuContainer(fighters, this.handleStartFightClick);

        App.rootElement.append(fightersElement, menuElement);
    }


    // click handlers
    private async handlerFighterClick(event: MouseEvent, fighterId: string): Promise<void>
    {
        // get from map or load info and add to fightersMap
        const fightersDetails:FighterFullInfo = await this.getFighter(fighterId);

        // show modal with fighter info
        const divModalDetailElement: HTMLElement = this.fightersView.createFightersDetails(fightersDetails, this.handleUpdateClick);
        App.modalElement.appendChild(divModalDetailElement);        
        App.modalElement.style.display = "block";
    }

    private async handlerUpdateFighterClick(event: MouseEvent, args:null) : Promise<void>
    {
        // get update data 
        const updateFighterInfo: UpdateFighterInfo = this.fightersView.getFightersInfo();
        const { fighterId } = updateFighterInfo;

        // update property
        const fightersDetails:FighterFullInfo = await this.getFighter(fighterId);
        const fighterNewInfo:FighterFullInfo = { ...fightersDetails, ...updateFighterInfo };

        this.fightersDetailsMap.set(fighterId, fighterNewInfo);

        // hide modal
        App.modalElement.style.display = "none";
        ViewBase.clearElement(App.modalElement);
    }

    private async handlerStartFightClick(event: MouseEvent, args:null) : Promise<void>
    {
        // get selected fighters
        const leftFighterId = this.menuView.getSelectedFighterId("left-fighter");
        const rightFighterId = this.menuView.getSelectedFighterId("right-fighter");
                        
        // create fight and its view
        this.fight = new Fight(await this.getFighter(leftFighterId), await this.getFighter(rightFighterId));

        // start fight
        this.roundResult = this.fight.start();
        const roundResult = this.roundResult.next();

        // show fight        
        ViewBase.clearElement(App.rootElement);
        const fightElement = this.fightView.createFight(this.fight, roundResult, this.handleNextRoundClick);
        App.rootElement.append(fightElement);
    }
    private async handlerNextRoundClick(event: MouseEvent, args:null) : Promise<void>
    {        
        // clear panel
        ViewBase.clearElement(App.rootElement);

        // fight next round
        const roundResult = this.roundResult.next(); 

        // determine next round button click handler
        const handler = !roundResult.done ? this.handleNextRoundClick : this.handleStartNewFightClick;

        // create fight view
        const fightViewElement = this.fightView.createFight(this.fight, roundResult, handler);

        // add fight and button
        App.rootElement.append(fightViewElement);
    }
        
    private async handlerStartNewFightClick(event: MouseEvent, args:null) : Promise<void>
    {        
        // reset all values
        ViewBase.clearElement(App.rootElement);
        this.fight = null;
        this.roundResult = null;

        // go back to main screen
        this.showAllFighters();
    }
}

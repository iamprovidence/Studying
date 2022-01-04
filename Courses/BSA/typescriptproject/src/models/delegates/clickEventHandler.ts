export interface ClickEventHandler<TArgs>
{
    (event: MouseEvent, args?: TArgs): void;
}
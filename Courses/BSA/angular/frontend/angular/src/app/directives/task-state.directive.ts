import { Directive, Input, TemplateRef, ViewContainerRef, ElementRef } from '@angular/core';
 
@Directive({ selector: '[taskState]' })
export class TaskStateDirective 
{
    // fields
    private elementRef : ElementRef;

    // constructors
    constructor(elementRef: ElementRef) 
    {
        this.elementRef = elementRef;
    }

    // properties
    @Input() 
    set taskState(taskStateId : string) 
    {
        this.elementRef.nativeElement.style.background = this.getColor(parseInt(taskStateId));
    }

    // methods
    private getColor(taskStateId: number) : string
    {
        switch (taskStateId)
        {
            case 1: return "#17a2b8";   // created  = blue
            case 2: return "#ffc107";   // started  = yellow
            case 3: return "#28a745";   // finished = green
            case 4: return "#dc3545";   // canceled = red

            default: throw Error("Not expected task state id");
        }
    }
}
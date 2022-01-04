import { CreateElemetnDTO, ClickEventHandler } from '../../models/index';

export abstract class ViewBase
{  
    // static method
    public static clearElement(element: HTMLElement) : void
    {
        element.innerHTML = "";
    }

    // method
    public createButton(text: string, className: string, clickHandler: ClickEventHandler<null>)
    {
        const buttonElement: HTMLButtonElement = document.createElement("button");
        
        // add class
        buttonElement.classList.add(className);
        
        // add content
        buttonElement.innerText = text;

        // add event listener
        buttonElement.addEventListener('click', event => clickHandler(event, null), false);

        return buttonElement;
    }
    public createFighterImage(source: string) : HTMLElement
    {
        return this.createElement(
        {
            tagName: 'img',
            className: 'fighter-image',
            attributes: { src: source }
        });
    }  
    public createElement(createElement:CreateElemetnDTO) : HTMLElement
    {
        const element = document.createElement(createElement.tagName);

        // add content
        if (createElement.content)
        {
            element.innerHTML = createElement.content;
        }

        // add class
        if (createElement.className)
        {
            element.classList.add(createElement.className);
        }

        // add attributes
        if (createElement.attributes)
        {
            Object.keys(createElement.attributes)
                .forEach(key => element.setAttribute(key, createElement.attributes[key]));
        }

        return element;
    }
    
}



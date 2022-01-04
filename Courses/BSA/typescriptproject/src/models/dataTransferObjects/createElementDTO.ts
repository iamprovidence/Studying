import { Dictionary } from '../collections/dictionary';

export interface CreateElemetnDTO
{
    tagName: string;
    content?: string;
    className?: string;
    attributes?: Dictionary<string>;
}
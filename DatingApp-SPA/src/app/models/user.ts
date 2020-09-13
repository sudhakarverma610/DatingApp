import { Photo } from './Photo';

export interface User {
    id:number;
    userName:string;
    knowAs:string;
    age:number;
    gender:string;
    created:Date;
    lastActive:Date;
    photoUrl:string;
    city:string;
    country:string;
    introduction?:string;
    lookingFor?:string;
    interests?:string;
    photos?:Photo[];
}

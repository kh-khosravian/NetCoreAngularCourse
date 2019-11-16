import { PhotoModel } from './photoModel';

export interface UserModel {
    id: number;
    username: string;
    knownAs: string;
    age: number;
    gender: string;
    created: Date;
    photoURL: string;
    city: string;
    country: string;
    intrest?: string;
    lastActive?: Date;
    introduction?: string;
    lookingFor?: string;
    photos?: PhotoModel[];
}

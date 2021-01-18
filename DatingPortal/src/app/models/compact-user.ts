import { User } from './user';

export class CompactUser extends User
{
    age: number;
    created: Date;
    lastActive: Date;
    photoUrl: string;
}

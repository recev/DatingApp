import { Photo } from './photo';

export class DetailedUser {
  id: number;
  username: string;
  password: string;
  gender: string;
  introduction: string;
  lookingFor: string;
  interests: string;
  city: string;
  country: string;
  knownAs: string;
  age: number;
  created: Date;
  lastActive: Date;
  photoUrl: string;
  photos: Photo[];
}

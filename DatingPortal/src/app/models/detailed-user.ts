import { Photo } from './photo';
import { User } from './user';

export class DetailedUser extends User{
  introduction: string;
  lookingFor: string;
  interests: string;
  age: number;
  created: Date;
  lastActive: Date;
  photoUrl: string;
  photos: Photo[];
  unApprovedPhotos: Photo[];
  roles: string[];
}

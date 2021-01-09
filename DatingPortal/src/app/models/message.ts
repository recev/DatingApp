export class Message {
  id: number;

  senderId: number;
  senderKnownAs: string;
  senderPhotoUrl: string;

  recipientId: number;
  recipientKnownAs: string;
  recipientPhotoUrl: string;

  sentDate: Date;
  readDate: Date;

  isRead: boolean;
  isSenderDeleted: boolean;
  isRecipientDeleted: boolean;

  content: string;
}

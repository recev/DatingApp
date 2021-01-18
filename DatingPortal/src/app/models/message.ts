export class Message {
  id: number;

  senderId: string;
  senderKnownAs: string;
  senderPhotoUrl: string;

  recipientId: string;
  recipientKnownAs: string;
  recipientPhotoUrl: string;

  sentDate: Date;
  readDate: Date;

  isRead: boolean;
  isSenderDeleted: boolean;
  isRecipientDeleted: boolean;

  content: string;
}

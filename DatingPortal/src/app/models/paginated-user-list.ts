import { CompactUser } from './compact-user';

export class PaginatedUserList {
  pageNumber: number;
  pageSize: number;
  totalUserCount: number;
  totalPageCount: number;
  users: CompactUser[];
}

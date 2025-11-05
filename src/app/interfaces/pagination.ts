export interface Pagination {
  currentPage:number;
  totalPages:number;
  itemsPerPage:number;
  totalItems:number;
}
export class PaginationResult<T> {
  result? : T;
  Pagination?: Pagination;
}

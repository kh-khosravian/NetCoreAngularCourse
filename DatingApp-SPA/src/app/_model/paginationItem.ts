
export interface PaginationItem {
    currentPage: string;
    itemsPerPage: string;
    totalItems: number;
    totalPages: string;
}

export class PaginatedResult<T> {
    result: T;
    pagination: PaginationItem;
}
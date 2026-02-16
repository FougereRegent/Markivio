export interface OffsetPagination<T> {
    Data: Array<T>
    Count: number,
    HasNextPage: boolean,
    HasPreviousPage: boolean,
}

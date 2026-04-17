export interface OffsetPagination<T> {
  data: Array<T>
  count: number
  hasNextPage: boolean
  hasPreviousPage: boolean
}

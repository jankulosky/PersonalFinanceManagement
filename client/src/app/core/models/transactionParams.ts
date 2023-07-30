export class TransactionParams {
  pageNumber = 1;
  pageSize = 10;
  kind = '';
  startDate!: Date | null;
  endDate!: Date | null;
  sortBy = 'date';
  sortOrder = 'asc';
}

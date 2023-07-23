import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FileParams } from 'src/app/core/models/fileParams';
import { Pagination } from 'src/app/core/models/pagination';
import { Transaction } from 'src/app/core/models/transaction';
import { TransactionsService } from 'src/app/core/services/transactions.service';

@Component({
  selector: 'app-transaction-list',
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.scss'],
})
export class TransactionListComponent implements OnInit {
  displayedColumns: string[] = [
    'id',
    'beneficiary-name',
    'date',
    'direction',
    'amount',
    'description',
    'currency',
    'mcc',
    'kind',
    'action',
  ];

  transactions: Transaction[] = [];
  dataSource!: MatTableDataSource<Transaction>;
  fileParams: FileParams = new FileParams();
  pagination!: Pagination;
  errorMessage: string = '';
  showFirstLastButtons = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(private transactionsService: TransactionsService) {}

  ngOnInit(): void {
    this.loadTransactions();
  }

  loadTransactions() {
    this.transactionsService.getTransactions(this.fileParams).subscribe({
      next: (response) => {
        if (response.pagination) {
          this.dataSource = new MatTableDataSource(response.result);
          this.pagination = response.pagination;
          this.dataSource.sort = this.sort;
        }
      },
      error: (err) => {
        this.errorMessage = err;
      },
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  onPageChange(event: PageEvent) {
    this.fileParams.pageNumber = event.pageIndex + 1;
    this.fileParams.pageSize = event.pageSize;
    this.loadTransactions();
  }
}

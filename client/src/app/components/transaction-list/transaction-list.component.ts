import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TransactionParams } from 'src/app/core/models/transactionParams';
import { Pagination } from 'src/app/core/models/pagination';
import { Transaction } from 'src/app/core/models/transaction';
import { TransactionsService } from 'src/app/core/services/transactions.service';
import { MatInput } from '@angular/material/input';
import { CategorizeComponent } from '../categorize/categorize.component';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';

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
  transactionParams: TransactionParams = new TransactionParams();
  pagination!: Pagination;
  errorMessage: string = '';
  showFirstLastButtons = true;

  kindList = [
    { value: '', display: '- All Kinds -' },
    { value: 'pmt', display: 'pmt' },
    { value: 'wdw', display: 'wdw' },
    { value: 'dep', display: 'dep' },
    { value: 'fee', display: 'fee' },
    { value: 'sal', display: 'sal' },
  ];

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild('kindSelect') kindSelect!: MatInput;
  @ViewChild('fromInput', { read: MatInput }) fromInput!: MatInput;
  @ViewChild('toInput', { read: MatInput }) toInput!: MatInput;

  constructor(
    private transactionsService: TransactionsService,
    public dialog: MatDialog,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadTransactions();
  }

  loadTransactions() {
    this.transactionsService.getTransactions(this.transactionParams).subscribe({
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

  onPageChange(event: PageEvent) {
    this.transactionParams.pageNumber = event.pageIndex + 1;
    this.transactionParams.pageSize = event.pageSize;

    this.loadTransactions();
  }

  handleKindChange(event: any) {
    this.transactionParams.kind = event.value;
  }

  handleStartDateChange(event: any) {
    if (event.value) {
      this.transactionParams.startDate = event.value;
    }
  }

  handleEndDateChange(event: any) {
    if (event.value) {
      this.transactionParams.endDate = event.value;
    }
  }

  handleFilterTransactions() {
    this.loadTransactions();
  }

  handleClearSelections() {
    this.transactionParams.kind = '';
    this.transactionParams.startDate = null;
    this.transactionParams.endDate = null;

    this.kindSelect.value = '';
    this.fromInput.value = '';
    this.toInput.value = '';

    this.loadTransactions();
  }

  openCategorizeDialog(transaction: Transaction) {
    const dialogRef = this.dialog.open(CategorizeComponent, {
      width: '600px',
      height: '250px',
      data: { transactinId: transaction.id },
    });

    dialogRef.afterClosed().subscribe(({ catCode }) => {
      if (catCode) {
        this.transactionsService
          .categorizeTransaction(transaction.id, catCode)
          .subscribe({
            next: (response) => {
              const i = this.transactions.findIndex(
                (t) => t.id == transaction.id
              );
              if (i >= 0) {
                this.transactions[i] = response;
              }
              this.toastr.success(
                'Transaction categorized successfully!',
                'Success'
              );
            },
            error: (err) => {
              this.toastr.error(err.error);
            },
          });
      }
    });
  }
}

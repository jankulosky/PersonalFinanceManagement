import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Category } from 'src/app/core/models/category';
import { CategoryParams } from 'src/app/core/models/categoryParams';
import { CategoriesService } from 'src/app/core/services/categories.service';

@Component({
  selector: 'app-categorize',
  templateUrl: './categorize.component.html',
  styleUrls: ['./categorize.component.scss'],
})
export class CategorizeComponent implements OnInit {
  categories: Category[] = [];
  categoryParams: CategoryParams = new CategoryParams();
  catCode!: string;

  constructor(
    private categoriesService: CategoriesService,
    public dialogRef: MatDialogRef<CategorizeComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { transactionId: number }
  ) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  getCategoryTooltip(category: Category): string {
    if (category.parentCode) {
      return `Code: ${category.code}, Name: ${category.name}, Parent Code: ${category.parentCode}`;
    } else {
      return `Code: ${category.code}, Name: ${category.name}, No Parent Code`;
    }
  }

  loadCategories() {
    this.categoriesService.getCategories(this.categoryParams).subscribe({
      next: (response) => {
        this.categories = response;
        console.log(response);
      },
      error: (error) => {
        console.error(error);
      },
    });
  }

  onConfirm(): void {
    this.dialogRef.close({ catCode: this.catCode });
  }
}

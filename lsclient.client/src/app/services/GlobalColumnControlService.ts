import { Injectable } from '@angular/core';
import { TableColumn } from '../models/components/table-columns';

@Injectable({
  providedIn: 'root'
})
export class GlobalColumnControlService {
  private columnDialogConfig = {
     responsive: true
  };

  private sourceColumns: TableColumn[] = []; // Columns available to add
  private targetColumns: TableColumn[] = []; // Columns currently displayed in the table
  first: number = 0;
  rows: number = 10;

  constructor() { }

  // Get the current dialog configuration
  getColumnDialogConfig() {
    return this.columnDialogConfig;
  }

  // Set or update the dialog configuration
  setColumnDialogConfig(config: any) {
    this.columnDialogConfig = { ...this.columnDialogConfig, ...config };
  }

  // Get source columns
  getSourceColumns() {
    return this.sourceColumns;
  }

  // Set source columns
  setSourceColumns(columns: any[]) {
    this.sourceColumns = columns;
  }

  // Get target columns
  getTargetColumns() {
    return this.targetColumns;
  }

  // Set target columns
  setTargetColumns(columns: any[]) {
    this.targetColumns = columns;
  }

  getNestedValue(rowData: any, field: string): any {
    return field.split('.').reduce((acc, part) => acc && acc[part], rowData) || null;
  }
  
  pageChange(event: any): void {
    this.first = event.first;
    this.rows = event.rows;
  }

  
  next(): void {
    this.first += this.rows;
  }

  prev(): void {
    this.first -= this.rows;
  }

  reset(): void {
    this.first = 0;
  }
  isFirstPage(): boolean {
    return this.first === 0;
  }

  
//#region  TableFilter
filterData<T extends Record<string, any>>(data: T[], searchTerm: string, fields: string[]): T[] {
  if (!searchTerm) {
    return data;
  }

  return data.filter((item: T) =>
    fields.some(field => {
      // Resolve nested fields dynamically
      const fieldValue = field.split('.').reduce((acc, part) => acc && acc[part], item);

      return fieldValue && fieldValue.toString().toLowerCase().includes(searchTerm.toLowerCase());
    })
  );
}

//#endregion

}

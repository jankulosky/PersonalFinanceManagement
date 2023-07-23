import { Category } from './category';

export interface Transaction {
  id: number;
  beneficiaryName?: string;
  date: Date;
  direction: string;
  amount: number;
  description: string;
  currency: string;
  mcc?: number;
  kind: string;
  category?: Category;
}

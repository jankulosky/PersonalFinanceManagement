import { Category } from './category';
import { Kind } from './kind';

export interface Transaction {
  id: number;
  beneficiaryName?: string;
  date: Date;
  direction: string;
  amount: number;
  description: string;
  currency: string;
  mcc?: number;
  kind: Kind;
  category?: Category;
}

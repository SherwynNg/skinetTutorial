import { IProduct } from './product';

export interface IPagingation {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: IProduct[];
}

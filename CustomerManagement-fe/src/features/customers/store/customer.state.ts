import { Customer } from "../models/customer.model";

export interface CustomerState {
  customers: Customer[];
  loading: boolean;
  error: string | null;
}

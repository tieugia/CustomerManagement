import { Customer } from "../../models/customer.model";
import { Observable, takeUntil } from "rxjs";
import { loadCustomers } from "../../store/customer.actions";
import { Store } from "@ngrx/store";
import { selectAllCustomers } from "../../store/customer.selectors";
import { Component, OnInit } from "@angular/core";
import { CommonModule } from "@angular/common";
import { MatTableModule } from "@angular/material/table";
import { MatToolbarModule } from "@angular/material/toolbar";
import { CustomerState } from "../../store/customer.state";
import { MatCardModule } from "@angular/material/card";
import { BaseComponent } from "../../../base/base.component";

@Component({
  selector: 'app-customer-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatToolbarModule, MatCardModule],
  templateUrl: './customer-list.component.html',
  styleUrl: './customer-list.component.scss',
})
export class CustomerListComponent extends BaseComponent implements OnInit {
  customers$: Observable<Customer[]> | undefined;
  constructor(private store: Store<CustomerState>) {
    super();
  }

  override ngOnInit() {
    super.ngOnInit();
    this.customers$ = this.store.select(selectAllCustomers).pipe(takeUntil(this.destroy$));
    this.store.dispatch(loadCustomers());
  }
}

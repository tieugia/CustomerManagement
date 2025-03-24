import { addCustomer } from "../../store/customer.actions";
import { Component } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Store } from "@ngrx/store";
import { Customer } from "../../models/customer.model";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatCardModule } from "@angular/material/card";
import {MatInputModule} from '@angular/material/input';
import { MatButtonModule } from "@angular/material/button";
import { CommonModule } from "@angular/common";
import { CustomerState } from "../../store/customer.state";
import { MatSnackBarModule } from "@angular/material/snack-bar";

@Component({
    selector: 'app-customer-form',
    templateUrl: './customer-form.component.html',
    styleUrl: './customer-form.component.scss',
    imports: [MatFormFieldModule, MatCardModule, ReactiveFormsModule, MatInputModule, MatButtonModule, CommonModule, MatSnackBarModule],
})
export class CustomerFormComponent {
    form: any;

    constructor(private fb: FormBuilder, private store: Store<CustomerState>) {
        this.form = this.fb.group({
            firstName: ['', Validators.required],
            middleName: [''],
            lastName: ['', Validators.required],
            email: ['', [Validators.required, Validators.email]]
        });
    }

    submit() {
        if (this.form.valid) {
            const customer: Customer = {
                ...this.form.value
            };
            this.store.dispatch(addCustomer({ customer }));
        }
    }
}
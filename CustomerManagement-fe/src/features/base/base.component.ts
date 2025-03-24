import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';

@Component({
    selector: 'app-base',
    template: ''
})
export abstract class BaseComponent implements OnInit, OnDestroy {
    destroy$ = new Subject<void>();

    ngOnInit(): void {
        this.destroy$.next();
    }

    ngOnDestroy(): void {
        this.destroy$.complete();
    }
}

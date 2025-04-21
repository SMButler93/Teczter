import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { ITest } from '../Interfaces/test-interface';
import { TestService } from '../test-services/test-service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-test',
  standalone: true,
  imports: [],
  templateUrl: './test.component.html',
  styleUrl: './test.component.css'
})
export class TestComponent implements OnInit, OnDestroy {
  
  @Input({required: true}) id!: number;
  test!: ITest;

  constructor(
    private _testService: TestService
  ) {}

  unsubscribe$ = new Subject<void>;

  ngOnInit(): void {
    this._testService.getTestById(this.id)
    .pipe(takeUntil(this.unsubscribe$))
    .subscribe(test => this.test = test);
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
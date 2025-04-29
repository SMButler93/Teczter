import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TestService } from '../test-services/test-service';
import { map, switchMap, Observable } from 'rxjs';
import { ITest } from '../Interfaces/test-interface';
import { CommonModule } from '@angular/common';
import { TestStepComponent } from '../test-step/test-step.component';

@Component({
  selector: 'app-test',
  standalone: true,
  templateUrl: './test.component.html',
  styleUrl: './test.component.css',
  imports: [
    CommonModule,
    TestStepComponent
  ]
})
export class TestComponent implements OnInit {
  test$?: Observable<ITest>;

  constructor(
    private _testService: TestService,
    private _route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.test$ = this._route.paramMap.pipe(
      map(params => Number(params.get('testId'))),
      switchMap(id => this._testService.getTestById(id)),
    );
  }
}

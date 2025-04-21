import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestStepComponent } from './test-step.component';

describe('TestStepComponent', () => {
  let component: TestStepComponent;
  let fixture: ComponentFixture<TestStepComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TestStepComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TestStepComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { Component, Input } from '@angular/core';
import { ITestStep } from '../Interfaces/test-step-interface';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TextareaModule } from 'primeng/textarea';

@Component({
  selector: 'app-test-step',
  standalone: true,
  imports: [CardModule, ButtonModule, CommonModule, FormsModule, TextareaModule],
  templateUrl: './test-step.component.html',
  styleUrl: './test-step.component.css'
})
export class TestStepComponent {

  @Input({required: true}) step!: ITestStep

  isEditMode = false;

  enterEditMode(){
    this.isEditMode = true;
  }
}

import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { QuestionBase } from '../questions-list.component';

@Component({
  selector: 'app-question-form',
  templateUrl: './question-form.component.html',
  styleUrls: ['./question-form.component.scss']
})
export class QuestionFormComponent {
  @Input()
  question!: QuestionBase<string>;
  @Input()
  form!: FormGroup;
  get isValid() { return this.form.controls[this.question.key].valid; }
}

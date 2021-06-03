import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { CreateGigCommand, GigsClient } from 'src/app/clients/gigs-client';

@Component({
  selector: 'app-gig-create',
  templateUrl: './gig-create.component.html',
  styleUrls: ['./gig-create.component.scss']
})
export class GigCreateComponent implements OnInit {
  isLinear = true;
  gigForm = this.fb.group({
    title: ['', Validators.required],
    description: ['', Validators.required],
    // todo: put requred in real implmentation
    categoryId: [''],
    questions: this.fb.array([ this.initQuestion() ]),
  });
  imageForm = this.fb.group({
    image: ['']
  })

  imageSrc: string = '';

  constructor(
    public dialogRef: MatDialogRef<GigCreateComponent>,
    private fb: FormBuilder,
    private spinnerService: NgxSpinnerService,
    private gigsApiClient: GigsClient) { }

  ngOnInit(): void {
  }

  getErrorMessage(key: string, error: string = 'required') {
    const control = this.gigForm.get(key)!;
    if (control.hasError('required')) {
      return 'You must enter a value';
    }

    return control.hasError('email') ? 'Not a valid email' : '';
  }

  onFileChange(event: any) {
    const reader = new FileReader();
    
    if (event.target.files && event.target.files.length) {
      const [file] = event.target.files;
      reader.readAsDataURL(file);
    
      reader.onload = () => {
   
        this.imageSrc = reader.result as string;
     
        this.imageForm.patchValue({
          fileSource: reader.result
        });
   
      };
   
    }
  }
  
  onNoClick(): void {
    this.dialogRef.close();
  }

  onSubmit() {
    console.log(this.gigForm.value);
  }

  addQuestion() {
    const control = <FormArray>this.gigForm.controls['questions'];
    control.push(this.initQuestion());
  }

  removeQuestion(i: number) {
    const control = <FormArray>this.gigForm.controls['questions'];
    control.removeAt(i);
  }

  private initQuestion() {
    return this.fb.group({
        title: ['', Validators.required],
        answer: ['', Validators.required]
    });
  }

}

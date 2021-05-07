import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { GigsClient, CategoryDto, CreateGigCommand } from 'src/app/clients/gigs-client';
import { NotificationService } from 'src/app/core/services/notification.service';

@Component({
  selector: 'app-gigs-create',
  templateUrl: './gigs-create.component.html',
  styleUrls: ['./gigs-create.component.scss']
})
export class GigsCreateComponent implements OnInit {
  form!: FormGroup;
  
  constructor(
    private gigsApiClient: GigsClient,
    private formBuilder: FormBuilder,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      categoryId: [-1, Validators.required],
      planId: [null],
      tags: [[]],
      questions: [[]]
    })
  }

  public onSubmit() {
    // TODO:
    let data: any = {};
    for (let key in this.form.controls) {
      data[key] = this.form.controls[key].value
    }
    const command = CreateGigCommand.fromJS(data)

    this.gigsApiClient.post(command)
      .subscribe(response => this.notificationService.openSnackBar("Created"))
  }


  public get tags() : string[] {
    return this.form.controls['tags'].value
  }

  receiveTagsList($event: any) {
    this.form.controls['tags'].setValue($event)
  }

  storeCategoryId($event: CategoryDto) {
    this.form.controls['categoryId'].setValue($event.id)
  }

}

import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { NotificationService } from 'src/app/core/services/notification.service';
import { GigsClient, UpdateGigCommand } from 'src/app/clients/gigs-client';
import { MatChipInputEvent } from '@angular/material/chips';


@Component({
  selector: 'app-gigs-details',
  templateUrl: './gigs-details.component.html',
  styleUrls: ['./gigs-details.component.scss']
})
export class GigsDetailsComponent implements OnInit, OnDestroy {
  gig$!: Subscription;
  form!: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private gigsApiClient: GigsClient,
    private notificationService: NotificationService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      id: [0, Validators.required],
      title: ['', Validators.required],
      description: ['', Validators.required],
      isDraft: [true, Validators.required],
      planId: [null],
      tags: [[]],
      questions: [[]]
    })

    this.gig$ = this.route.params
      .pipe(map(params => +params['id']),
        switchMap(id => this.gigsApiClient.getGigById(id)))
      .subscribe(gig => this.form.patchValue(gig))
  }

  ngOnDestroy(): void {
    this.gig$.unsubscribe();
  }

  public onSubmit() {
    debugger;
    let data: any = {};
    for (let key in this.form.controls) {
      data[key] = this.form.controls[key].value
    }
    data.isDraft = (data.isDraft == 'true')
    const command = UpdateGigCommand.fromJS(data)
    const id = +data['id']

    this.gigsApiClient.update(id, command)
      .subscribe(response => this.notificationService.openSnackBar("Updated"))
  }

  public isInDraftModeValue() {
    const isInDraft: boolean = this.form.controls['isDraft'].value
    return JSON.stringify(+isInDraft);
  }

  
  public get tags() : string[] {
    return this.form.controls['tags'].value
  }

  receiveTagsList($event: any) {
    this.form.controls['tags'].setValue($event)
  }
}

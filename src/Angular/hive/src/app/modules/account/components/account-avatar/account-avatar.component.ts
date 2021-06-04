import { Component, Input, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { FileUpload, ProfileClient } from 'src/app/clients/profile-client';
import { NotificationService } from 'src/app/modules/core/services/notification.service';
import { ProfileService } from '../../services/profile.service';

@Component({
  selector: 'app-account-avatar',
  templateUrl: './account-avatar.component.html',
  styleUrls: ['./account-avatar.component.scss']
})
export class AccountAvatarComponent implements OnInit {
  private unsubscribe = new Subject();
  private id: number | undefined;
  
  @Input('name') fullName: string | undefined;
    
  url: string | null = '';

  constructor(
    private profileService: ProfileService,
    private notificationService: NotificationService,
    private profileApiClient: ProfileClient) { }

  ngOnInit(): void {
    this.url = this.url || '/assets/user.png';

    this.profileService.profile$
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(data => this.id = data?.id)
  }

  onSelectFile(event: any) {
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]); // read file as data url

      reader.onload = (event) => { // called once readAsDataURL is completed
        this.url = <string>event.target!.result;
        if (this.id) {
          var base64result = this.url.split(',')[1];
          this.profileApiClient
            .changeAvatar(this.id!, FileUpload.fromJS({ fileData: base64result }))
            .subscribe();
        }
        else {
          this.notificationService.openSnackBar('Cannot upload because profile could not be loaded.')
        }
      }
    }
  }

  dataURIToBlob(dataURI: string) {
    const splitDataURI = dataURI.split(',')
    const byteString = splitDataURI[0].indexOf('base64') >= 0 ? atob(splitDataURI[1]) : decodeURI(splitDataURI[1])
    const mimeString = splitDataURI[0].split(':')[1].split(';')[0]
        
    const ia = new Uint8Array(byteString.length)
    for (let i = 0; i < byteString.length; i++)
        ia[i] = byteString.charCodeAt(i)
      
        return new Blob([ia], { type: mimeString })
  }
}

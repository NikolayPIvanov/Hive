import { Component, Input, OnInit } from '@angular/core';
import { from, Subject } from 'rxjs';
import { map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { FileUpload, ProfileClient, UserProfileDto } from 'src/app/clients/profile-client';
import { NotificationService } from 'src/app/modules/core/services/notification.service';
import { ProfileService } from '../../services/profile.service';
import { DomSanitizer } from '@angular/platform-browser';


@Component({
  selector: 'app-account-avatar',
  templateUrl: './account-avatar.component.html',
  styleUrls: ['./account-avatar.component.scss']
})
export class AccountAvatarComponent implements OnInit {
  private DEFAULT_IMAGE = '/assets/user.png';
  private unsubscribe = new Subject();
  private id: number | undefined;
  
  @Input() profile!: UserProfileDto;
  avatarFile: string | null = '';

  constructor(
    private notificationService: NotificationService,
    private profileApiClient: ProfileClient,
    public domSanitizer: DomSanitizer) { }

  ngOnInit(): void {
    this.avatarFile = this.profile.avatarFile || this.DEFAULT_IMAGE;
    if (this.avatarFile != this.DEFAULT_IMAGE) {
      this.profileApiClient.getAvatar(this.profile.id!)
        .pipe(
          takeUntil(this.unsubscribe),
          map(image => this.createImageFromBlob(image.data)))
        .subscribe();
    }
  }

  createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      const base64Image = this.domSanitizer.bypassSecurityTrustUrl(reader.result as string);
      this.avatarFile = (base64Image as string)
    }, false);
 
    if (image) {
       reader.readAsDataURL(image);
    }
 }

  onSelectFile(event: any) {
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]); // read file as data url

      reader.onload = (event) => { // called once readAsDataURL is completed
        this.avatarFile = <string>event.target!.result;
        if (this.id) {
          var base64result = this.avatarFile.split(',')[1];
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

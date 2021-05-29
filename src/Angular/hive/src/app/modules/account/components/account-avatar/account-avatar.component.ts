import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-account-avatar',
  templateUrl: './account-avatar.component.html',
  styleUrls: ['./account-avatar.component.scss']
})
export class AccountAvatarComponent implements OnInit {
  @Input('name') fullName: string | undefined;

  url: string | null = '';

  ngOnInit(): void {
    this.url = this.url || '/assets/user.png';
  }

  onSelectFile(event: any) {
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]); // read file as data url

      reader.onload = (event) => { // called once readAsDataURL is completed
        this.url = <string>event.target!.result;
      }
    }
  }
}

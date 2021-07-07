import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { NotificationService } from 'src/app/modules/core/services/notification.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registerForm!: FormGroup;
  loading!: boolean;
  
  constructor(private router: Router,
    private titleService: Title,
    private notificationService: NotificationService) { }

  ngOnInit() {
    this.titleService.setTitle('Hive - Register');
    this.createForm();
  }

  private createForm() {

    this.registerForm = new FormGroup({
          name: new FormControl('', Validators.required),
          email: new FormControl('', [Validators.required, Validators.email]),
          password: new FormControl('', Validators.required),
      });
  }

  register() {
  }

}

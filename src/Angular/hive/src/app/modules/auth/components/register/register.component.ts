import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Title } from '@angular/platform-browser';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/modules/core/services/auth.service';
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
    private notificationService: NotificationService,
    private authenticationService: AuthenticationService) { }

  ngOnInit() {
    this.titleService.setTitle('Hive - Register');
    this.authenticationService.logout();
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
    const email = this.registerForm!.get('email')!.value;
    const password = this.registerForm!.get('password')!.value;
    const name = this.registerForm.get('name')!.value;

    this.loading = true;
    this.authenticationService
        .register(name, email.toLowerCase(), password)
        .subscribe(
            data => this.router.navigate(['/auth/login']),
            error => {
                this.notificationService.openSnackBar(error.error);
                this.loading = false;
            }
        );
}

}

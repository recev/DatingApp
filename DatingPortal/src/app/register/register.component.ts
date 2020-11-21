import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { AuthorizationService } from '../services/authorization.service';
import { ToastrService } from 'ngx-toastr';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { faSlash, faSearch } from '@fortawesome/free-solid-svg-icons';
import { DetailedUser } from '../models/detailed-user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() RegisterationCancelled = new EventEmitter<boolean>();
  registerForm: FormGroup;

  constructor(
    private authService: AuthorizationService,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
        username: new FormControl('', Validators.required),
        password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
        confirmPassword: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
        gender: new FormControl('', Validators.required),
        knownAs: new FormControl('', Validators.required),
        dateOfBirth: new FormControl('', Validators.required),
        city: new FormControl('', Validators.required),
        country: new FormControl('', Validators.required)
      },
      [this.PasswordCompareValidator]
    );
  }

  register(){
    console.log(this.registerForm.value);
    const user = this.registerForm.value as DetailedUser;

    this.authService.register(user.username, user.password)
    .subscribe(value => {
      this.toastr.success('User registred successfuly');
    }, error => {
      this.toastr.error(error);
    });
  }

  cancel()
  {
    this.RegisterationCancelled.emit(true);
  }

  PasswordCompareValidator(formGroup: FormGroup) {

      const password = formGroup.get('password').value;
      const confirmPassword = formGroup.get('confirmPassword').value;

      return password === confirmPassword ? null : { missmatch: true };
  }

  isValid(controlName: string)
  {
    const valid = this.registerForm.get(controlName).valid;
    const touched = this.registerForm.get(controlName).touched;

    if (touched === true && valid === true)
    {
      return true;
    }
    else
    {
      return false;
    }
  }

  isSupplied(controlName: string)
  {
    const requiredError = this.registerForm.get(controlName).hasError('required');
    const touched = this.registerForm.get(controlName).touched;
    const pristine = this.registerForm.get(controlName).pristine;

    if (pristine === true)
    {
      return true;
    }
    else if (touched === true && requiredError === false)
    {
      return true;
    }
    else
    {
      return false;
    }
  }

  arePasswordsMatched()
  {
    const missmatchError = this.registerForm.hasError('missmatch');
    const passwordTouched = this.registerForm.get('password').touched;
    const confirmPasswordTouched = this.registerForm.get('confirmPassword').touched;

    const passwordPristine = this.registerForm.get('password').pristine;
    const confirmPasswordPristine = this.registerForm.get('confirmPassword').pristine;

    if (passwordPristine === true || confirmPasswordPristine === true)
    {
      return true;
    }

    if (passwordTouched === false || confirmPasswordTouched === false)
    {
      return true;
    }

    return missmatchError === true ? false : true;
  }
}

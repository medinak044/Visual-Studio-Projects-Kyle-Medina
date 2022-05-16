import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter()
  model: any = {}
  registerForm: FormGroup
  maxDate: Date

  constructor(private accountService: AccountService, private toastr: ToastrService, private fb: FormBuilder) { }

  ngOnInit(): void {
    this.initializeForm()
    this.maxDate = new Date()
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18) // Because the minimum age to use site is 18, and this will set the initial datepicker date value 18 years back when user selects the input box
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    })

    // When "password" value changes, updates the validity of "password" against "confirmPassword"
    this.registerForm.controls.password.valueChanges.subscribe({
      next: () => this.registerForm.controls.confirmPassword.updateValueAndValidity()
    })
  }

  // Checks if both "password" and "confirmPassword" values are the same
  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[matchTo].value
        ? null : { isMatching: true }
    }
  }

  register() {
    console.log(this.registerForm.value)
    // this.accountService.register(this.model).subscribe({
    //   next: response => {
    //     console.log(response)
    //     this.cancel()
    //   },
    //   error: err => {
    //     console.log(err)
    //     this.toastr.error(err.error)
    //   }
    // })
  }

  cancel() {
    this.cancelRegister.emit(false)
  }
}

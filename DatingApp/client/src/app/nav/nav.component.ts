import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})

export class NavComponent implements OnInit {
  model: any = {}

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  login() {
    this.accountService.login(this.model).subscribe({
      next: res => this.router.navigateByUrl('/members'),
    })

    // this.accountService.login(this.model).subscribe(response => {
    //   this.router.navigateByUrl('/members')
    // }, err => {
    //   console.log(err)
    //   this.toastr.error(err.error)
    // })

    // try {
    //   this.accountService.login(this.model).subscribe(response => {
    //     this.router.navigateByUrl('/members')
    //   })
    // } catch (err) {
    //   console.log(err)
    //   this.toastr.error(err.error)
    // }
  }

  logout() {
    this.accountService.logout()
    this.router.navigateByUrl('/') // Redirect user to home page
  }

}

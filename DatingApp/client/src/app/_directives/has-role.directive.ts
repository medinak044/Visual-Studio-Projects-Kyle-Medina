import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { RouterEvent } from '@angular/router';
import { take } from 'rxjs/operators';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Directive({
  selector: '[appHasRole]' // *appHasRole="['Admin']"
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[]
  user: User

  constructor(private viewContainerRef: ViewContainerRef, private templateRef: TemplateRef<any>,
    private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        this.user = user
      }
    })
  }

  ngOnInit(): void {
    // Clear view if no roles
    if (!this.user?.roles || this.user == null) {
      this.viewContainerRef.clear()
      return
    }

    // If the user has a role, create embedded view
    if (this.user?.roles.some(role => this.appHasRole.includes(role))) {
      this.viewContainerRef.createEmbeddedView(this.templateRef)
    } else {
      this.viewContainerRef.clear()
    }
  }

}

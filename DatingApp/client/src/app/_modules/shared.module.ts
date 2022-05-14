import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    BsDropdownModule.forRoot(), // "forRoot()" Loads all the neccessary services for the root module
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right'
    })
  ],
  exports: [ // "exports:" enables the included modules to be made available to any other script
    BsDropdownModule,
    ToastrModule
  ]
})
export class SharedModule { }

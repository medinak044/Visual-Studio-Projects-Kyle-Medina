import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { EditHeroComponent } from './components/edit-hero/edit-hero.component';


@NgModule({
  declarations: [
    AppComponent,
    EditHeroComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule, // Allows this client app to perform HTTP requests
    FormsModule, // ngModel
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

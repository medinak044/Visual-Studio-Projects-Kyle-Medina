import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Dating app';
  users: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getUsers()
  }

  // Using the observer pattern
  getUsers() {
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => this.users = response,
      error: error => console.log(error)
    })
  }

  /// (Depricated)
  // getUsers() {
  //   // next time use try/catch for http requests + error handling
  //   this.http.get('https://localhost:5001/api/users').subscribe(response => {
  //     this.users = response
  //   }, error => {
  //     console.log(error)
  //   })
  // }

}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Hero } from '../models/hero';

@Injectable({
  providedIn: 'root'
})
export class HeroService {
  private url = "Hero" // From "HeroController"
  constructor(private http: HttpClient) { }

  public getHeroes(): Observable<Hero[]> {
    return this.http.get<Hero[]>
      (`${environment.apiUrl}/${this.url}`)
  }

  public updateHero(hero: Hero): Observable<Hero[]> {
    return this.http.put<Hero[]>
      (`${environment.apiUrl}/${this.url}`, hero)
  }

  public createHero(hero: Hero): Observable<Hero[]> {
    return this.http.post<Hero[]>
      (`${environment.apiUrl}/${this.url}`, hero)
  }

  public deleteHero(hero: Hero): Observable<Hero[]> {
    return this.http.delete<Hero[]>
      (`${environment.apiUrl}/${this.url}/${hero.id}`)
  }
}

import { Component } from '@angular/core';
import { Hero } from './models/hero';
import { HeroService } from './services/hero.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'client';
  heroes: Hero[] = []
  heroToEdit?: Hero

  constructor(private heroService: HeroService) { }

  ngOnInit(): void {
    this.heroService
      .getHeroes()
      .subscribe((result: Hero[]) => (this.heroes = result))
  }

  updateHeroList(heroes: Hero[]) {
    this.heroes = heroes
  }

  initNewHero() {
    this.heroToEdit = new Hero
  }

  editHero(hero: Hero) {
    this.heroToEdit = hero
  }
}

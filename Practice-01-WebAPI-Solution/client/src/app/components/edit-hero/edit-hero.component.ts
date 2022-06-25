import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Hero } from 'src/app/models/hero';
import { HeroService } from 'src/app/services/hero.service';

@Component({
  selector: 'app-edit-hero',
  templateUrl: './edit-hero.component.html',
  styleUrls: ['./edit-hero.component.css']
})
export class EditHeroComponent implements OnInit {
  @Input() hero?: Hero
  @Output() heroesUpdated = new EventEmitter<Hero[]>()

  constructor(private heroService: HeroService) { }

  ngOnInit(): void {
  }

  public updateHero(hero: Hero) {
    this.heroService
      .updateHero(hero)
      .subscribe({
        next: (heroes: Hero[]) => this.heroesUpdated.emit(heroes),
        error: err => console.log(err)
      })
  }
  public deleteHero(hero: Hero) {
    this.heroService
      .deleteHero(hero)
      .subscribe({
        next: (heroes: Hero[]) => this.heroesUpdated.emit(heroes),
        error: err => console.log(err)
      })
  }
  public createHero(hero: Hero) {
    this.heroService
      .createHero(hero)
      .subscribe({
        next: (heroes: Hero[]) => this.heroesUpdated.emit(heroes),
        error: err => console.log(err)
      })
  }
}

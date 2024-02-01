import { Component, Input, OnInit, AfterViewInit } from '@angular/core';
import { Restoran } from 'src/app/models/restoran';
import { LeafletMap } from './leaflet-map';

@Component({
  selector: 'app-lokal-mapa-lokacija',
  templateUrl: './lokal-mapa-lokacija.component.html',
  styleUrls: ['./lokal-mapa-lokacija.component.scss'],
})
export class LokalMapaLokacijaComponent implements OnInit, AfterViewInit {
  @Input() lokal: Restoran | null = null;
  map: LeafletMap | null = null;

  constructor() {}
  
  ngOnInit(): void {
    //console.log(this.lokal);
  }

  ngAfterViewInit(): void {
    if(this.lokal)
    this.map = new LeafletMap('map', this.lokal.mapaY, this.lokal.mapaX, this.lokal.naziv);
  }

}

import { galerija } from "./galerija";
import { Korisnik } from "./korisnik";
import { Moderator } from "./moderator";
import { Sto } from "./sto";

export class Restoran
{
     id: number;
     naziv: string;
     ocena: number;
     adresa: string;
     telefon: string;
     dogadjaj: string;
     mapaLokacija: string;
     ukupanKapacitet: number;
     radnoVreme: string;
     mapaX: number;
     mapaY: number;

     stos: Sto[];
     galerijas: galerija[];


     constructor(id: number, naziv: string, adresa: string, telefon: string, radnoVreme: string)
     {
          this.id = id;
          this.naziv = naziv;
          this.adresa = adresa;
          this.ocena = 0;         
          this.telefon = telefon;
          this.dogadjaj = " ";
          this.mapaLokacija = " ";
          this.ukupanKapacitet = 0;
          this.radnoVreme = radnoVreme;

          this.stos = [];
          this.galerijas = [];
          this.mapaX = 0;
          this.mapaY = 0;
     }
}
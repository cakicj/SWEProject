import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { Moderator } from '../models/moderator';
import { Restoran } from '../models/restoran';
import { AppState } from '../store/app-state';
import { ModeratorService } from './moderator/moderator.service';
import { Store } from '@ngrx/store';
import * as LokalActions from 'src/app/store/lokal/lokal.actions';
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
  HttpResponse,
} from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { environment } from 'src/environments/environment';
import { testModel } from '../models/testModel';
import { galerija } from '../models/galerija';
import { Sto } from '../models/sto';
import * as KorisnikSelector from 'src/app/store/korisnik/korisnik.selectors';
import { Subscription } from 'rxjs';
import { Korisnik } from '../models/korisnik';
import { KorisnikService } from './korisnik/korisnik.service';

@Injectable({
  providedIn: 'root',
})
export class RestoraniService {
  currentSubscription: Subscription | null = null;
  currentKorisnik: Korisnik | null = null;
  private httpHeaderUser: HttpHeaders | null = null;

  constructor(
    private httpClient: HttpClient,
    private _korisnikService: KorisnikService,
    private store: Store<AppState>
  ) {}

  getAll() {
    // console.log(
    //   'Korisnik u trenutnku slanja zahteva za kafice: ' +
    //     this._korisnikService.getUlogovanKorisnik().token
    // );

    // let httpHeaders: HttpHeaders = new HttpHeaders({
    //   Authorization:
    //     'Bearer ' +
    //     'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidnVrYXNpbiIsImp0aSI6IjFlMDIxN2U1LTNmM2MtNDgwYy1hZDZkLWNmYjg5NWU2NTRhYSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlByaWphdmxqZW5pIiwiZXhwIjoxNjI1NjkxMzUxLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjYzMzgzIiwiYXVkIjoiVXNlciJ9.kOhR0ZDJf5xq1Ng_C6Y7m8I15NWezEquXxuKzhmf9jY',
    // });

    //return this.httpClient.get<Restoran[]>(environment.apiUrl + "lokali").pipe(catchError(errorHandler));
    // this.httpClient
    //   .get<Restoran[]>(environment.apiUrl + 'VratiKafice', {
    //     headers: httpHeaders,
    //   })
    //   .pipe(catchError(errorHandler))
    //   .subscribe((x) => console.log(x));
    
    return this.httpClient
      .get<Restoran[]>(environment.apiUrl + 'VratiKafice')
      .pipe(catchError(errorHandler));
  }

  sendLokal(lokal: Restoran) {
    //return this.httpClient.post<Restoran>(environment.apiUrl + "lokali", lokal).pipe(catchError(errorHandler));
    console.log(this.setAuthorizationHeader());
    return this.httpClient
      .post<Restoran>(environment.apiUrl + 'DodajKafic', lokal, {
        headers: this.setAuthorizationHeader(),
      })
      .pipe(catchError(errorHandler));
  }

  setAuthorizationHeader() {

    return new HttpHeaders({
      Authorization:
        'Bearer ' +
        this._korisnikService.getUlogovanKorisnik().token,
    });
  }

  dodajSlikuLokalu(slika: galerija) {
    //return this.httpClient.put<Restoran>(environment.apiUrl + "lokali", lokal).pipe(catchError(errorHandler));
    console.log("Poslata slika u bazu " + slika);
    return this.httpClient
      .post<Restoran>(environment.apiUrl + 'DodajGaleriju', slika, {
        headers: this.setAuthorizationHeader(),
      })
      .pipe(catchError(errorHandler));
  }

  deleteSlika(slikaId: number) {
    //return this.httpClient.delete<Restoran>()
    console.log(slikaId);
    return this.httpClient
      .delete<Restoran>(environment.apiUrl + 'ObrisiGaleriju/' + slikaId, {
        headers: this.setAuthorizationHeader(),
      })
      .pipe(catchError(errorHandler));
  }

  deleteLokal(lokalId: number) {
    //return this.httpClient.delete<Restoran>()
    return this.httpClient
      .delete<Restoran>(environment.apiUrl + 'ObrisiKafic/' + lokalId, {
        headers: this.setAuthorizationHeader(),
      })
      .pipe(catchError(errorHandler));
  }

  dodajSto(sto: Sto) {
    console.log(sto);
    return this.httpClient
      .post<Restoran>(environment.apiUrl + 'DodajSto', sto, {
        headers: this.setAuthorizationHeader(),
      })
      .pipe(catchError(errorHandler));
  }

  deleteSto(sto: Sto) {
    return this.httpClient
      .delete<Restoran>(environment.apiUrl + 'ObrisiSto/' + sto.id, {
        headers: this.setAuthorizationHeader(),
      })
      .pipe(catchError(errorHandler));
  }

  rezervisiSto(sto: Sto, brOsoba: number, lokalId: number, korisnikId: number) {
    return this.httpClient
      .put<Restoran>(
        environment.apiUrl +
          'DodajRezervaciju/' +
          korisnikId +
          '/' +
          brOsoba +
          '/' +
          lokalId,
        sto, {
          headers: this.setAuthorizationHeader(),
        }
      )
      .pipe(catchError(errorHandler));
  }

  oceni(idLokala: number, ocena: number, lokal: Restoran, korisnikId: number) {
    return this.httpClient
      .post<Restoran>(
        environment.apiUrl +
          'DodajOcenu/' +
          idLokala +
          '/' +
          korisnikId +
          '/' +
          ocena,
        lokal, {
          headers: this.setAuthorizationHeader(),
        }
      )
      .pipe(catchError(errorHandler));
  }

  dodavanjeMape(mapaUrl: string, idLokala: number, lokal: Restoran) {
    console.log(mapaUrl);
    return this.httpClient
      .put<Restoran>(
        environment.apiUrl + 'IzmeniMapuKafica/' + idLokala + '/' + mapaUrl,
        lokal, {
          headers: this.setAuthorizationHeader(),
        }
      )
      .pipe(catchError(errorHandler));
  }

  otkaziRezervaciju(idStola: number, sto: Sto) {
    console.log(idStola);
    return this.httpClient
      .put<Restoran>(environment.apiUrl + 'UkloniRezervaciju/' + idStola, sto, {
        headers: this.setAuthorizationHeader(),
      })
      .pipe(catchError(errorHandler));
  }

  dodajDogadjaj(dogadjaj: string, idLokala: number, lokal: Restoran) {
    return this.httpClient
      .put<Restoran>(
        environment.apiUrl +
          'IzmeniDogadjajKafica/' +
          idLokala +
          '/' +
          dogadjaj,
        lokal, {
          headers: this.setAuthorizationHeader(),
        }
      )
      .pipe(catchError(errorHandler));
  }

  izmeniLokalMapa(idLokala: number, lokal: Restoran) {
    console.log(lokal);
    return this.httpClient
      .put<Restoran>(environment.apiUrl + 'IzmeniKafic/' + idLokala, lokal, {
        headers: this.setAuthorizationHeader(),
      })
      .pipe(catchError(errorHandler));
  }
}

const errorHandler = (error: HttpErrorResponse) => {
  alert(error.error);
  const errorMessage =
    error.status === 0
      ? `Can't connect to API ${error.error}`
      : `Backend returned code ${error.status}`;

  return throwError(errorMessage);
};

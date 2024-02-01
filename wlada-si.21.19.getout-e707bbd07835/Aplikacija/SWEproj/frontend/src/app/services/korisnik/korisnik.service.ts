import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
  HttpResponse,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Korisnik } from 'src/app/models/korisnik';
import * as KorisnikActions from 'src/app/store/korisnik/korisnik.actions';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class KorisnikService {
  constructor(private httpClient: HttpClient, private store: Store<Korisnik>) {}

  setAuthorizationHeader() {
    return new HttpHeaders({
      Authorization: 'Bearer ' + this.getUlogovanKorisnik().token,
    });
  }

  getAll() {
    return this.httpClient
      .get<Korisnik[]>(environment.apiUrl + 'VratiKorisnike', {
        headers: this.setAuthorizationHeader(),
      })
      .pipe(catchError(errorHandler));
  }

  deleteLokal(korisnikId: number) {
    console.log(korisnikId);
    return this.httpClient
      .delete<Korisnik>(environment.apiUrl + 'ObrisiKorisnika/' + korisnikId, {
        headers: this.setAuthorizationHeader(),
      })
      .pipe(catchError(errorHandler));
  }

  dodaKorisnika(noviKorisnik: Korisnik) {
    return this.httpClient
      .post<Korisnik>(environment.apiUrl + 'Register', noviKorisnik)
      .pipe(catchError(errorHandler));
  }

  izmeniKorisnika(korisnik: Korisnik) {
    console.log(korisnik);
    return this.httpClient
      .put<Korisnik>(
        environment.apiUrl + 'IzmeniKorisnika/' + korisnik.id,
        korisnik,
        {
          headers: this.setAuthorizationHeader(),
        }
      )
      .pipe(catchError(errorHandler));
  }

  unaprediKorisnika(korisnik: Korisnik) {
    return this.httpClient
      .post<Korisnik>(
        environment.apiUrl + 'Unapredi/' + korisnik.username,
        korisnik,
        {
          headers: this.setAuthorizationHeader(),
        }
      )
      .pipe(catchError(errorHandler));
  }

  getUlogovanKorisnik() {
    let sessionStorage1 = JSON.parse(
      <string>sessionStorage.getItem('korisnik')
    );
    return sessionStorage1 == null ? null : sessionStorage1.korisnik;
  }

  setUlogovanKorisnik(saveKorisnik: Korisnik) {
    sessionStorage.setItem(
      'korisnik',
      JSON.stringify({ korisnik: saveKorisnik })
    );
    // httpHeaders: HttpHeaders = new HttpHeaders({
    //   Authorization:
    //     'Bearer ' +
    //     'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidnVrYXNpbiIsImp0aSI6IjFlMDIxN2U1LTNmM2MtNDgwYy1hZDZkLWNmYjg5NWU2NTRhYSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlByaWphdmxqZW5pIiwiZXhwIjoxNjI1NjkxMzUxLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjYzMzgzIiwiYXVkIjoiVXNlciJ9.kOhR0ZDJf5xq1Ng_C6Y7m8I15NWezEquXxuKzhmf9jY',
    // });
  }

  izlogujKorisnika() {
    sessionStorage.clear();
  }

  korisnikLogin(korisnik: Korisnik) {
    return this.httpClient
      .post<Korisnik>(environment.apiUrl + 'Login', korisnik)
      .pipe(catchError(errorHandler));
  }

  korisnikLogOut() {
    let tempKorisnik = this.getUlogovanKorisnik();
    sessionStorage.clear();
    return this.httpClient
      .put<Korisnik>(
        environment.apiUrl + 'Izloguj/' + tempKorisnik.id,
        tempKorisnik
      )
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

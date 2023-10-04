import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { TRIPPIN_BASE_URL } from './app.config';
import { Person, Trip } from './models/TrippinModel';
import { Observable, catchError, firstValueFrom, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TrippinService {
  constructor(private httpClient: HttpClient, @Inject(TRIPPIN_BASE_URL) private baseUrl: string) {}

  private handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      console.error(`Backend returned code ${error.status}, body was: `, error.error);
    }

    // Return an observable with a user-facing error message.
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }

  public getPeople(nameFilter?: string): Observable<{ value: Person[] }> {
    let params = new HttpParams();
    if (nameFilter) {
      params = params.set(
        '$filter',
        `contains(FirstName, '${nameFilter}') or contains(LastName, '${nameFilter}') or (MiddleName ne null and contains(MiddleName, '${nameFilter}'))`
      );
    }
    params = params.set('$select', 'UserName,FirstName,MiddleName,LastName,Age');
    params = params.set('$orderby', 'LastName,FirstName');

    return this.httpClient
      .get<{ value: Person[] }>(`${this.baseUrl}/People`, { params })
      .pipe(catchError(this.handleError));
  }

  public getTrips(userName: string): Observable<{ value: Trip[] }> {
    let params = new HttpParams()
      .set('$select', 'TripId,Name,Budget,Description,Tags,StartsAt,EndsAt')
      .set('$orderby', 'EndsAt desc');

    const url = `${this.baseUrl}/People('${userName}')/Trips`;
    return this.httpClient
      .get<{ value: Trip[] }>(url, { params })
      .pipe(catchError(this.handleError));
  }
}

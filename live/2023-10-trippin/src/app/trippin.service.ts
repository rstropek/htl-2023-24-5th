import { Inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { TRIPPIN_BASE_URL } from './app.config';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Person, Trip } from './models/TrippinModel';

export interface ODataResponse<T> {
  value: T[];
}

@Injectable({
  providedIn: 'root',
})
export class TrippinService {
  constructor(
    private httpClient: HttpClient,
    @Inject(TRIPPIN_BASE_URL) private baseUrl: string
  ) {}

  public getPeople(nameFilter?: string): Observable<ODataResponse<Person>> {
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
      .get<{ value: Person[] }>(`${this.baseUrl}/People`, { params });
  }

  public getTrips(userName: string): Observable<{ value: Trip[] }> {
    let params = new HttpParams()
      .set('$select', 'TripId,Name,Budget,Description,Tags,StartsAt,EndsAt')
      .set('$orderby', 'EndsAt desc');

    const url = `${this.baseUrl}/People('${userName}')/Trips`;
    return this.httpClient
      .get<{ value: Trip[] }>(url, { params });
  }

  public addTrip(userName: string, trip: Trip): Observable<Trip> {
    const url = `${this.baseUrl}/People('${userName}')/Trips`;
    return this.httpClient.post<Trip>(url, trip);
  }
}

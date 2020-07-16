import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SessiondataService {

  private baseUrl = 'https://localhost:44321/Sessions';

  constructor(private http: HttpClient) { }

  public create(data) {
    const client = new XMLHttpRequest();
    client.open("POST", this.baseUrl);
    client.setRequestHeader('Content-Type', 'application/json');
    client.send(JSON.stringify(data));
  }
}

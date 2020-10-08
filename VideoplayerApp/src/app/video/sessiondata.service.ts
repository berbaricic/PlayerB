import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SessiondataService {

  private baseUrl = 'http://localhost:5000/';

  constructor(private http: HttpClient) { }

  public create(data) {
    const client = new XMLHttpRequest();
    client.open("POST", this.baseUrl + "Sessions");
    client.setRequestHeader('Content-Type', 'application/json');
    client.send(JSON.stringify(data));
  }

  public createOnVideoPlay(data) {
    const client = new XMLHttpRequest();
    client.open("POST", this.baseUrl + "Video");
    client.setRequestHeader('Content-Type', 'application/json');
    client.send(JSON.stringify(data));
  }
}

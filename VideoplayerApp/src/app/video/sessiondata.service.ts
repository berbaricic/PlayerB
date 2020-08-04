import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SessiondataService {

  private baseUrl = 'https://localhost:5000/Sessions';

  constructor(private http: HttpClient) { }

  public create(data) {
    const client = new XMLHttpRequest();
    client.open("POST", this.baseUrl);
    client.setRequestHeader('Content-Type', 'application/json');
    client.send(JSON.stringify(data));
  }

  public update(data, id) {
    const client = new XMLHttpRequest();
    client.open("PUT", this.baseUrl + "/" + id);
    client.setRequestHeader('Content-Type', 'application/json');
    client.send(JSON.stringify(data));
  }

  public read(id) {
    const client = new XMLHttpRequest();
    client.open("GET", this.baseUrl + "/" + id);
    client.setRequestHeader('Content-Type', 'application/json');
    client.send(null);
    console.log(client.responseText);
  }
}

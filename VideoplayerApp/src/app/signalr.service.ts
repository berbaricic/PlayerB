import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {

  private hubConnection: signalR.HubConnection
  public rowsNumber: any;

  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5000/signalr', {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();
 
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err))
  }

  public addNumberOfRowsToHeader = () => {
    this.hubConnection.on('ReceiveMessage', (rowsNumber) => {
      this.rowsNumber = rowsNumber;
      console.log("from signalr servisa: " + this.rowsNumber);
    });
  }
}

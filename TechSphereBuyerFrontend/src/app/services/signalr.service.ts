import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  constructor() {}
  hubConnectionBuilder!: HubConnection;
  url: string = 'http://localhost/TopShopBuyer/signalr';
  makeConnection(): HubConnection {
    // if(JSON.parse(localStorage.getItem('User')).Username == "ahmed")
    // {
    //   this.url = 'http://localhost/TopShopBuyer2/signalr';
    // }

    this.hubConnectionBuilder = new HubConnectionBuilder()
      .withUrl(this.url, {
        accessTokenFactory: () =>
          JSON.parse(localStorage.getItem('User')).Token,
        withCredentials: false,
      })
      .withAutomaticReconnect()
      .build();
    this.hubConnectionBuilder
      .start()
      .then(() => console.log('Connection started.......!'))
      .catch((err) => console.log('Error while connect with server'));
    return this.hubConnectionBuilder;
  }

  async requesttoServer(action: string, products:number[]) {
    switch (action) {
      case 'checkout': {
        await this.hubConnectionBuilder
          .invoke('OnCheckout', products)
          .then((message) => console.log(message));
        return;
      }
      default:
        break;
    }
  }

  disconnect() {
    this.hubConnectionBuilder.stop();
  }
}

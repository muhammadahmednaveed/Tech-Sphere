import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  constructor() {}
  hubConnectionBuilder!: HubConnection;

  makeConnection(): HubConnection {
    this.hubConnectionBuilder = new HubConnectionBuilder()
      .withUrl('http://localhost/TopShopBuyer/signalr')
      .build();
    this.hubConnectionBuilder
      .start()
      .then(() => console.log('Connection started.......!'))
      .catch((err) => console.log('Error while connect with server'));
    return this.hubConnectionBuilder;
  }

  async requesttoServer(action: string, data: any) {
    switch (action) {
      case 'checkout': {
        await this.hubConnectionBuilder
          .invoke('onCheckout', data)
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

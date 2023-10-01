import { Component, OnDestroy, OnInit } from '@angular/core';
import { SignalrService } from '../services/signalr.service';
import { HubConnection } from '@microsoft/signalr';

@Component({
  selector: 'app-buyer-ui',
  templateUrl: './buyer-ui.component.html',
  styleUrls: ['./buyer-ui.component.scss'],
})
export class BuyerUIComponent implements OnInit, OnDestroy {
  constructor(private signalr: SignalrService) {}

  ngOnInit(): void {
    this.HubConnection = this.signalr.makeConnection();
    this.HubConnection.on('OnCheckout', (message: string) => {
      this.messages.push(message);
      setTimeout(() => {
        this.messages = this.messages.filter((m) => m !== message);
      }, 5000);
    });
  }

  HubConnection!: HubConnection;
  messages: string[] = [];
  ngOnDestroy(): void {
    this.signalr.disconnect();
  }
}

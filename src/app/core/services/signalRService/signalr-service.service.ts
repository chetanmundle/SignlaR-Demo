import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { BASE_URL_HUB } from '../../../../environments/environments';
import { BehaviorSubject, Subject } from 'rxjs';
import { ReciveMessageDto } from '../../models/ChatHubDtos/chatHub.model';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private hubConnection?: HubConnection;
  private connected$ = new BehaviorSubject<boolean>(false);

  private message$ = new Subject<ReciveMessageDto | null>();
  public messages$ = this.message$.asObservable();


  constructor(

  ) { }

  public async start(): Promise<void> {
    const access_token = localStorage.getItem("accessToken");
    if (!access_token) {
      return
    };

    if (this.hubConnection) {
      if (this.hubConnection.state === 'Connected') return;
      // if not connected, try start again
    }

    // Connection with SignalR
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${BASE_URL_HUB}/chatHub`, {
        accessTokenFactory: () => access_token ?? ""
      })
      .withAutomaticReconnect() // default reconnect delays (0,2s,10s,30s)
      .configureLogging(LogLevel.Information)
      .build();


    // lisnen the message 
    this.hubConnection.on('ReceiveConversationGroupMessage', (msg: ReciveMessageDto) => {
      console.log("Message received in service:", msg);
      this.message$.next(msg);
    });

    try {
      await this.hubConnection.start();
      console.log('[ SignalR ]SignalR connected');
      this.connected$.next(true);
    } catch (err) {
      console.error('SignalR failed to start', err);
      this.connected$.next(false);
      // Try again after a delay (exponential backoff is better for production)
      setTimeout(() => this.start(), 5000);
    }
  }

  // ✅ Join conversation (calls Hub method)
  public async joinConversation(conversationId: number): Promise<void> {
    if (!this.hubConnection) throw new Error('Not connected');
    return this.hubConnection.invoke('JoinConversation', conversationId).then(res => {
      console.log("[signalR ] Join the conversationID :", conversationId);
    }).catch((err) => {
      console.error("[SignalR] Faild to join the conversation ");

    })
  }

  // ✅ Leave conversation (calls Hub method)
  public async leaveConversation(conversationId: number): Promise<void> {
    if (!this.hubConnection) throw new Error('Not connected');
    return this.hubConnection.invoke('LeaveGroup', conversationId);
  }

  // ✅ Send message to conversation (calls Hub method)
  public async sendMessageToConversation(conversationId: number, message: string, userId: number): Promise<void> {
    if (!this.hubConnection) throw new Error('Not connected');
    return this.hubConnection.invoke('SendMessageToConversation', conversationId, message, userId)
      .then(() => {
        // console.log(`[SignalR] Message sent to conversation ${conversationId}`);
      })
      .catch(err => {
        console.error('[SignalR] Failed to send message:', err);
      });
  }

}

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, from, Observable, of } from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';
import { AuthService } from '../../layout/services/auth.service';

export interface ChatMessage {
  text: string;
  senderIdentifier: string;
  dateTime: Date;
}

export interface Room {
  id: string;
  participantOne: UniqueIdentifier;
  participantTwo: UniqueIdentifier;
  messages: ChatMessage[]
}

export class UniqueIdentifier {
  userId: string = '';
  uniqueIdentifier: string = '';
  givenName: string = '';
  surname: string = '';
}

export interface RoomCreateModel {
  participantOneUI: string;
  participantTwoUI: string;
}

const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type':  'application/json'
  })
};

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  
  private hubConnection!: HubConnection
  private connectionUrl = 'https://localhost:6001/chat';
  private apiUrl = 'https://localhost:6001/api/chat';

  constructor(private http: HttpClient, private authService: AuthService) { }

  private _identifier!: UniqueIdentifier;
  public get identifier(): UniqueIdentifier {
    return this._identifier;
  }
  public setIdentifier(value: UniqueIdentifier) {
    this._identifier = value;
  }

  public rooms: Room[] = [];
  private roomsSubject = new BehaviorSubject<Room[]>(this.rooms);
  public rooms$ = this.roomsSubject.asObservable();

  public selectedRoom!: Room;
  
  private messagesSubject = new BehaviorSubject<ChatMessage[] | undefined>(undefined);
  public roomMessages = this.messagesSubject.asObservable();

  public setRoom(room: Room) {
    this.selectedRoom = room;
    this.messagesSubject.next(this.selectedRoom.messages)
  }

  getOtherChatParticipantUUID() {
    return this.selectedRoom.participantOne.uniqueIdentifier === this.identifier.uniqueIdentifier ?
      this.selectedRoom.participantTwo : this.selectedRoom.participantOne
  }

  // API
  fetchUUID(userId: string, isCurrentUser = false) {
    let params = new HttpParams().set("userId", userId);
    return this.http.get<UniqueIdentifier>(this.apiUrl, { params: params })
      .pipe(tap({
        next: (uuid) => {
          if (uuid && isCurrentUser) {
            this.setIdentifier(uuid!)
          }
      } }))
  }

  getUuids(userId: string): Observable<UniqueIdentifier[]> {
    let params = new HttpParams().set("userId", userId);
    return this.http.get<UniqueIdentifier[]>(`${this.apiUrl}/uuids`, { params: params });
  }

  generateUUID(userId: string) {
    return this.http.post<string>(this.apiUrl, { userId: userId })
  }

  fetchRooms() {
    return this._fetchRooms();
  }

  fetchRoom(roomId: string) {
    const uri = `${this.apiUrl}/rooms/${roomId}`;
    return this.http.get<Room>(uri)
      .pipe(map(room => {
        const hasRoom = this.rooms.findIndex(r => r.id == room.id)
        if (!hasRoom) {
          this.rooms.push(room);
          this.roomsSubject.next(this.rooms)
        }

        return room;
      }))
  }

  createAndFetchRoom(participantUUIDToInclude: string) {
    const uri = `${this.apiUrl}/rooms`;
    const body: RoomCreateModel = {
      participantOneUI: this.identifier.uniqueIdentifier,
      participantTwoUI: participantUUIDToInclude
    }
    return this.http.post<Room>(uri, body, httpOptions)
      .pipe(tap({
        next: (room) => {
          const hasRoom = this.rooms.findIndex(r => r.id == room.id)
          if (!hasRoom) {
            this.rooms.push(room);
            this.roomsSubject.next(this.rooms)
          }
      }}))
  }

  private _fetchRooms() {
    const uri = `${this.apiUrl}/rooms/participants/${this.identifier.uniqueIdentifier}`;
    return this.http.get<Room[]>(uri)
      .pipe(map(rooms => {
        this.rooms = rooms;
        this.roomsSubject.next(this.rooms)
        return rooms;
      }))
  }


  public connect() {
    return this.startConnection()
      .pipe(map(_ => {
        this.addListeners();
        return _;
      }))
  }

  public sendMessageToApi(message: string) {
    const uri = `${this.apiUrl}/rooms/${this.selectedRoom.id}/messages`;
    return this.http.post(uri, this.buildChatMessage(message))
      .pipe(tap(message => {
        
        console.log("message sucessfully sent to api controller")
      }));
  }

  public sendMessageToHub(message: string) {
    var promise = this.hubConnection.invoke("BroadcastAsync", this.buildChatMessage(message))
      .then(() => { console.log('message sent successfully to hub'); })
      .catch((err) => console.log('error while sending a message to hub: ' + err));

    return from(promise);
  }
  

  private buildChatMessage(message: string): ChatMessage {
    return {
      senderIdentifier: this._identifier.uniqueIdentifier,
      text: message,
      dateTime: new Date()
    };
  }

  private startConnection() {
    this.hubConnection = this.getConnection();
    return from(this.hubConnection.start());
  }

  private getConnection(): HubConnection {
    return new HubConnectionBuilder()
      .withUrl(this.connectionUrl, {
        withCredentials: false, accessTokenFactory: () => this.authService.user?.access_token!})
      .build();
  }

  private addListeners() {
    this.hubConnection.on("messageReceivedFromApi", (data: MessageSaved) => {
      console.log("message received from API Controller")
      const message: ChatMessage = { senderIdentifier: data.senderIdentifier, dateTime: data.dateTime, text: data.text} 
      const roomIndex = this.rooms.findIndex(r => r.id == data.roomId);
      this.rooms[roomIndex].messages.push(message)

      if (this.selectedRoom && this.selectedRoom.id === data.roomId) {
        this.messagesSubject.next(this.rooms[roomIndex].messages);
      }
    })
    this.hubConnection.on("messageReceivedFromHub", (data: MessageSaved) => {
      console.log("message received from Hub")
      const message: ChatMessage = { senderIdentifier: data.senderIdentifier, dateTime: data.dateTime, text: data.text} 
      const roomIndex = this.rooms.findIndex(r => r.id == data.roomId);
      this.rooms[roomIndex].messages.push(message)
    })
    this.hubConnection.on("newUserConnected", _ => {
      console.log("new user connected")
    })
  }
}

export interface MessageSaved {
  roomId: string;
  senderIdentifier: string;
  text: string;
  dateTime: Date
}

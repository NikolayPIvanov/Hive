import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { switchMap, takeUntil, tap } from 'rxjs/operators';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { ChatMessage, ChatService, Room, UniqueIdentifier } from '../../services/chat.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit, OnDestroy {
  private unsubscribe = new Subject();


  public text: string = '';
  public otherParticipantName: string = ''

  private roomsSubject = new BehaviorSubject<Room[] | undefined>(undefined);
  public rooms$ = this.roomsSubject.asObservable();
  private rooms: Room[] = [];
  uuids: UniqueIdentifier[] = [];
  messages$!: Observable<ChatMessage[] | undefined>;

  constructor(
    public chatService: ChatService,
    private spinner: NgxSpinnerService,
    private authService: AuthService) { }
  
  ngOnDestroy(): void {
    //Called once, before the instance is destroyed.
    //Add 'implements OnDestroy' to the class.
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }
 
  ngOnInit(): void {
    this.spinner.show();

    this.messages$ = this.chatService.fetchUUID(this.authService.user?.profile.sub!, true)
      .pipe(
        // takeUntil(this.unsubscribe),
        switchMap(() => this.chatService.fetchRooms()),
        tap({
          next: (rooms) => {
            if (rooms && rooms.length > 0) {
              this.chatService.setRoom(rooms[0])
              this.otherParticipantName = this.getChattingWithName(rooms[0]);
              this.rooms = rooms;
              this.roomsSubject.next(rooms);
            }
          }
        }),
        switchMap(rooms => this.chatService.getUuids(this.authService.user?.profile.sub!)),
        tap({
          next: (uuids) => {
            const otherUuids = uuids.filter(uuid => {
              return this.rooms.findIndex(room => {
                return uuid.uniqueIdentifier === room.participantOne.uniqueIdentifier ||
                uuid.uniqueIdentifier === room.participantTwo.uniqueIdentifier
              }) === -1
            })

            this.uuids = otherUuids
          }
        }),
        switchMap(() => this.chatService.roomMessages),
        tap({
          complete: () => {
            this.spinner.hide();
          }
        })
      )
  }

  getChattingWithName(room: Room) {
    const participantOneUuid = room.participantOne.uniqueIdentifier;

    if (participantOneUuid === this.chatService.identifier.uniqueIdentifier) {
      return `${room.participantTwo.givenName} ${room.participantTwo.surname}`
    }
    return `${room.participantOne.givenName} ${room.participantOne.surname}`
  }

  onRoomSelected($event: any) {
    this.chatService.setRoom($event.option.value)
    this.otherParticipantName = this.getChattingWithName($event.option.value)
    this.chatService.roomMessages.subscribe()
  }

  isReceiving(senderIdentifier: string) {
    return senderIdentifier != this.chatService.identifier.uniqueIdentifier;
  }

  selected($event: any) {
    if (!$event.value.uniqueIdentifier) {
      return;
    }

    const index = (Array.from(this.chatService.rooms)).findIndex(r => 
      (r.participantOne.uniqueIdentifier == this.chatService.identifier.uniqueIdentifier
        && r.participantTwo == $event.value.uniqueIdentifier)
      ||
      (r.participantTwo.uniqueIdentifier == this.chatService.identifier.uniqueIdentifier
        && r.participantOne == $event.value.uniqueIdentifier)
    )
    if (index > -1) {
      const room = Array.from(this.chatService.rooms)[index];
      this.otherParticipantName = this.getChattingWithName(room);
        this.chatService.setRoom(room)
        this.chatService.roomMessages.subscribe()
    } else {
      const data = $event.value.uniqueIdentifier as string;
        this.chatService.createAndFetchRoom(data)
          .subscribe(room => {
            this.rooms.push(room);
            this.otherParticipantName = this.getChattingWithName(room);
            this.roomsSubject.next(this.rooms);
            this.chatService.setRoom(room)
            this.chatService.roomMessages.subscribe();
        });
    }
  }
  
  sendMessage(): void {
    this.chatService.sendMessageToApi(this.text)
      .pipe(tap({ next: () => this.text = ''}))
      .subscribe();
  }

}

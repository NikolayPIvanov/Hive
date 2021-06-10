import { HttpClient, HttpParams } from '@angular/common/http';
import { AfterViewChecked, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { profile } from 'console';
import { concat, forkJoin, from, Observable, of, zip } from 'rxjs';
import { catchError, concatMap, map, mergeMap, switchMap, tap, toArray } from 'rxjs/operators';
import { ProfileClient, UserProfileDto } from 'src/app/clients/profile-client';
import { AuthService } from 'src/app/modules/layout/services/auth.service';
import { ChatMessage, ChatService, Room, UniqueIdentifier } from '../../services/chat.service';

export interface ProfileUnifier {
  uuid: string;
  profile: UserProfileDto;
}

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit {
  public text: string = '';
  public uuids$!: Observable<ProfileUnifier[]>;

  public otherParticipantName: string = ''

  constructor(
    public chatService: ChatService,
    private authService: AuthService,
    private profileClient: ProfileClient) { }
  
  // profiles!: UserProfileDto[];
  // private roomId: string | undefined = undefined;

  public unifiers: ProfileUnifier[] = []
  source!: Observable<ProfileUnifier[] | undefined>;
  

  getOtherChatPersonProfile() {
    const uuid = this.chatService.getOtherChatParticipantUUID();
    this.otherParticipantName = this.getNameByUUID(uuid);
  }

  getName(identifierA: string, identifierB: string) {
    const elementA = this.unifiers.find(x => x.uuid === identifierA);
    const elementB = this.unifiers.find(x => x.uuid === identifierB);
    if (elementA && (elementA.profile.firstName || elementA.profile.lastName)) {
      return `${elementA.profile.firstName} ${elementA.profile.lastName}`;

    }
    else if (elementB && (elementB.profile.firstName || elementB.profile.lastName)) {
      return `${elementB.profile.firstName} ${elementB.profile.lastName}`;
    }

    return 'Name not provided';
  }

  getNameByUUID(identifier: string) {
    const element = this.unifiers.find(x => x.uuid === identifier);
    if (element && (element.profile.firstName || element.profile.lastName)) {
      const name = `${element.profile.firstName} ${element.profile.lastName}`;
      return name.trim() === '' ? 'User' : name;
    }

    return 'Name not provided';
  }

  ngOnInit(): void {
    this.source = this.chatService.fetchUUID(this.authService.user?.profile.sub!, true)
      .pipe(
        switchMap(uuid => this.chatService.fetchRooms()
          .pipe(
            tap({
              next: (rooms) => {
                if (rooms && rooms.length > 0) {
                  this.chatService.setRoom(rooms[0])
                  this.chatService.roomMessages.subscribe()
                }
              }
            }))
        ),
        switchMap(rooms => {
          return this.profileClient.getProfiles()
            .pipe(concatMap(profiles => {
              const uuids = profiles.map(profile => this.chatService.fetchUUID(profile.userId!))
              return forkJoin(uuids).pipe(map((uuidsResult) => {
                let result: ProfileUnifier[] = []
                uuidsResult.forEach(unique => {
                  const profile = profiles.find(p => p.userId == unique.userId)!;
                  const unifier = { uuid: unique?.uniqueIdentifier, profile: profile }
                  if (unifier.uuid !== this.chatService.identifier.uniqueIdentifier) {
                    result.push(unifier);
                  }
                })

                this.unifiers = result;
                this.getOtherChatPersonProfile()

                return result;
              }))
            }))
        })
      );
  }

  onRoomSelected($event: any) {
    this.chatService.setRoom($event.option.value)
    this.chatService.roomMessages.subscribe()
    this.getOtherChatPersonProfile()
  }

  isReceiving(senderIdentifier: string) {
    return senderIdentifier != this.chatService.identifier.uniqueIdentifier;
  }

  selected($event: any) {
    const index = (Array.from(this.chatService.rooms)).findIndex(r => 
      (r.participantOne == this.chatService.identifier.uniqueIdentifier
      && r.participantTwo == $event.value.uuid)
      ||
      (r.participantTwo == this.chatService.identifier.uniqueIdentifier
        && r.participantOne == $event.value.uuid)
    )
    if (index > -1) {
        const room = Array.from(this.chatService.rooms)[index];
        this.chatService.setRoom(room)
        this.getOtherChatPersonProfile()
        this.chatService.roomMessages.subscribe()
    } else {
      const data = $event.value.uuid as string;
        this.chatService.createAndFetchRoom(data)
        .subscribe(room => {
          this.chatService.setRoom(room)
          this.getOtherChatPersonProfile()
          this.chatService.roomMessages.subscribe()
        });
    }
  }
  

  sendMessage(): void {
    this.chatService.sendMessageToApi(this.text)
      .pipe(tap({ next: () => this.text = ''}))
      .subscribe();
  }


}

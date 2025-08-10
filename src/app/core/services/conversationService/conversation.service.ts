import { Injectable } from '@angular/core';
import { CommonService } from '../commonService/common-service.service';
import { CreateConversationRequest, ConversationDto } from '../../models/conversationModels/conversation.model';
import { Observable } from 'rxjs';
import { AppResponse } from '../../models/genericResponse.model';

@Injectable({
  providedIn: 'root'
})
export class ConversationService {

  constructor(
    private _commonService: CommonService
  ) { }

  CreateConversation$(payload: CreateConversationRequest): Observable<AppResponse<null>> {
    return this._commonService.post<AppResponse<null>>(
      "Conversation/create-conversation",
      payload
    )
  }

  GetMyConversation$(userId: number): Observable<AppResponse<ConversationDto[]>> {
    return this._commonService.get<AppResponse<ConversationDto[]>>(
      `Conversation/${userId}/get-My-conversation`
    )
  }
}

import { Injectable } from '@angular/core';
import { CommonService } from '../commonService/common-service.service';
import { Observable } from 'rxjs';
import { AppResponse } from '../../models/genericResponse.model';
import { GetMessagesResponseDto } from '../../models/messageDtos/messageDtos.model';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(
    private _commonService :CommonService
  ) { }

  GetMessagesByConversationId$(conversationId : number) : Observable<AppResponse<GetMessagesResponseDto[]>>{
    return this._commonService.get<AppResponse<GetMessagesResponseDto[]>>(
      `message/conversationId/${conversationId}/getMessages`
    )
  }

}

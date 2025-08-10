import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SideConversationsComponent } from './side-conversations.component';

describe('SideConversationsComponent', () => {
  let component: SideConversationsComponent;
  let fixture: ComponentFixture<SideConversationsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SideConversationsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SideConversationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

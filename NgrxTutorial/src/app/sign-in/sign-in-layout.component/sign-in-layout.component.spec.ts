import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignInLayoutComponent } from './sign-in-layout.component';

describe('SignInLayoutComponent', () => {
  let component: SignInLayoutComponent;
  let fixture: ComponentFixture<SignInLayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SignInLayoutComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SignInLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

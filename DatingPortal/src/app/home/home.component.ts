import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from '../services/authorization.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  isRegisterMode = false;

  constructor(private authService: AuthorizationService) { }

  ngOnInit(): void {}

  showRegister()
  {
    this.isRegisterMode = true;
  }

  RegisterationCancelled(isCancelled: boolean)
  {
    console.log('RegisterationCancelled');
    console.log(isCancelled);
    this.isRegisterMode = !isCancelled;
  }

}

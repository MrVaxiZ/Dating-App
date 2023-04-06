import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {}

  constructor(private accountService: AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  register(){
    this.accountService.register(this.model).subscribe({
      next:() => {
        this.cancel();
      },
      error: error => {
        this.toastr.error(error.error.errors.Password, error.error.errors.UserName); //Username error is in <b> for some reason
        console.log(error);
      }
    })
  }

  cancel(){
    this.cancelRegister.emit(false);
  }
}

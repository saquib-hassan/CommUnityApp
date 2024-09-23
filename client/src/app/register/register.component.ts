import { Component, input, Input, output, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  model: any = {};
  userFromHomeComponent = input.required<any>();
  cancelRegister = output<boolean>();
  register() {
    console.log(this.model);
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}

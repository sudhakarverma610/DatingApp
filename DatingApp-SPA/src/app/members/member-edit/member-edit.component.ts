import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router'; 
import { User } from 'src/app/models/user';
import { AlertifyService } from 'src/app/Services/Alertify.service';
import { UserService } from 'src/app/Services/user.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.scss']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm')  editForm: NgForm;
  user: User;
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any){
    if (this.editForm.dirty){
      $event.returnValue = true;
    }
  }
  constructor(private route: ActivatedRoute,
              private alert: AlertifyService,
              private userService: UserService) { }

  ngOnInit() {
    this.route.data.subscribe(
       res => {
        this.user = res.currentUser;
       }
    );
  }
  updateUser(){
    console.log(this.user);
    this.userService.UpdateUser(this.user.id,
      this.user).subscribe(_res => {
      this.alert.Success('Profile Update Successfully');
      this.editForm.reset(this.user);
     } , error => {
      this.alert.Error(error);
    });

  }
  memberPhotoMainChanged($event){
   // console.log($event);
    this.user.photoUrl = $event;
  }

}

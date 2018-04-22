import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';
import { User } from '../user';
@Component({
  selector: 'app-user-search',
  templateUrl: './user-search.component.html',
  styleUrls: ['./user-search.component.css'],
  providers: [UserService]
})
export class UserSearchComponent implements OnInit {

  SearchText: string = "";
  SearchResult: Array<User> = null;
  SpinnerHidden: boolean = true;

  constructor(private userService: UserService) { }

  Submit() {
    this.SpinnerHidden = false;
    this.SearchResult = null;
    this.userService.Search(this.SearchText)
    .then(result => {
      this.SearchResult = result;
      this.SpinnerHidden = true;
    })
    .catch(ex => {
      this.SpinnerHidden = true;
      console.log(ex)
    })
  }

  ngOnInit() {
  }

}

import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ShopKaro';

  constructor(private readonly httpClient: HttpClient) { }

  callApi() {
    // alert("api is getting called")
    return this.httpClient.get("https://localhost:44320/Auth/LogOut").subscribe((response) => {
      console.log("respose",response)
    })
  }
}

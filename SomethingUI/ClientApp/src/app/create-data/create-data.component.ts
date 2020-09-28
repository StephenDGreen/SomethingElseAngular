import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-create-data',
  templateUrl: './create-data.component.html'
})
export class CreateDataComponent {
  public somethings: Something[];
  public token: Token;
  public http: HttpClient;
  public baseUrl: string;
  public header: HttpHeaders;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }
  async ngOnInit() {
    await this.http.get<Token>(this.baseUrl + 'home/authenticate').subscribe(result => {
      this.token = result;
      this.header = new HttpHeaders().set(
        "Authorization",
        `Bearer ${this.token.access_token}`
      );
      this.http.get<Something[]>(this.baseUrl + 'api/things', { headers: this.header }).subscribe(result => {
        this.somethings = result;
      }, error2 => console.error(error2));
    }, error1 => console.error(error1));
  }
  async clickFunction() {
    await this.http.get<Token>(this.baseUrl + 'home/authenticate').subscribe(result => {
      this.token = result;
      this.header = new HttpHeaders()
        .set(
          "Authorization",
          `Bearer ${this.token.access_token}`
        )
        .set(
          'Content-Type',
          'application/x-www-form-urlencoded'
      );
      let possible = "abcdefghijklmnoprstuvwy";
      const lengthOfCode = 10;
      let name = this.makeRandom(lengthOfCode, possible);
      let body = `Name=${name}`;
      this.http.post<any>(this.baseUrl + 'api/things', body, { headers: this.header }).subscribe(result => {
        this.somethings = result;
      }, error2 => console.error(error2));
    }, error1 => console.error(error1));
  }
  makeRandom(lengthOfCode: number, possible: string) {
    let text = "";
    for (let i = 0; i < lengthOfCode; i++) {
      text += possible.charAt(Math.floor(Math.random() * possible.length));
    }
    return text;
  }
}

interface Something {
  name: string;
}

interface Token {
  access_token: string;
}

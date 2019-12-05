import { Injectable } from '@angular/core';
import { ContactDetails } from './contact-details.model';
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class ContactDetailsService {
formData:ContactDetails;
readonly rootURL = 'http://localhost:5000';
list : ContactDetails[];

constructor(private http: HttpClient) { }

postContactDetail() {
  return this.http.post(this.rootURL + '/contact', this.formData);
}
putContactDetail() {
  return this.http.put(this.rootURL + '/contact/'+ this.formData.Id, this.formData);
}
deleteContactDetail(id) {
  return this.http.delete(this.rootURL + '/contact/'+ id);
}

ContactList(){
  this.http.get(this.rootURL + '/contact')
  .toPromise()
  .then(res => this.list = res as ContactDetails[]);
}
}

import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ContactDetailsService } from 'src/app/shared/contact-details.service';
import { ContactDetails } from 'src/app/shared/contact-details.model';

@Component({
  selector: 'app-list-contacts',
  templateUrl: './list-contacts.component.html',
  styles: []
})
export class ListContactsComponent implements OnInit {
  constructor(public service: ContactDetailsService,
    private toastr: ToastrService) { }

  ngOnInit() {
    this.service.ContactList();
  }

  editForm(a: ContactDetails) {
    this.service.formData = Object.assign({}, a);
  }

  contactDelete(ContactId) {
    if (confirm('Are you sure to delete this record ?')) {
      this.service.deleteContactDetail(ContactId)
        .subscribe(res => {
          this.service.ContactList();
          this.toastr.warning('Contact details Deleted successfully', 'Contact Detail Register');
        },
          err => {
            console.log(err);
          })
    }
  }

}

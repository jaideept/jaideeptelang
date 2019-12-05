import { Component, OnInit } from '@angular/core';
import { ContactDetailsService } from 'src/app/shared/contact-details.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-contact',
  templateUrl: './add-contact.component.html',
  styleUrls: ['./add-contact.component.css']
})

export class AddContactComponent implements OnInit {

  constructor(public service:ContactDetailsService,private toastr: ToastrService) { }

  ngOnInit() {
    this.resetForm();
  }

  resetForm(form?: NgForm) {
    if (form != null)
      form.form.reset();
    this.service.formData = {
      Id:0,
      FirstName:'',
      LastName:'',
      Email:'',
      PhoneNo:'',
      Status:true
    }
  }

  onSubmit(form: NgForm) {
    if (this.service.formData.Id == 0)
      this.insertRecord(form);
    else
      this.updateRecord(form);
  }

  insertRecord(form: NgForm) {
    this.service.postContactDetail().subscribe(
      res => {
        this.resetForm(form);
        this.toastr.success('Submitted successfully', 'Contact Detail Register');
        this.service.ContactList();
      },
      err => {
        console.log(err);
      }
    )
  }
  updateRecord(form: NgForm) {
    this.service.putContactDetail().subscribe(
      res => {
        this.resetForm(form);
        this.toastr.info('Updated successfully', 'Contact Detail Register');
        this.service.ContactList();
      },
      err => {
        console.log(err);
      }
    )
  }

}


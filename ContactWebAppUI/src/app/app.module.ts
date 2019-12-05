import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import{ FormsModule} from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ContactDetailsComponent } from './contact-details/contact-details.component';
import { AddContactComponent } from './contact-details/add-contact/add-contact.component';
import { ListContactsComponent } from './contact-details/list-contacts/list-contacts.component';
import { ContactDetailsService } from './shared/contact-details.service';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    ContactDetailsComponent,
    AddContactComponent,
    ListContactsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot()
  ],
  providers: [ContactDetailsService],
  bootstrap: [AppComponent]
})
export class AppModule { }

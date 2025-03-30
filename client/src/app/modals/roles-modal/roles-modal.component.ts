import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AdminService } from '../../_services/admin.service';

@Component({
  selector: 'app-roles-modal',
  imports: [FormsModule],
  templateUrl: './roles-modal.component.html',
  styleUrl: './roles-modal.component.css'
})
export class RolesModalComponent {
  private adminService = inject(AdminService);
  bsModalRef = inject(BsModalRef);
  title: string = '';
  availableRoles: string[] = [];
  selectedRoles: string[] = [];
  username: string = '';
  rolesUpdated: boolean = false;

  updateChecked(checkedValue: string){
    if(this.selectedRoles.includes(checkedValue)){
      this.selectedRoles = this.selectedRoles.filter(r => r !== checkedValue);
    }
    else{
      this.selectedRoles.push(checkedValue);
    }
  }

  onSelectRoles(){
    this.rolesUpdated = true;
    this.bsModalRef.hide();
  }
}

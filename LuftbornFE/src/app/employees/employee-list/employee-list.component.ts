import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { EmployeeService } from 'src/app/shared/employee.service';
import { Employee } from 'src/app/shared/employee.model';
import { ToastrService } from 'ngx-toastr';
import { FlagService } from 'src/app/flag.service';

@Component({
  selector: 'app-employee-list',
  templateUrl:  './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {

  
  constructor(public service: EmployeeService,
    private toastr: ToastrService, private flash: FlagService) { 
  
    }
  ngOnInit() {
    this.service.refreshList();

  }

  populateForm(emp: Employee) {
  if(this.flash.checkflag()==true){
    this.service.formData = Object.assign({}, emp); 
    this.flash.updateflag(false);
  }
  }
  


  onDelete(id: string) {
    if(this.flash.checkflag()==true){
    if (confirm('Are you sure to delete this record?')) {
      this.service.deleteEmployee(id).subscribe(res => {
        this.service.refreshList();
        this.toastr.warning('Deleted successfully', 'EMP. Register');
      });
    }
  }
  }
}

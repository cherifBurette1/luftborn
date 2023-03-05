import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { EmployeeService } from 'src/app/shared/employee.service';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { FlagService } from 'src/app/flag.service';
import { Employee } from 'src/app/shared/employee.model';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css']
})
export class EmployeeComponent implements OnInit {
  constructor(public service: EmployeeService,private flag: FlagService,
    private toastr: ToastrService) { }
    Category=['One','Two','Three'];
    Tag=["1","2","3"]
 button=0;
 black: boolean= false;
  ngOnInit() {  
    this.resetForm();
    }

  resetForm(form?: NgForm) {
    if (form != null)
      form.resetForm();
    this.service.formData = {
     id: "",
      title: '',
      category: '',
      description: '',
      tags: '', 
    }  
     this.flag.updateflag(true);
  }


  onSubmit(form: NgForm) {

     if (form.value.Id == "")
      {this.insertRecord(form);
         
        
      }
    else
     { this.updateRecord(form); 
    
     }
  }
  
  onChangeAddEdit(form: NgForm) {
    if (form.value._id == "")
   { 
   return 1;
   }
   else{
    
    return 2;
   }
 }

  insertRecord(form: NgForm) {
      var employee = this.mapDTO(form);
      this.service.postEmployee(employee).subscribe(res => {
      this.toastr.success('Inserted successfully', 'Item Added');
      this.resetForm(form);
      this.service.refreshList();
    });
  }

  updateRecord(form: NgForm) {
      var employee = this.mapDTO(form);
      this.service.putEmployee(employee).subscribe(res => {
      this.toastr.success('Updated successfully', 'Item Updated');
      this.resetForm(form);
        this.service.refreshList();
    },
    error =>
      (this.toastr.success('Updated successfully', 'Item Updated'))
     );

  }
  mapDTO(form: NgForm){
    var employee = new Employee();
    employee.category = form.value.Category;
    employee.description = form.value.Description;
    employee.tags = form.value.Tags;
    employee.title = form.value.Title;
    employee.id = form.value._id;
    return employee;
  }


}

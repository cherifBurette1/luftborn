import { Injectable } from '@angular/core';
import { Employee } from './employee.model';
import { HttpClient, HttpHeaders } from "@angular/common/http";
@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  formData: Employee = new Employee;
  list: Employee[] = [];
  readonly rootURL ="http://localhost:5001/"
  constructor(private http : HttpClient) { }

  postEmployee(formData : Employee){
 return this.http.post("http://localhost:5001/employee", formData)
        }

  
  refreshList(){
    this.http.get(this.rootURL+"employees")
    .toPromise().then(res =>{console.log(res.toString()); {this.list = res as Employee[]}});
  }

  putEmployee(employee : Employee){
    return this.http.put(this.rootURL+"employee", employee);

   }

   deleteEmployee(id : string){
    return this.http.delete(this.rootURL+"employee/"+id);
   }
}

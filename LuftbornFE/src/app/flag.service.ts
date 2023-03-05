import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FlagService {
  private alam: boolean;
  constructor() {
    this.alam=false;   }
checkflag(){
if(this.alam==true){
return true;
}else return false;
}
updateflag(flg: boolean){
if(flg==false){
this.alam=false;
}else {

  this.alam=true;
}
}
}

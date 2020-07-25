import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Value } from '../models/value';
import { faCoffee } from '@fortawesome/free-solid-svg-icons';
import { faStar } from '@fortawesome/free-regular-svg-icons';

@Component({
  selector: 'app-values',
  templateUrl: './values.component.html',
  styleUrls: ['./values.component.css']
})
export class ValuesComponent implements OnInit {

  constructor(private client: HttpClient) { }
  values: Array<Value>;
  faCoffee = faCoffee;
  faStar = faStar;
  ngOnInit(): void {

    this.client.get('http://localhost:5000/api/values')
    .subscribe(v => {
      console.log(v);
      this.values = v as Array<Value>;
    }, e => {
      console.log(e);
    });
  }

}

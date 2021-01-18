import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthorizationService } from '../services/authorization.service';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {

  constructor(
    private authorizationService: AuthorizationService,
    private templateRef: TemplateRef<any>,
    private viewContainerRef: ViewContainerRef
    ) {}

  @Input() appHasRole: string[];
  isVisible = false;

  ngOnInit(): void {
    const isAuthorize = this.authorizationService.isUserAuthorized(this.appHasRole);

    if (isAuthorize)
    {
      if (!this.isVisible)
      {
        this.viewContainerRef.createEmbeddedView(this.templateRef);
        this.isVisible = true;
      }
    }
    else
    {
      this.viewContainerRef.clear();
      this.isVisible = false;
    }
  }

}

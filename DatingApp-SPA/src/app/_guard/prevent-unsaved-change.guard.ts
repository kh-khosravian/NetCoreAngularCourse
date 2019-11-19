import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';


@Injectable()
export class PreventUnsavedChange implements CanDeactivate<MemberEditComponent> {
    canDeactivate(component: MemberEditComponent) {
        if (component.editFrom.dirty) {
            return confirm('are you sure you want to continue, any unsaved change will be lost');
        }
        return true;
    }
}
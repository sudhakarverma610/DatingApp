import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
@Injectable({
    providedIn: 'root'
})
export class PreventUnSavedGaurd implements CanDeactivate<MemberEditComponent>{

    canDeactivate(component: MemberEditComponent): boolean {
        if (component.editForm.dirty){
            return confirm('Are are Sure yo want to Countinue ? Any unsaved changed will be lost');
        }
        return true;
    }

}

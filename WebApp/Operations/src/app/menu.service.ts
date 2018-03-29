import { Injectable } from '@angular/core';
import { MenuItem } from './menu-item';
import { MENU_ITEMS } from './mock-menu-items'

@Injectable()
export class MenuService {

  constructor() { }

  getMenuItems() : Promise<Array<Array<MenuItem>>> {
    return Promise.resolve(MENU_ITEMS);
  }
}

import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filter',
  standalone: true,
})
export class FilterPipe implements PipeTransform {
  transform(items: any[], searchText: string): any[] {
    if (!items || !Array.isArray(items)) {
      return [];
    }

    if (!searchText || typeof searchText !== 'string') {
      return items;
    }

    const searchTerm = searchText.toLowerCase().trim();

    if (searchTerm === '') {
      return items;
    }

    return items.filter((item) => {
      if (!item) return false;

      return JSON.stringify(item).toLowerCase().includes(searchTerm);
    });
  }
}

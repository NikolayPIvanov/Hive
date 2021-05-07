import {FlatTreeControl} from '@angular/cdk/tree';
import {Component} from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import {MatTreeFlatDataSource, MatTreeFlattener} from '@angular/material/tree';
import { CategoriesClient, CategoryDto, PaginatedListOfCategoryDto } from 'src/app/clients/gigs-client';
import { ExploreService } from '../explore.service';

/**
 * Food data with nested structure.
 * Each node has a name and an optional list of children.
 */
interface ExpandableNode {
  id: number;
  name: string;
  children?: ExpandableNode[];
}

/** Flat node with expandable and level information */
interface ExampleFlatNode {
  id: number;
  expandable: boolean;
  name: string;
  level: number;
}

class CategoryNode implements ExpandableNode {
  id: number;
  name: string;
  children?: ExpandableNode[];

  constructor(id: number, name: string, children?: ExpandableNode[]) {
    this.id = id;
    this.name = name;
    this.children = children;
  }
}


@Component({
  selector: 'app-categories-tree',
  templateUrl: './categories-tree.component.html',
  styleUrls: ['./categories-tree.component.scss']
})
export class CategoriesTreeComponent {

  length = 500;
  pageSize = 10;
  pageIndex = 1;
  pageSizeOptions = [5, 10, 25];
  showFirstLastButtons = true;

  private _transformer = (node: CategoryNode, level: number) => {
    return {
      id: node.id,
      expandable: !!node.children && node.children.length > 0,
      name: node.name,
      level: level
    };
  }

  private setDataSource(list: PaginatedListOfCategoryDto) {
    let nodes = list.items?.map(x => {
      let children = x.subCategories?.map(p => new CategoryNode(p.id!, p.title!, []));
      return new CategoryNode(x.id!, x.title!, children);
    });

    this.dataSource.data = nodes!;
  }

  treeControl = new FlatTreeControl<ExampleFlatNode>(node => node.level, node => node.expandable);

  treeFlattener = new MatTreeFlattener(
      this._transformer, node => node.level, node => node.expandable, node => node.children);

  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  constructor(private categoryApiClient: CategoriesClient,
              private exploreService: ExploreService) {
    this.categoryApiClient.getCategories(this.pageIndex, this.pageSize, true).subscribe(list => this.setDataSource(list))
  }

  selectCategory(id: number) {
    this.exploreService.changeSelectedCategory(id);
  }

  handlePageEvent(event: PageEvent) {
    this.length = event.length;
    this.pageSize = event.pageSize;
    if (event.pageIndex == 0) {
      this.pageIndex = 1;
    }
    else {
      this.pageIndex = event.pageIndex;
    }
    
    this.categoryApiClient.getCategories(this.pageIndex, this.pageSize, true).subscribe(list => this.setDataSource(list))
  }

  hasChild = (_: number, node: ExampleFlatNode) => node.expandable;
}

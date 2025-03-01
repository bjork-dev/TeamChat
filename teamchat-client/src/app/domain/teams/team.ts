import {Group} from './group';

export interface Team {
  id: number;
  name: string;
  description: string;
  users: number[];
  groups: Group[];
}


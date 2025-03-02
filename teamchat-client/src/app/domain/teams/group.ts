import {Message} from './message';

export interface Group {
  id: number;
  name: string;
  description: string;
  messages: Message[];
}

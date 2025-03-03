export interface User  {
  id: number;
  username: string;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
}


export const isAdmin = (user: User) => user.role === 'Admin';


export interface User {
  id: string;
  name: string;
  email: string;
  avatar: string;
  token?: string;
  refresh_token?:string;
}

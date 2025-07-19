import { User } from "./user";

export type AuthContextType = {
  user:User | null,
  login: (provider: "google") => Promise<User>;
  logout: () => Promise<void>;
  getCurrentUser: () => Promise<User>;
  setCurrentUser: (user: User) => void;
}

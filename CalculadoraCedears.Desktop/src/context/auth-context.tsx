import { postLogOutUserAsync } from "@/api/cedears-api";
import { deleteStore, getStore, setApiToken } from "@/lib/utils";
import { AuthContextType } from "@/types/auth-context-type";
import { LogOutUser } from "@/types/logout-user";
import { User } from "@/types/user";
import { invoke } from "@tauri-apps/api/core";
import React, { createContext } from "react";


const STORE_KEY = "currentUser";

export const AuthContext = createContext<AuthContextType | undefined>(
  undefined
);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {

  const login = async (provider: "google"): Promise<User> => {
    try {
      const userInfo = await invoke<{
        sub: string;
        name: string;
        email: string;
        token: string;
        refresh_token: string;
      }>("login_with_provider", { provider });

      const user: User = {
        id: userInfo.sub,
        name: userInfo.name,
        email: userInfo.email,
        avatar: "",
        token: userInfo.token,
        refresh_token: userInfo.refresh_token,
      };

      await setCurrentUser(user);
      setApiToken(userInfo.token, userInfo.refresh_token);

      return user;
    } catch (error) {
      console.error("Login failed:", error);
      throw error;
    }
  };

  const logout = async (): Promise<void> => {
    try {
      const user = await getCurrentUser();

      const request: LogOutUser = { userId: user.id };
      await postLogOutUserAsync(request);
      await deleteStore();
    } catch (error) {
      console.error("Logout failed:", error);
      await deleteStore();
      throw error;
    }
  };

  const setCurrentUser = async (user: User) => {
    const store = await getStore();
    await store.set(STORE_KEY, user);
    await store.save();
  };

  const getCurrentUser = async (): Promise<User> => {
    try {
      const store = await getStore();
      const user = await store.get<User>(STORE_KEY);
      if (!user) throw new Error("No hay usuario logueado");
      return user;
    } catch (error) {
      console.error("Failed to get stored user:", error);
      throw error;
    }
  };

  return (
    <AuthContext.Provider
      value={{ login, logout, getCurrentUser, setCurrentUser }}
    >
      {children}
    </AuthContext.Provider>
  );
};

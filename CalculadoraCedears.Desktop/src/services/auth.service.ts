import { User } from "@/types/user";
import { invoke } from "@tauri-apps/api/core";
import { postLogOutUserAsync } from "@/api/cedears-api";
import { LogOutUser } from "@/types/logout-user";
import { deleteStore, getCurrentUser, setCurrentUser } from "@/lib/utils";

export async function login(provider: "google"): Promise<User> {
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

    return user;
  } catch (error) {
    console.error("Login failed:", error);
    throw error;
  }
}

export async function logout(): Promise<void> {
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
}

import { postUserLoginAsync } from '@/api/cedears-api';
import { CreateUser } from '@/types/create-user';
import { User } from '@/types/user';
import { invoke } from '@tauri-apps/api/core';
import { load } from '@tauri-apps/plugin-store';

let currentUser: User | undefined;
let store: Awaited<ReturnType<typeof load>> | null = null;

async function getStore() {
  if (!store) {
    store = await load('user-store.json', { autoSave: true });
  }
  return store;
}


export async function login(provider: 'google'): Promise<User> {
  try {
    const userInfo = await invoke<{
      id: string;
      name: string;
      email: string;
      avatar: string | null;
      access_token: string;
    }>('login_with_provider', { provider });

    currentUser = {
      id: userInfo.id,
      name: userInfo.name,
      email: userInfo.email,
      avatar: userInfo.avatar || undefined,
      accessToken: userInfo.access_token,
    };

    const userRequest: CreateUser = {
      userId: userInfo.id,
      email: userInfo.email
    };

    // Store user in Tauri Store
    const store = await getStore();
    await store.set('user', currentUser);
    await store.save();
    console.log('User logged in:', currentUser);

    await postUserLoginAsync(userRequest);

    return currentUser;
  } catch (error) {
    console.error('Login failed:', error);
    throw error;
  }
}


export async function getCurrentUser(): Promise<User> {
  if (!currentUser) {
    try {
      const store = await getStore();
      currentUser = await store.get<User>('user');
    } catch (error) {
      console.error('Failed to get stored user:', error);
    }
  }
  return currentUser!;
}


export async function logout(): Promise<void> {
  const store = await getStore();
  await store.delete('user');
  await store.save();
  console.log('User logged out');
}
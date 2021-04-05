import { User } from "./User";
import { getItem, setItem } from "@/utils/LocalStorageUtilities";

export interface IUserState {
    user: User | null;
}

class UserState implements IUserState {
    public get user(): User | null {
        return getItem<User>('user');
    }
    public set user(value: User | null) {
        setItem('user', value);
    }
}

export const userState = new UserState();
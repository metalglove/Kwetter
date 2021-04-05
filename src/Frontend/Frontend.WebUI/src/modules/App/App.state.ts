import { getItem, setItem } from "@/utils/LocalStorageUtilities";

export interface IAppState  {
    drawer: boolean | null;
}


class AppState implements IAppState {
    constructor() {
        this.drawer = false;
    }

    public get drawer(): boolean {
        return getItem<boolean>('drawer')!;
    }

    public set drawer(value: boolean) {
        setItem('drawer', value);
    }
}

export const appState = new AppState();
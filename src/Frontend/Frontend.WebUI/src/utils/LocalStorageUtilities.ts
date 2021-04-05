export const setItem = (name: string, content: any): void => {
    if (!name)
        return;
    if (typeof content !== 'string') {
        content = JSON.stringify(content);
    }
    window.localStorage.setItem(name, content);
}

export const getItem = <T>(name: string): T | null => {
    if (!name)
        return null;
    return JSON.parse(window.localStorage.getItem(name)!) as T;
}

export const removeItem = (name: string): void => {
    if (!name)
        return;
    return window.localStorage.removeItem(name);
}
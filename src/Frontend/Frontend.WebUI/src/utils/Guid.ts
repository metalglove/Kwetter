/**
 * Represents the Guid class.
 */
export default class Guid {
    private value: string = this.empty;

    /**
     * Initializes an instance of the Guid class.
     * @param value The guid value.
     */
    constructor(value?: string) {
        if (value) {
            if (Guid.isValid(value)) {
                this.value = value;
            }
        }
    }

    /**
     * Gets the string value of the guid.
     */
    public toString() {
        return this.value;
    }

    /**
     * Creates a new guid.
     */
    public static newGuid(): Guid {
        return new Guid(
            'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (c: any) => {
                const r = (Math.random() * 16) | 0;
                const v = c === 'x' ? r : (r & 0x3) | 0x8;
                return v.toString(16);
            })
        );
    }

    /**
     * Creates an empty guid.
     * @returns an empty guid.
     */
    public static get empty(): string {
        return '00000000-0000-0000-0000-000000000000';
    }

    /**
     * Creates an empty guid.
     * @returns an empty guid.
     */
    public get empty(): string {
        return Guid.empty;
    }

    /**
     * Checks whether a string is a valid guid.
     * @param str The string to validate.
     * @returns A value whether the string was a valid guid.
     */
    public static isValid(str: string): boolean {
        const validRegex = /^[0-9A-Fa-f]{8}-[0-9A-Fa-f]{4}-[1-5][0-9A-Fa-f]{3}-[89ab][0-9A-Fa-f]{3}-[0-9A-Fa-f]{12}$/i;
        return validRegex.test(str);
    }
}
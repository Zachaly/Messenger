export default interface ResponseModel {
    success: boolean,
    error: string,
    errors: ["", []],
    newEntityId?: number
}
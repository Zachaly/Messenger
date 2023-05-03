import PagedRequest from "./PagedRequest";

export default interface GetUserBanRequest extends PagedRequest {
    userId?: number,
    start?: Date,
    end?: Date,
    id?: number
}
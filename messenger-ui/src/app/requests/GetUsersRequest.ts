import PagedRequest from "./PagedRequest";

export default interface GetUsersRequest extends PagedRequest {
    id?: number
}
import PagedRequest from "./PagedRequest";

export default interface GetUserClaimRequest extends PagedRequest {
    userId: number,
    value: string
}
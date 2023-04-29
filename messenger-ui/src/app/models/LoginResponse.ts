export default interface LoginResponse {
    authToken: string,
    userName: string,
    userId: number,
    claims: string[]
}
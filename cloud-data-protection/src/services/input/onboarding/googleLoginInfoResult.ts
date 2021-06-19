interface GoogleLoginInfoResult {
    state: string;
    clientId: string;
    redirectUri: string;
    scopes: string[];
}

export default GoogleLoginInfoResult;
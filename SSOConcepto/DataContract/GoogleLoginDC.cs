namespace SSOConcepto.DataContract
{
    public class GoogleLoginDC
    {
        public string idToken { get; set; }
        public string accessToken { get; set; }
        public long expiresAt { get; set; }
        public long expiresIn { get; set; }
        public string tokenType { get; set; }
    }
}

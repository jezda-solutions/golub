namespace Golub.Enums
{
    /// <summary>
    /// Email Provider Type, all types that API supports are here
    /// There can be more providers that implemented, but not less
    /// </summary>
    public enum EmailProviderType : byte
    {
        None,
        Sendgrid,
        Sendblue,
        Mailchimp,
        Mandrill,
        Brevo,
    }
}
namespace Golub.Settings
{
    /// <summary>
    /// Represents configuration settings for email delivery.  
    /// Currently, it includes a BCC (Blind Carbon Copy) option,  
    /// which allows tracking email success by ensuring a copy is sent  
    /// to a designated address. This class can be extended in the future  
    /// to support additional email-related settings as needed.  
    /// </summary>
    public class EmailSettings
    {
        public string Bcc { get; set; }
    }
}

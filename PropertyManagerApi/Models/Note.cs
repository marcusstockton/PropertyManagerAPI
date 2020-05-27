namespace PropertyManagerApi.Models
{
    /// <summary>
    /// Note class
    /// </summary>
    public class Note : Base
    {
        /// <summary>
        /// Title of the note
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of note
        /// </summary>
        public string Description { get; set; }
    }
}
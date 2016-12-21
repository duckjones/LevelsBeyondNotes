using System.ComponentModel.DataAnnotations;

namespace LevelsBeyondNotes.Models
{
    /// <summary>
    /// Very basic data model for a Note
    /// </summary>
    public class Note
    {
        [Key]
        public int? id { get; set; }
        public string body { get; set; }
    }
}
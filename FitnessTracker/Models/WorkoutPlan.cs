using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace FitnessTracker.Models
{
    public class WorkoutPlan
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public FitnessUser User { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string SessionsJSON { get; set; }
        [NotMapped]

        public WorkoutSession[] Sessions => string.IsNullOrEmpty(SessionsJSON) ? [] : JsonSerializer.Deserialize<WorkoutSession[]>(SessionsJSON);
    }
}

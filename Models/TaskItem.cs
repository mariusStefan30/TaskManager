using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campul este obligatoriu")]
        [StringLength(100, ErrorMessage = "Titlul poate avea maxim 100  de caractere")]
        public string Title { get; set; }


        [StringLength(500, ErrorMessage = "Descrierea poate avea maxim 500 de caractere")]
        public string Description { get; set; }


        [Display(Name = "Finalizat")]
        public bool IsDone { get; set; }
    }
}

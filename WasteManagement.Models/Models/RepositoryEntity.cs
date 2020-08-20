using System;
using System.ComponentModel.DataAnnotations;

namespace WasteManagement.Models
{
    public class RepositoryEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
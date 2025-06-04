using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CertificateManager.Server.Models // We'll keep this namespace for now, anticipating moving the file later
{
    public class Certificate
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty; // Friendly name

        [Required]
        [StringLength(500)]
        public string Issuer { get; set; } = string.Empty; // Certificate Issuer

        [Required]
        [StringLength(500)]
        public string Subject { get; set; } = string.Empty; // Certificate Subject

        [Required]
        [StringLength(100)]
        public string Thumbprint { get; set; } = string.Empty; 

        [Required]
        public DateTime ValidFrom { get; set; }

        [Required]
        public DateTime ValidTo { get; set; }

        [Required]
        [StringLength(255)]
        public string SerialNumber { get; set; } = string.Empty;

        [StringLength(1024)]
        public string? PfxFilePath { get; set; } 

        public string? PfxPasswordHash { get; set; } 

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Collection of Subject Alternative Names
        public virtual ICollection<SubjectAlternativeName> SubjectAlternativeNames { get; set; } = new List<SubjectAlternativeName>();

        public Certificate()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

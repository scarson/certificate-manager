using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CertificateManager.Shared.Models // Keeping namespace consistent
{
    public class SubjectAlternativeName
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; } = string.Empty; // The SAN value (e.g., DNS name, IP address)

        // Foreign Key to Certificate
        public Guid CertificateId { get; set; }
        [ForeignKey("CertificateId")]
        public virtual Certificate? Certificate { get; set; }

        public SubjectAlternativeName()
        {
            Id = Guid.NewGuid();
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FilmWPF
{
    using System;
    using System.Collections.Generic;
    
    public partial class film
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public film()
        {
            this.appartenir = new HashSet<appartenir>();
            this.jouer = new HashSet<jouer>();
            this.parrution = new HashSet<parrution>();
            this.reserver = new HashSet<reserver>();
        }
    
        public int Id { get; set; }
        public string Titre { get; set; }
        public System.TimeSpan Duree { get; set; }
        public string Image { get; set; }
        public int NbEntrees { get; set; }
        public Nullable<int> NumRealisateur { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<appartenir> appartenir { get; set; }
        public virtual realisateur realisateur { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<jouer> jouer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<parrution> parrution { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reserver> reserver { get; set; }
    }
}

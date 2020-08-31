using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Ajka.DAL.Model.Interfaces;

namespace Ajka.DAL.Model
{
    /// <summary>
    /// Group of goods.
    /// </summary>
    [Table(nameof(Category))]
    public class Category : IEntity<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// Title text.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Text identification of goods group, used in import Adler products function.
        /// </summary>
        public string GroupIdentifier { get; set; }

        /// <summary>
        /// Validity of record. When is false, the title is not displayed.
        /// </summary>
        public bool IsValid { get; set; }

        public virtual IList<ItemCard> ItemCards { get; set; } = new List<ItemCard>();
    }
}

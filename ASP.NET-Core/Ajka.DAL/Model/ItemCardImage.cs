using System.ComponentModel.DataAnnotations.Schema;
using Ajka.DAL.Model.Interfaces;

namespace Ajka.DAL.Model
{
    /// <summary>
    /// Images for item cards.
    /// </summary>
    [Table(nameof(ItemCardImage))]
    public class ItemCardImage : IEntity<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// Link to the item card table.
        /// </summary>
        public virtual ItemCard ItemCard { get; set; }

        public int ItemCardId { get; set; }

        /// <summary>
        /// Description of the color of the goods.
        /// </summary>
        public string ColorName { get; set; }

        /// <summary>
        /// Relative network path to image.
        /// </summary>
        public string ImagePath { get; set; }
    }
}

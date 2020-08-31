using Ajka.DAL.Model.Interfaces;

namespace Ajka.BL.Models.ItemCard
{
    /// <summary>
    /// Images for item cards.
    /// </summary>
    public class ItemCardImageDto : IEntity<int>
    {
        public int Id { get; set; }

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

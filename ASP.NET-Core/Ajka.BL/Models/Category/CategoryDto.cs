using Ajka.DAL.Model.Interfaces;

namespace Ajka.BL.Models.Category
{
    /// <summary>
    /// Group of goods.
    /// </summary>
    public class CategoryDto : IEntity<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// Title text.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Validity of record. When is false, the title is not displayed.
        /// </summary>
        public bool IsValid { get; set; }
    }
}

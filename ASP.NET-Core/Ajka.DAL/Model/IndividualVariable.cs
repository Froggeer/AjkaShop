using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ajka.DAL.Model.Interfaces;

namespace Ajka.DAL.Model
{
    /// <summary>
    /// Space for store single variables for users.
    /// </summary>
    [Table(nameof(IndividualVariable))]
    public class IndividualVariable : IEntity<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// Value for searching records.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string KeyName { get; set; }

        /// <summary>
        /// Users value in string format.
        /// </summary>
        public string ValueString { get; set; }
    }
}

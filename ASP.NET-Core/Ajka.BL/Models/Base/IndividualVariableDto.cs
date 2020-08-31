using Ajka.DAL.Model.Interfaces;

namespace Ajka.BL.Models.Base
{
    public class IndividualVariableDto : IEntity<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// Value for searching records.
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// Users value in string format.
        /// </summary>
        public string ValueString { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using Ajka.DAL.Model.Interfaces;

namespace Ajka.DAL.Model
{
    /// <summary>
    /// Evidence of users.
    /// </summary>
    [Table(nameof(User))]
    public class User : IEntity<int>
    {
        public int Id { get; set; }

        /// <summary>
        /// Person name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Person surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Hashed password for login to system.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// If true, the user has system administration rights.
        /// </summary>
        public bool IsAdministrator { get; set; }

        /// <summary>
        /// Validity of record.
        /// </summary>
        public bool IsValid { get; set; }
    }
}

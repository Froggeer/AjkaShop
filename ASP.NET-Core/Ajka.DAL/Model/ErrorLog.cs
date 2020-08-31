using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ajka.DAL.Model.Interfaces;

namespace Ajka.DAL.Model
{
    /// <summary>
    /// Evidence of catched exceptions.
    /// </summary>
    [Table(nameof(ErrorLog))]
    public class ErrorLog : IEntity<int>
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Message from exception.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// All information from exception include inner exceptions.
        /// </summary>
        public string FullDescription { get; set; }
    }
}

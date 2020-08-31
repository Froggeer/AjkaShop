namespace Ajka.Common.Models.Base
{
    public class CrudFilterModel
    {
        public int ObjectsPerPage { get; set; }

        public int PageNumber { get; set; }

        public string OrderColumn { get; set; }

        public bool IsDescendingOrder { get; set; }
    }
}

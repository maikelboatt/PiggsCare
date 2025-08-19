using System.Data;

namespace PiggsCare.DataAccess.Helpers
{
    public static class SqlTableHelpers
    {
        public static DataTable ToIntListDataTable( IEnumerable<int> ids )
        {
            DataTable table = new();
            table.Columns.Add("AnimalId", typeof(int)); // Must match column name in SQL type
            foreach (int id in ids)
            {
                table.Rows.Add(id);
            }
            return table;
        }
    }
}

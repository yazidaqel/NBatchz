using System;
using System.Text;

namespace NBatchzInfrastructure.Database.Support
{
    public class SqlPagingQueryUtils
    {
        private SqlPagingQueryUtils()
        {
        }

        public static String GenerateRowNumSqlQuery(AbstractSqlPagingQueryProvider provider, String selectClause,
            bool remainingPageQuery, String rowNumClause)
        {

            string whereClause = string.IsNullOrEmpty(provider.WhereClause) ? string.Empty : " WHERE " + provider.WhereClause;

            string sqlQuery = $@"SELECT * FROM (SELECT {selectClause} FROM {provider.FromClause}
{whereClause} ORDER BY {BuildSortClause(provider)}) WHERE {rowNumClause}";

            StringBuilder sql = new StringBuilder();
            sql.Append(sqlQuery);
            if (remainingPageQuery)
            {
                sql.Append(" AND ");
                List<string> clauses = BuildSortConditions(provider);


                sql.Append('(');
                String prefix = "";

                foreach (String curClause in clauses)
                {
                    sql.Append(prefix);
                    prefix = " OR ";
                    sql.Append('(');
                    sql.Append(curClause);
                    sql.Append(')');
                }
                sql.Append(')');
            }

            return sql.ToString();

        }

        public static string BuildSortClause(AbstractSqlPagingQueryProvider provider)
        {
            return BuildSortClause(provider.SortKeys);
        }


        public static string BuildSortClause(Dictionary<string, Order> sortKeys)
        {
            StringBuilder builder = new();
            String prefix = "";



            foreach (var sortKey in sortKeys)
            {
                builder.Append(prefix);

                prefix = ", ";

                builder.Append(sortKey.Key);

                if (sortKey.Value == Order.DESCENDING)
                {
                    builder.Append(" DESC");
                }
                else
                {
                    builder.Append(" ASC");
                }
            }

            return builder.ToString();
        }

        public static List<string> BuildSortConditions(AbstractSqlPagingQueryProvider provider)
        {
            List<String> clauses = new List<string>();

            foreach (var entry in provider.SortKeys)
            {
                StringBuilder clause = new StringBuilder();
                clause.Append(entry.Key);

                if (entry.Value == Order.DESCENDING)
                {
                    clause.Append(" < ?");
                }
                else
                {
                    clause.Append(" > ?");
                }

                clauses.Add(clause.ToString());
            }

            return clauses;
        }
    }
}


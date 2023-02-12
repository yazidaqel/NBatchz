using System;
using System.Text;

namespace NBatchzInfrastructure.Database.Support
{
    public abstract class AbstractSqlPagingQueryProvider : PagingQueryProvider
    {

        private string? _selectClause;

        public string SelectClause
        {
            get
            {
                return this._selectClause;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    this._selectClause = value.Replace("select", string.Empty);
            }
        }

        public string FromClause { get; set; }

        public string WhereClause { get; set; }

        public Dictionary<string, Order> SortKeys { get; set; }


        private string _groupClause;

        public string GroupClause
        {
            get
            {
                return this._groupClause;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this._groupClause = value.Replace("group by", string.Empty);
                }
            }
        }

        private int ParameterCount { get; set; }

        public abstract string GenerateFirstPageQuery(int pageSize);

        public abstract string GenerateRemainingPagesQuery(int pageSize);

        public Dictionary<string, Order> GetSortKeys()
        {
            return this.SortKeys;
        }

        public void Init()
        {
            StringBuilder sql = new StringBuilder(64);
            sql.Append("SELECT ").Append(_selectClause);
            sql.Append(" FROM ").Append(FromClause);
            if (WhereClause != null)
            {
                sql.Append(" WHERE ").Append(WhereClause);
            }
            if (GroupClause != null)
            {
                sql.Append(" GROUP BY ").Append(GroupClause);
            }
        }
    }
}


using System;
using System.Text;

namespace NBatchzInfrastructure.Database.Support
{
    public class OraclePagingQueryProvider : AbstractSqlPagingQueryProvider
    {

        public override string GenerateFirstPageQuery(int pageSize)
        {
            return SqlPagingQueryUtils.GenerateRowNumSqlQuery(this, this.SelectClause, false, BuildRowNumClause(pageSize));
        }

        public override string GenerateRemainingPagesQuery(int pageSize)
        {
            return SqlPagingQueryUtils.GenerateRowNumSqlQuery(this, this.SelectClause, true, BuildRowNumClause(pageSize));
        }

        private String BuildRowNumClause(int pageSize)
        {
            return new StringBuilder().Append("ROWNUM <= ").Append(pageSize).ToString();
        }
    }
}


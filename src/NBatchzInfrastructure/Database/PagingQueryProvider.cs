using System;
namespace NBatchzInfrastructure.Database
{
	public interface PagingQueryProvider
	{

        public void Init();

        public string GenerateFirstPageQuery(int pageSize);

        public string GenerateRemainingPagesQuery(int pageSize);

        public Dictionary<string, Order> GetSortKeys();

    }
}


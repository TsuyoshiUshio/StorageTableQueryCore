using System;
using Xunit;

namespace TableQuerySpike.Test
{
    public class OrchestrationInstanceStatusQueryBuilderTest
    {
        [Fact]
        public void Test_OrchestrationInstanceQuery_RuntimeStatus()
        {
            var queryBuilder = new OrchestrationInstanceStatusQuerBuilder();
            queryBuilder.AddRuntimeStatus("Runnning");
            var query = queryBuilder.Build();
            Assert.Equal("RuntimeStatus eq 'Runnning'", query.FilterString);
        }

        [Fact]
        public void Test_OrchestrationInstanceQuery_CreatedTime()
        {
            var queryBuilder = new OrchestrationInstanceStatusQuerBuilder();
            var createdDateFrom =  new DateTime(2018, 1, 10, 10, 10, 10);
            Console.WriteLine(createdDateFrom.ToString());
            var createdDateTo = new DateTime(2018, 1, 10, 10, 10, 50);

            queryBuilder.AddCreatedDate(createdDateFrom, createdDateTo);
            var result = queryBuilder.Build().FilterString;
            Assert.Equal("(CreatedDate ge datetime'2018-01-10T01:10:10.0000000Z') and (CreatedDate le datetime'2018-01-10T01:10:50.0000000Z')", queryBuilder.Build().FilterString);

        }

        [Fact]
        public void Test_OrchestrationInstanceQuery_CreatedTimeVariations()
        {
            var queryBuilder = new OrchestrationInstanceStatusQuerBuilder();
            var createdDateFrom = new DateTime(2018, 1, 10, 10, 10, 10);
            Console.WriteLine(createdDateFrom.ToString());
            queryBuilder.AddCreatedDate(createdDateFrom, default(DateTime));
            Assert.Equal("CreatedDate ge datetime'2018-01-10T01:10:10.0000000Z'", queryBuilder.Build().FilterString);
        }        
    }
}

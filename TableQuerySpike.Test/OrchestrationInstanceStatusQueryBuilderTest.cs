using System;
using Xunit;

namespace TableQuerySpike.Test
{
    public class OrchestrationInstanceStatusQueryBuilderTest
    {
        [Fact]
        public void Test_OrchestrationInstanceQuery_RuntimeStatus()
        {
            var queryBuilder = new OrchestrationInstanceStatusQueryBuilder();
            queryBuilder.AddRuntimeStatus("Runnning");
            var query = queryBuilder.Build();
            Assert.Equal("RuntimeStatus eq 'Runnning'", query.FilterString);
        }

        [Fact]
        public void Test_OrchestrationInstanceQuery_CreatedTime()
        {
            var queryBuilder = new OrchestrationInstanceStatusQueryBuilder();
            var createdTimeFrom =  new DateTime(2018, 1, 10, 10, 10, 10);
            var createdTimeTo = new DateTime(2018, 1, 10, 10, 10, 50);

            queryBuilder.AddCreatedTime(createdTimeFrom, createdTimeTo);
            var result = queryBuilder.Build().FilterString;
            Assert.Equal("(CreatedTime ge datetime'2018-01-10T01:10:10.0000000Z') and (CreatedTime le datetime'2018-01-10T01:10:50.0000000Z')", queryBuilder.Build().FilterString);

        }

        [Fact]
        public void Test_OrchestrationInstanceQuery_CreatedTimeVariations()
        {
            var queryBuilder = new OrchestrationInstanceStatusQueryBuilder();
            var createdTimeFrom = new DateTime(2018, 1, 10, 10, 10, 10);
            queryBuilder.AddCreatedTime(createdTimeFrom, default(DateTime));
            Assert.Equal("CreatedTime ge datetime'2018-01-10T01:10:10.0000000Z'", queryBuilder.Build().FilterString);

            var createdTimeTo = new DateTime(2018, 1, 10, 10, 10, 50);

            queryBuilder.AddCreatedTime(default(DateTime), createdTimeTo);
            Assert.Equal("CreatedTime le datetime'2018-01-10T01:10:50.0000000Z'", queryBuilder.Build().FilterString);
        }        

        [Fact]
        public void Test_OrchestrationInstanceQuery_Combination()
        {
            var queryBuilder = new OrchestrationInstanceStatusQueryBuilder();
            queryBuilder.AddRuntimeStatus("Runnning");
            var createdTimeFrom = new DateTime(2018, 1, 10, 10, 10, 10);
            var createdTimeTo = new DateTime(2018, 1, 10, 10, 10, 50);

            queryBuilder.AddCreatedTime(createdTimeFrom, createdTimeTo);
            var result = queryBuilder.Build().FilterString;
            Assert.Equal("((CreatedTime ge datetime'2018-01-10T01:10:10.0000000Z') and (CreatedTime le datetime'2018-01-10T01:10:50.0000000Z')) and (RuntimeStatus eq 'Runnning')", queryBuilder.Build().FilterString);

        }

        [Fact]
        public void Test_OrchestrationInstanceQuery_LastUpdatedTime()
        {
            var queryBuilder = new OrchestrationInstanceStatusQueryBuilder();
            var lastUpdatedTimeFrom = new DateTime(2018, 1, 10, 10, 10, 10);
            var lastUpdatedTimeTo = new DateTime(2018, 1, 10, 10, 10, 50);
            queryBuilder.AddLastUpdatedTime(lastUpdatedTimeFrom, lastUpdatedTimeTo);
            Assert.Equal("(LastUpdatedTime ge datetime'2018-01-10T01:10:10.0000000Z') and (LastUpdatedTime le datetime'2018-01-10T01:10:50.0000000Z')", queryBuilder.Build().FilterString);

        }
    }
}

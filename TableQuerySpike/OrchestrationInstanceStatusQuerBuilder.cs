using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace TableQuerySpike
{
    public class OrchestrationInstanceStatusQuerBuilder
    {
        private string RuntimeStatus { get; set; }
        private DateTime CreatedDateFrom { get; set; }
        private DateTime CreatedDateTo { get; set; }

        public OrchestrationInstanceStatusQuerBuilder AddRuntimeStatus(string runtimeStatus)
        {
            this.RuntimeStatus = runtimeStatus;
            return this;
        }

        public OrchestrationInstanceStatusQuerBuilder AddCreatedDate(DateTime createdDateFrom, DateTime createdDateTo)
        {
            this.CreatedDateFrom = createdDateFrom;
            this.CreatedDateTo = createdDateTo;
            return this;
        }

        public TableQuery<OrchestrationInstanceStatus> Build()
        {
            var query = new TableQuery<OrchestrationInstanceStatus>()
                .Where(
                    GetConditions()
                    );
            return query;
        }

        private string GetConditions()
        {
            var conditions = new List<string>();

            if (default(DateTime) != this.CreatedDateFrom)
            {
                conditions.Add(TableQuery.GenerateFilterConditionForDate("CreatedDate", QueryComparisons.GreaterThanOrEqual, new DateTimeOffset(this.CreatedDateFrom)));
            }

            if (default(DateTime) != this.CreatedDateTo)
            {
                conditions.Add(TableQuery.GenerateFilterConditionForDate("CreatedDate", QueryComparisons.LessThanOrEqual, new DateTimeOffset(this.CreatedDateTo)));
            }

            if (!string.IsNullOrEmpty(this.RuntimeStatus))
            {
                conditions.Add(TableQuery.GenerateFilterCondition("RuntimeStatus", QueryComparisons.Equal, this.RuntimeStatus));
            }

            if (conditions.Count == 1)
            {
                return conditions[0];
            } else
            {
                string lastCondition = null;
                foreach(var condition in conditions)
                {
                    if (string.IsNullOrEmpty(lastCondition))
                    {
                        lastCondition = condition;
                        continue;
                    }

                    lastCondition = TableQuery.CombineFilters(lastCondition, TableOperators.And, condition);
                    
                }
                return lastCondition;
            }
            
        }

    }
}

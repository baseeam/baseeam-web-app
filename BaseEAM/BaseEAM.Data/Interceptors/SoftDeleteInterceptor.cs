/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;

namespace BaseEAM.Data.Interceptors
{
    /// <summary>
    /// We only need to filter by IsDeleted = false
    /// </summary>
    public class SoftDeleteInterceptor : IDbCommandTreeInterceptor
    {
        public const string IsDeletedColumnName = "IsDeleted";
        public const string IsNewColumnName = "IsNew";

        public void TreeCreated(DbCommandTreeInterceptionContext interceptionContext)
        {
            if (interceptionContext.OriginalResult.DataSpace != DataSpace.SSpace)
            {
                return;
            }

            var queryCommand = interceptionContext.Result as DbQueryCommandTree;
            if (queryCommand != null)
            {
                interceptionContext.Result = HandleQueryCommand(queryCommand);
            }
        }

        private static DbCommandTree HandleQueryCommand(DbQueryCommandTree queryCommand)
        {
            var newQuery = queryCommand.Query.Accept(new SoftDeleteQueryVisitor());
            return new DbQueryCommandTree(
                queryCommand.MetadataWorkspace,
                queryCommand.DataSpace,
                newQuery);
        }

        public class SoftDeleteQueryVisitor : DefaultExpressionVisitor
        {
            public override DbExpression Visit(DbScanExpression expression)
            {
                var table = (EntityType)expression.Target.ElementType;
                //only need to check for IsDeleted column
                //IsDeleted and IsNew always go side by side in BaseEntity
                if (table.Properties.All(p => p.Name != IsDeletedColumnName))
                {
                    return base.Visit(expression);
                }

                var binding = expression.Bind();
                //return binding.Filter(
                //    binding.VariableType
                //    .Variable(binding.VariableName)
                //    .Property(IsDeletedColumnName)
                //    .Equal(DbExpression.FromBoolean(false)));

                var filter = DbExpressionBuilder.And(
                    binding.VariableType
                    .Variable(binding.VariableName)
                    .Property(IsDeletedColumnName)
                    .Equal(DbExpression.FromBoolean(false)),
                    binding.VariableType
                    .Variable(binding.VariableName)
                    .Property(IsNewColumnName)
                    .Equal(DbExpression.FromBoolean(false)));

                return binding.Filter(filter);
            }
        }
    }
}

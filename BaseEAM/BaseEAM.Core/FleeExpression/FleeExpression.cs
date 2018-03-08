/*******************************************************
 * Copyright 2016 (C) BaseEAM Systems, Inc
 * All Rights Reserved
*******************************************************/
using Ciloci.Flee;
using System;

namespace BaseEAM.Core
{
    public class FleeExpression
    {
        private string expression;
        private ExpressionContext context = null;
        private IDynamicExpression eDynamic = null;

        public FleeExpression()
        {
            context = new ExpressionContext();
        }

        public void AddType(Type type, string ns)
        {
            context.Imports.AddType(type, ns);
        }

        public void AddVariable(string name, object value)
        {
            context.Variables[name] = value;
        }

        public object Evaluate(string expression)
        {
            //compile
            try
            {
                this.expression = expression;
                eDynamic = context.CompileDynamic(expression);
            }
            catch (Exception e)
            {
                throw new BaseEamException("Cannot compile the expression '" + expression + "'. " + e.Message);
            }
            //evaluate
            try
            {
                return eDynamic.Evaluate();
            }
            catch (Exception e)
            {
                throw new Exception("Cannot evaluate the expression '" + this.expression + "'. " + e.Message);
            }
        }

        public T Evaluate<T>(string expression)
        {
            object result = Evaluate(expression);

            if (result is T)
                return (T)result;
            else
                return default(T);
        }

        #region Helper methods

        public static object Evaluate(BaseEntity entity, string expression)
        {
            var flee = new FleeExpression();
            flee.AddVariable("entity", entity);
            return flee.Evaluate(expression);
        }

        public static T Evaluate<T>(BaseEntity entity, string expression)
        {
            var flee = new FleeExpression();
            flee.AddVariable("entity", entity);
            return flee.Evaluate<T>(expression);
        }

        #endregion
    }
}

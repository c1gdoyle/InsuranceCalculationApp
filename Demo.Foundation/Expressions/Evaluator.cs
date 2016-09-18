using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Demo.Foundation.Expressions
{
    public static class Evaluator
    {
        /// <summary>
        /// Performs evaluation &amp; replacement of independent sub-trees.
        /// </summary>
        /// <param name="expression">The root of the expression-tree.</param>
        /// <returns>A new tree with sub-trees evaluated and replaced.</returns>
        public static Expression PartialEval(Expression expression)
        {
            return PartialEval(expression, Evaluator.CanBeEvaluatedLocally);
        }

        /// <summary>
        /// Performs evaluation &amp; replacement of independent sub-trees.
        /// </summary>
        /// <param name="expression">The root of the expression tree.</param>
        /// <param name="funcCanBeEvaluated">A function that decides whether a given expression node can be part of the local function.</param>
        /// <returns>A new tree with sub-tress evaluated and replaced.</returns>
        public static Expression PartialEval(Expression expression, Func<Expression, bool> funcCanBeEvaluated)
        {
            return new SubtreeEvaluator(new Nominator(funcCanBeEvaluated).Nominate(expression)).Eval(expression);
        }

        private static bool CanBeEvaluatedLocally(Expression expression)
        {
            return expression.NodeType != ExpressionType.Parameter;
        }

        /// <summary>
        /// Evaluates &amp; replaces sub-tress when first candidate is reached (top-down).
        /// </summary>
        private class SubtreeEvaluator : ExpressionVisitor
        {
            private readonly HashSet<Expression> _candidates;

            internal SubtreeEvaluator(HashSet<Expression> candidates)
            {
                _candidates = candidates;
            }

            internal Expression Eval(Expression exp)
            {
                return Visit(exp);
            }

            public override Expression Visit(Expression node)
            {
                if (node == null)
                {
                    return null;
                }
                if (_candidates.Contains(node))
                {
                    return Evaluate(node);
                }

                return base.Visit(node);
            }

            private Expression Evaluate(Expression e)
            {
                if (e.NodeType == ExpressionType.Constant)
                {
                    return e;
                }
                LambdaExpression lambda = Expression.Lambda(e);
                Delegate fn = lambda.Compile();
                return Expression.Constant(fn.DynamicInvoke(null), e.Type);
            }
        }

        /// <summary>
        /// Performs bottom-up analysis to determine which nodes can possibly
        /// be part of an evaluated sub-tree.
        /// </summary>
        private class Nominator : ExpressionVisitor
        {
            private Func<Expression, bool> _funcCanBeEvaluated;
            private HashSet<Expression> _candidates;
            private bool _cannotBeEvaluated;

            internal Nominator(Func<Expression, bool> funcCanBeEvaluated)
            {
                _funcCanBeEvaluated = funcCanBeEvaluated;
            }

            internal HashSet<Expression> Nominate(Expression expression)
            {
                _candidates = new HashSet<Expression>();
                Visit(expression);
                return _candidates;
            }

            public override Expression Visit(Expression node)
            {
                if (node != null)
                {
                    bool saveCannotBeEvaluated = _cannotBeEvaluated;
                    _cannotBeEvaluated = false;
                    base.Visit(node);
                    if (!_cannotBeEvaluated)
                    {
                        if (_funcCanBeEvaluated(node))
                        {
                            _candidates.Add(node);
                        }
                        else
                        {
                            _cannotBeEvaluated = true;
                        }
                    }
                    _cannotBeEvaluated |= saveCannotBeEvaluated;
                }
                return base.Visit(node);
            }
        }
    }

}

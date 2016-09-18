using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using Demo.Foundation.Expressions;
using Demo.Foundation.Extensions;
using Prism.Commands;

namespace Demo.Presentation.Commands
{
    /// <summary>
    /// A <see cref="DelegateCommand{T}"/> subclass which supports using
    /// a property as the delegate which controls the command's CanExecute
    /// state.
    /// </summary>
    /// <typeparam name="T">The type of the command parameter.</typeparam>
    public sealed class NotifiedDelegateCommand<T> : DelegateCommand<T>
    {
        #region Private Members
        private readonly Dictionary<INotifyPropertyChanged, HashSet<string>> _propertyNamesByController;
        #endregion Private Members

        #region Constructors
        /// <summary>
        /// Initialises a new instance of <see cref="NotifiedDelegateCommand{T}"/> with the <see cref="Action{T}"/> to invoke
        /// on execution and an ExpressionTree for determining if the command can execute.
        /// </summary>
        /// <param name="onExecute">The action to invoke.</param>
        /// <param name="canExecute">The ExpressionTree.</param>
        public NotifiedDelegateCommand(Action onExecute, Expression<Func<bool>> canExecute)
            : this(CreateOnExecuted(onExecute), canExecute)
        {

        }

        /// <summary>
        /// Initialises a new instance of <see cref="NotifiedDelegateCommand"/> with the <see cref="Action"/> to invoke
        /// on execution and an ExpressionTree for determining if the command can execute.
        /// </summary>
        /// <param name="onExecute">The action to invoke.</param>
        /// <param name="canExecute">The ExpressionTree.</param>
        public NotifiedDelegateCommand(Action<T> onExecute, Expression<Func<bool>> canExecute)
            : base(onExecute, CreateCanExecute(canExecute))
        {
            if (!new CanExecuteFinder().TryFind(canExecute, out _propertyNamesByController))
            {
                throw new ArgumentException("Could not find any property references or INotifiyPropertyChanged instances.", "canExecute");
            }

            foreach (INotifyPropertyChanged controller in _propertyNamesByController.Keys)
            {
                controller.PropertyChanged += OnNotifierPropertyChanged;
            }
        }
        #endregion Constructors

        #region Event Handlers
        private void OnNotifierPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            INotifyPropertyChanged controller = sender as INotifyPropertyChanged;
            HashSet<string> propertyNames;

            if (controller != null &&
                _propertyNamesByController.TryGetValue(controller, out propertyNames) &&
                propertyNames.Contains(e.PropertyName))
            {
                RaiseCanExecuteChanged();
            }
        }
        #endregion Event Handlers

        private static Func<T, bool> CreateCanExecute(Expression<Func<bool>> expression)
        {
            Func<bool> compiled = expression.Compile();
            return x => compiled();
        }

        private static Action<T> CreateOnExecuted(Action action)
        {
            return x => action();
        }
    }

    ///<summary>
    /// A <see cref="DelegateCommand"/> subclass which supports using
    /// a property as the delegate which controls the command's CanExecute
    /// state.
    /// </summary>
    public sealed class NotifiedDelegateCommand : DelegateCommand
    {
        #region Private Members
        private readonly Dictionary<INotifyPropertyChanged, HashSet<string>> _propertyNamesByController;
        #endregion Private Members

        #region Constructors
        /// <summary>
        /// Initialises a new instance of <see cref="NotifiedDelegateCommand"/> with the <see cref="Action"/> to invoke
        /// on execution and an ExpressionTree for determining if the command can execute.
        /// </summary>
        /// <param name="onExecute">The action to invoke.</param>
        /// <param name="canExecute">The ExpressionTree.</param>
        public NotifiedDelegateCommand(Action onExecute, Expression<Func<bool>> canExecute)
            : base(onExecute, canExecute.Compile())
        {
            if (!new CanExecuteFinder().TryFind(canExecute, out _propertyNamesByController))
            {
                throw new ArgumentException("Could not find any property references or INotifiyPropertyChanged instances.", "canExecute");
            }

            foreach (INotifyPropertyChanged controller in _propertyNamesByController.Keys)
            {
                controller.PropertyChanged += OnNotifierPropertyChanged;
            }
        }
        #endregion Constructors

        #region Event Handlers
        private void OnNotifierPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            INotifyPropertyChanged controller = sender as INotifyPropertyChanged;
            HashSet<string> propertyNames;

            if (controller != null &&
                _propertyNamesByController.TryGetValue(controller, out propertyNames) &&
                propertyNames.Contains(e.PropertyName))
            {
                RaiseCanExecuteChanged();
            }
        }
        #endregion Event Handlers
    }

    internal class CanExecuteFinder : ExpressionVisitor
    {
        private readonly Dictionary<INotifyPropertyChanged, HashSet<string>> _propertyNamesByController = new Dictionary<INotifyPropertyChanged, HashSet<string>>();
        private INotifyPropertyChanged _lastController;

        public bool TryFind(Expression expression, out Dictionary<INotifyPropertyChanged, HashSet<string>> propertyNamesByController)
        {
            Visit(expression);
            propertyNamesByController = _propertyNamesByController;
            return _propertyNamesByController.Count != 0;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            INotifyPropertyChanged controller = node.Value as INotifyPropertyChanged;
            if (controller != null)
            {
                _lastController = controller;
            }
            return base.VisitConstant(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Member is PropertyInfo)
            {
                //try and see if this is where the controller is
                Expression instance = node.Expression;
                if (typeof(INotifyPropertyChanged).IsAssignableFrom(instance.Type))
                {
                    Visit(instance);

                    if (_lastController == null)
                    {
                        Expression evaluated = Evaluator.PartialEval(instance);
                        Visit(evaluated);

                        if (_lastController == null)
                        {
                            LambdaExpression lambda = Expression.Lambda(evaluated);
                            try
                            {
                                _lastController = lambda.Compile().DynamicInvoke() as INotifyPropertyChanged;
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    if (_lastController != null)
                    {
                        HashSet<string> propertyNames = _propertyNamesByController.GetOrAdd(_lastController, x => new HashSet<string>(StringComparer.InvariantCultureIgnoreCase));
                        propertyNames.Add(node.Member.Name);
                    }
                }
            }
            return base.VisitMember(node);
        }
    }

}

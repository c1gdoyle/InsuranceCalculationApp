using System;
using System.ComponentModel;
using Demo.Presentation.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Demo.Presentation.Tests.Commands
{
    [TestClass]
    public class NotifiedDelegateCommandTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NotifiedDelegateCommandThrowsIfCanExecuteNotAProperty()
        {
            Mock<ITestController> mockController = new Mock<ITestController>();
            ITestController controller = mockController.Object;

            var command = new NotifiedDelegateCommand<object>(() => { }, () => controller.CanExecute());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NotifiedDelegateCommandThrowsIfCanExecuteIsNotINotifyPropertyChanged()
        {
            Mock<ITestController> mockController = new Mock<ITestController>();
            ITestController controller = mockController.Object;

            var command = new NotifiedDelegateCommand<object>(() => { }, () => controller.IsEnabled);
        }

        [TestMethod]
        public void NotifiedDelegateCommandCanExecuteNotRaisedOnWrongPropertyChanged()
        {
            Mock<ITestViewController> mockController = new Mock<ITestViewController>();
            ITestViewController controller = mockController.Object;

            int canExecuteChangedCount = 0;
            var command = new NotifiedDelegateCommand<object>(() => { }, () => controller.IsEnabled);

            mockController.Raise(x => x.PropertyChanged += null, new PropertyChangedEventArgs("Foo"));

            Assert.AreEqual(0, canExecuteChangedCount);
        }

        [TestMethod]
        public void NotifiedDelegateCommandCanExecuteRaisedOnPropertyChanged()
        {
            Mock<ITestViewController> mockController = new Mock<ITestViewController>();
            ITestViewController controller = mockController.Object;

            int canExecuteChangedCount = 0;
            var command = new NotifiedDelegateCommand<object>(() => { }, () => controller.IsEnabled);
            command.CanExecuteChanged += (x, y) => canExecuteChangedCount++;

            mockController.Raise(x => x.PropertyChanged += null, new PropertyChangedEventArgs("IsEnabled"));

            Assert.AreEqual(1, canExecuteChangedCount);
        }

        public interface ITestController
        {
            bool IsEnabled { get; }

            bool CanExecute();
        }

        public interface ITestViewController : ITestController, INotifyPropertyChanged
        {
        }
    }

}

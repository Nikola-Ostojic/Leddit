using Mobile.Core.Services.Scheduler;
using Moq;
using NUnit.Framework;
using Sextant.Abstraction;
using Splat;
using System;
using System.Reactive;

namespace Mobile.ViewModels.UnitTests.Base
{
    [TestFixture]
    public sealed class ViewModelBaseTest
    {
        private TestSchedulerService _schedulerServiceMock;
        private Mock<IViewStackService> _viewStackServiceMock;
        private Mock<ViewModelBase> _targetMock;
        private ViewModelBase _target;

        [SetUp]
        public void Setup()
        {
            _schedulerServiceMock = new TestSchedulerService();
            _viewStackServiceMock = new Mock<IViewStackService>() { DefaultValue = DefaultValue.Mock };

            Locator.CurrentMutable.Register(() => _schedulerServiceMock, typeof(ISchedulerService));
            Locator.CurrentMutable.Register(() => _viewStackServiceMock.Object, typeof(IViewStackService));

            _targetMock = new Mock<ViewModelBase>(
                _schedulerServiceMock,
                _viewStackServiceMock.Object,
                " ");

            _target = _targetMock.Object;
        }

        [Test]
        public void ViewModelBase_Instantiated_InitializesInteractions()
        {
            Assert.IsNotNull(_target.ShowAlert);
            Assert.IsNotNull(_target.ShowSuccess);
            Assert.IsNotNull(_target.ShowError);
            Assert.IsNotNull(_target.ShowInfo);
            Assert.IsNotNull(_target.Confirm);
        }

        [Test]
        public void ViewModelBase_Instantiated_ActivatorIsNotNull()
        {
            Assert.IsNotNull(_target.Activator);
        }

        [Test]
        public void ShowGenericError_WithMessageAndNoException_CallsShowErrorInteractionWithDesiredMessage()
        {
            string expected = "Any error message";
            string actual = null;

            _target
                .ShowError
                .RegisterHandler(
                    ctx =>
                    {
                        actual = ctx.Input;
                        ctx.SetOutput(Unit.Default);
                    });

            _target
                .ShowGenericError(expected, null)
                .Subscribe();

            _schedulerServiceMock.AdvanceBy(TimeSpan.FromSeconds(1));

            Assert.AreSame(expected, actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShowGenericError_WithMessageAndException_CallsShowErrorAndLog()
        {
            string expected = "Any error message";
            string actual = null;

            _target
                .ShowError
                .RegisterHandler(
                    ctx =>
                    {
                        actual = ctx.Input;
                        ctx.SetOutput(Unit.Default);
                    });

            _target
                .ShowGenericError(expected, new ArgumentNullException())
                .Subscribe();

            _schedulerServiceMock.AdvanceBy(TimeSpan.FromSeconds(1));

            Assert.AreSame(expected, actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShowSuccessMessage_MessageIsNotNull_CallsShowSuccessInteractionWithDesiredMessage()
        {
            string expected = "Any message";
            string actual = null;

            _target
                .ShowSuccess
                .RegisterHandler(
                    ctx =>
                    {
                        actual = ctx.Input;
                        ctx.SetOutput(Unit.Default);
                    });

            _target
                .ShowSuccessMessage(expected)
                .Subscribe();

            _schedulerServiceMock.AdvanceBy(TimeSpan.FromSeconds(1));

            Assert.AreSame(expected, actual);
            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void ShowAlertMessage_MessageIsNotNull_CallsShowAlertInteractionWithDesiredMessage()
        {
            string expected = "Any message";
            string actual = null;

            _target
                .ShowAlert
                .RegisterHandler(
                    ctx =>
                    {
                        actual = ctx.Input;
                        ctx.SetOutput(Unit.Default);
                    });

            _target
                .ShowAlertMessage(expected)
                .Subscribe();

            _schedulerServiceMock.AdvanceBy(TimeSpan.FromSeconds(1));

            Assert.AreSame(expected, actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShowToastMessage_MessageIsNotNull_CallsShowToastInteractionWithDesiredMessage()
        {
            string expected = "Any message";
            string actual = null;

            _target
                .ShowSuccess
                .RegisterHandler(
                    ctx =>
                    {
                        actual = ctx.Input;
                        ctx.SetOutput(Unit.Default);
                    });

            _target
                .ShowSuccessMessage(expected)
                .Subscribe();

            _schedulerServiceMock.AdvanceBy(TimeSpan.FromSeconds(1));

            Assert.AreSame(expected, actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShowGenericError_WithMessageAndNoException_CallsShowErrorToastInteractionWithDesiredMessage()
        {
            string expected = "Any error message";
            string actual = null;

            _target
                .ShowError
                .RegisterHandler(
                    ctx =>
                    {
                        actual = ctx.Input;
                        ctx.SetOutput(Unit.Default);
                    });

            _target
                .ShowGenericError(expected, null)
                .Subscribe();

            _schedulerServiceMock.AdvanceBy(TimeSpan.FromSeconds(1));

            Assert.AreSame(expected, actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShowGenericError_WithMessageAndException_CallsShowErrorToastAndLog()
        {
            string expected = "Any error message";
            string actual = null;

            _target
                .ShowError
                .RegisterHandler(
                    ctx =>
                    {
                        actual = ctx.Input;
                        ctx.SetOutput(Unit.Default);
                    });

            _target
                .ShowGenericError(expected, new ArgumentNullException())
                .Subscribe();

            _schedulerServiceMock.AdvanceBy(TimeSpan.FromSeconds(1));

            Assert.AreSame(expected, actual);
            Assert.AreEqual(expected, actual);
        }
    }
}

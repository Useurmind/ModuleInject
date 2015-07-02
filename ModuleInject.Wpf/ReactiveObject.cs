using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Text;

namespace ModuleInject.Wpf
{
    public interface IViewModel : INotifyPropertyChanging, INotifyPropertyChanged
    {

    }

    public class PropertyChangedMessage
    {
        public object Sender { get; private set; }
        public string PropertyName { get; private set; }

        public PropertyChangedMessage(IReactiveObject sender, string propertyName)
        {
            Sender = sender;
            PropertyName = propertyName;
        }
    }

    public class PropertyChangingMessage
    {
        public object Sender { get; private set; }
        public string PropertyName { get; private set; }

        public PropertyChangingMessage(IReactiveObject sender, string propertyName)
        {
            Sender = sender;
            PropertyName = propertyName;
        }
    }

    public interface IReactiveObject : IViewModel
    {
        IObservable<PropertyChangingMessage> PropertyChangingObs { get; }
        IObservable<PropertyChangedMessage> PropertyChangedObs { get; }
    }

    public class ReactiveObject : IReactiveObject
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        private ISubject<PropertyChangedMessage> propertyChangedSubject;
        private ISubject<PropertyChangingMessage> propertyChangingSubject;

        public IObservable<PropertyChangingMessage> PropertyChangingObs
        {
            get
            {
                if (propertyChangingSubject == null)
                {
                    propertyChangingSubject = new Subject<PropertyChangingMessage>();
                }
                return propertyChangingSubject;
            }
        }

        public IObservable<PropertyChangedMessage> PropertyChangedObs
        {
            get
            {
                if (propertyChangedSubject == null)
                {
                    propertyChangedSubject = new Subject<PropertyChangedMessage>();
                }
                return propertyChangedSubject;
            }
        }

        protected void OnPropertyChanging(string propertyName)
        {
            var sender = this;
            if (PropertyChanging != null)
            {
                PropertyChanging(sender, new PropertyChangingEventArgs(propertyName));
            }
            if (propertyChangingSubject != null)
            {
                propertyChangingSubject.OnNext(new PropertyChangingMessage(sender, propertyName));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var sender = this;
            if (PropertyChanged != null)
            {
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
            }
            if (propertyChangedSubject != null)
            {
                propertyChangedSubject.OnNext(new PropertyChangedMessage(sender, propertyName));
            }
        }

        protected void SetProperty<TProperty>(ref TProperty backingField, TProperty newValue, [CallerMemberName]string propertyName = null)
        {
            OnPropertyChanging(propertyName);

            backingField = newValue;

            OnPropertyChanged(propertyName);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Caliburn.Light
{
    internal sealed class WeakEventList
    {
        private readonly object _staticTarget = new object();
        private readonly List<WeakEventListEntry> _list = new List<WeakEventListEntry>();
        private readonly ConditionalWeakTable<object, object> _cwt = new ConditionalWeakTable<object, object>();

        public void AddHandler(Delegate handler)
        {
            var target = handler.Target ?? _staticTarget;

            // optimize weak event handler case
            if (target is IWeakEventHandler)
                target = _staticTarget;

            // add a record to the main list
            _list.Add(new WeakEventListEntry(target, handler));

            // add the handler to the CWT - this keeps the handler alive throughout
            // the lifetime of the target, without prolonging the lifetime of
            // the target
            if (!_cwt.TryGetValue(target, out object value))
            {
                // 99% case - the target only listens once
                _cwt.Add(target, handler);
            }
            else
            {
                // 1% case - the target listens multiple times
                // we store the delegates in a list
                if (value is not List<Delegate> list)
                {
                    // lazily allocate the list, and add the old handler
                    var oldHandler = value as Delegate;
                    list = new List<Delegate>
                    {
                        oldHandler
                    };

                    // install the list as the CWT value
                    _cwt.Remove(target);
                    _cwt.Add(target, list);
                }

                // add the new handler to the list
                list.Add(handler);
            }
        }

        public void RemoveHandler(Delegate handler)
        {
            var target = handler.Target ?? _staticTarget;

            // optimize weak event handler case
            if (target is IWeakEventHandler)
                target = _staticTarget;

            // remove the record from the main list
            for (var i = _list.Count - 1; i >= 0; --i)
            {
                if (_list[i].Matches(target, handler))
                {
                    _list.RemoveAt(i);
                    break;
                }
            }

            // remove the handler from the CWT
            if (_cwt.TryGetValue(target, out object value))
            {
                if (value is not List<Delegate> list)
                {
                    // 99% case - the target is removing its single handler
                    _cwt.Remove(target);
                }
                else
                {
                    // 1% case - the target had multiple handlers, and is removing one
                    list.Remove(handler);
                    if (list.Count == 0)
                    {
                        _cwt.Remove(target);
                    }
                }
            }
        }

        public bool Purge()
        {
            return _list.RemoveAll(l => l.IsDead) > 0;
        }

        public IReadOnlyList<TDelegate> GetHandlers<TDelegate>()
            where TDelegate : Delegate
        {
            // optimize for no handlers case
            if (_list.Count == 0)
                return Array.Empty<TDelegate>();

            var handlers = new List<TDelegate>(_list.Count);

            for (var i = 0; i < _list.Count; i++)
            {
                var entry = _list[i];

                if (!entry.IsDead && entry.Handler is TDelegate handler)
                    handlers.Add(handler);
            }

            if (_list.Count != handlers.Count)
            {
                _list.RemoveAll(l => l.IsDead);
            }

            return handlers;
        }

        private readonly struct WeakEventListEntry
        {
            private readonly WeakReference _target;
            private readonly WeakReference _handler;

            public WeakEventListEntry(object target, Delegate handler)
            {
                _target = new WeakReference(target);
                _handler = new WeakReference(handler);
            }

            public bool Matches(object target, Delegate handler)
            {
                return ReferenceEquals(target, _target.Target) && Equals(handler, _handler.Target);
            }

            public bool IsDead
            {
                get { return !_target.IsAlive; }
            }

            public object Handler
            {
                get { return _handler.Target; }
            }
        }
    }
}

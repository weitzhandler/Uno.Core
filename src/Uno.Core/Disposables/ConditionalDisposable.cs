// ******************************************************************
// Copyright � 2015-2018 nventive inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Uno.Disposables
{
	/// <summary>
	/// A disposable that can call an action when a dependent object has been collected.
	/// </summary>
	public class ConditionalDisposable : IDisposable
	{
		private static ConditionalWeakTable<object, List<IDisposable>> _registrations = new ConditionalWeakTable<object, List<IDisposable>>();

		private readonly Action _action;
		private bool _disposed;
		private readonly WeakReference _conditionSource;

#if DEBUG
		private readonly WeakReference _target;
#endif

		/// <summary>
		/// Creates a <see cref="ConditionalDisposable"/> instance using 
		/// <paramref name="target"/> as a reference for its lifetime.
		/// </summary>
		/// <param name="action">The action to be executed when target has been collected</param>
		/// <param name="conditionSource">An optional secondary reference, used to avoid calling action if it has been collected</param>
		/// <param name="target">The instance to use to keep the disposable alive</param>
		public ConditionalDisposable(object target, Action action, WeakReference conditionSource = null)
		{
			_action = action;
			_conditionSource = conditionSource;

#if DEBUG
			_target = new WeakReference(target);
#endif

			var list = _registrations.GetValue(target, CreateList);

			lock (list)
			{
				// The _registrations member is used to associate the target instane to the
				// current disposable instance. This way, when the target instance gets collected
				// the ConditionalWeakTable will make the "list" instance collectable, as well as
				// its content, making this instance collectable.
				list.Add(this);
			}
		}

		private static List<IDisposable> CreateList(object key) => new List<IDisposable>();

		public void Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if(disposing)
			{
				GC.SuppressFinalize(this);
			}

			if (_conditionSource?.IsAlive ?? true)
			{
				lock (this)
				{
					if (!_disposed)
					{
						_disposed = true;

						_action();
					}
				}
			}
		}

		~ConditionalDisposable()
		{
			Dispose(false);
		}
	}

}

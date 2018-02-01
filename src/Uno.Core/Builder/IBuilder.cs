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
using System.Linq;
using System.Text;

namespace Uno.Builder
{
	/// <summary>
	/// Defines a builder
	/// </summary>
	public interface IBuilder
	{
		/// <summary>
		/// Appends a build action when the parent builder is built
		/// </summary>
		/// <param name="action"></param>
		void AppendBuild(Action action);

		/// <summary>
		/// Builds the current builder
		/// </summary>
		void Build();
	}

	/// <summary>
	/// Defines a builder typed with its owner
	/// </summary>
	/// <typeparam name="TOwner"></typeparam>
	public interface IBuilder<TOwner> : IBuilder
	{
		/// <summary>
		/// The owner instance of this builder
		/// </summary>
		TOwner Owner { get; }
	}
}

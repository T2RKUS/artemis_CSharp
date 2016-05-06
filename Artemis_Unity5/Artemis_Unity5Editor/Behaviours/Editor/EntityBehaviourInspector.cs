﻿#region File description

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityWorld.cs" company="GAMADU.COM">
//     Copyright © 2013 GAMADU.COM. All rights reserved.
//     Redistribution and use in source and binary forms, with or without modification, are
//     permitted provided that the following conditions are met:
//        1. Redistributions of source code must retain the above copyright notice, this list of
//           conditions and the following disclaimer.
//        2. Redistributions in binary form must reproduce the above copyright notice, this list
//           of conditions and the following disclaimer in the documentation and/or other materials
//           provided with the distribution.
//     THIS SOFTWARE IS PROVIDED BY GAMADU.COM 'AS IS' AND ANY EXPRESS OR IMPLIED
//     WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//     FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL GAMADU.COM OR
//     CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//     CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//     SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//     ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//     NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//     ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//     The views and conclusions contained in the software and documentation are those of the
//     authors and should not be interpreted as representing official policies, either expressed
//     or implied, of GAMADU.COM.
// </copyright>
// <summary>
//   The Entity World Class. Main interface of the Entity System.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#endregion File description

namespace Artemis_Unity5Editor
{
	#region Using statements

	using global::System.Reflection;
	using global::System;

	using UnityEditor;
	using UnityEngine;

	using Artemis.Interface;
	using Artemis;

	#endregion

	/// <summary>
	/// Entity Behaviour Inspector.
	/// </summary>
	[CanEditMultipleObjects]
	[CustomEditor(typeof(EntityBehaviour))]
	public class EntityBehaviourInspector : Editor
	{
		bool Initialized=false;

		public void InitializeDrawer()
		{
			if (!Initialized) {
				Initialized = true;
				TypeDrawer.Initialize ();
			}
		}

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			InitializeDrawer ();

			if (targets.Length == 1) {
				EntityBehaviour EntityBahaviourScript = (EntityBehaviour)target;
				if (GUILayout.Button ("Delete Entity")) {
					EntityBahaviourScript.Entity.Delete ();
				}
				EditorGUILayout.Space ();

				DrawEntity (EntityBahaviourScript.Entity);
			}
		}

		void DrawEntity(Entity Entity)
		{
			foreach (IComponent Component in Entity.Components) {
				EditorGUILayout.LabelField (Component.GetType ().Name);
				DrawComponent (Component);
			}
		}

		void DrawComponent(IComponent Component)
		{
			Type ComponentType = Component.GetType ();
			foreach (PropertyInfo propertyInfo in ComponentType.GetProperties()) {
				// Get name.
				string name = propertyInfo.Name;

				// Get value on the target instance.
				object value = propertyInfo.GetValue (Component, null);

				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField (name, GUILayout.MaxWidth (10));

				object nvalue = TypeDrawer.Draw (propertyInfo.PropertyType, value);
				propertyInfo.SetValue (Component, Convert.ChangeType (nvalue, propertyInfo.PropertyType), null);

				EditorGUILayout.EndHorizontal ();
			}
		}
	}
}


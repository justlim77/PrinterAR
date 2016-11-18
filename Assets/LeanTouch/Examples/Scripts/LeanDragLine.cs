using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
	// This script will draw a line between the start and current finger points
	public class LeanDragLine : LeanDragTrail
	{
		protected override void WritePositions(LineRenderer line, LeanFinger finger)
		{
			// Get start and current world position of finger
			var start = finger.GetStartWorldPosition(Distance);
			var end   = finger.GetWorldPosition(Distance);

			// Write positions
			line.SetVertexCount(2);

			line.SetPosition(0, start);
			line.SetPosition(1, end);
		}
	}
}
using UnityEngine;
using System.Collections;

public static class Extensions
{
	/// <summary>
	/// Sets the x position of the transform.
	/// </summary>
	/// <param name="x">The new x coordinate.</param>
	public static void SetPositionX(this Transform t, float x)
	{
		t.position = new Vector3(x, t.position.y, t.position.z);
	}

	/// <summary>
	/// Sets the y position of the transform.
	/// </summary>
	/// <param name="y">The new y coordinate.</param>
	public static void SetPositionY(this Transform t, float y)
	{
		t.position = new Vector3(t.position.x, y, t.position.z);
	}

	/// <summary>
	/// Sets the z position of the transform.
	/// </summary>
	/// <param name="z">The z coordinate.</param>
	public static void SetPositionZ(this Transform t, float z)
	{
		t.position = new Vector3(t.position.x, t.position.y, z);
	}

	/// <summary>
	/// Binds the transform's position to the defined area
	/// </summary>
	/// <param name="minX">The minimum x coordinate.</param>
	/// <param name="maxX">The maximum x coordinate.</param>
	/// <param name="minZ">The minimum z coordinate.</param>
	/// <param name="maxZ">The maximum z coordinate.</param>
	public static void BindToArea(this Transform t, float minX, float maxX, float minZ, float maxZ)
	{
		//make sure the transform's position stays within the bounds
		if (t.position.x < minX) t.SetPositionX(minX);
		if (t.position.x > maxX) t.SetPositionX(maxX);
		if (t.position.z < minZ) t.SetPositionZ(minZ);
		if (t.position.z > maxZ) t.SetPositionZ(maxZ);
	}

}

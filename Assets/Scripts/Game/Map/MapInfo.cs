using UnityEngine;
using System.Collections;

public class MapInfo
{
	public static float MinimumX = -50;
	public static float MaximumX = 50;
	public static float MinimumZ = -50;
	public static float MaximumZ = 50;
	public static float MinimumY = 1;
	public static float Buffer = 2;

	/// <summary>
	/// Returns a random point on the map
	/// </summary>
	/// <param name="minX">Minimum x value for the region</param>
	/// <param name="minZ">Minimum z value for the region</param>
	/// <param name="maxX">Maximum x value for the region</param>
	/// <param name="maxZ">Maximum z value for the region</param>
	/// <returns></returns>
	public static Vector3 GetRandomPointOnMap()
	{
		return new Vector3(Random.Range(MinimumX + Buffer, MaximumX - Buffer), MinimumY, Random.Range(MinimumZ + Buffer, MaximumZ - Buffer));
	}
}
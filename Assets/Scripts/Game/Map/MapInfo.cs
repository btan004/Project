using UnityEngine;
using System.Collections;

public class MapInfo
{
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
		

		float minimumX = MapSystemScript.instance.GetLevelBounds().left;
		float maximumX = MapSystemScript.instance.GetLevelBounds().right;
		float minimumZ = MapSystemScript.instance.GetLevelBounds().bottom;
		float maximumZ = MapSystemScript.instance.GetLevelBounds().top;

		NavMeshHit hit;

		Vector3 position = new Vector3(
			Random.Range(minimumX + Buffer, maximumX - Buffer), 
			MinimumY, 
			Random.Range(minimumZ + Buffer, maximumZ - Buffer));

		NavMesh.SamplePosition(position, out hit, 10, 1);

		return hit.position;
	}
}
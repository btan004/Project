using UnityEngine;
using System.Collections;

public interface IState
{
	void BeforeEnter ();
	void Execute();
}
using UnityEngine;
using System.Collections;

public abstract class State<T> : MonoBehaviour
{
	abstract public void BeforeEnter( T owner );
	abstract public void Action( T owner );
	abstract public void BeforeExit( T owner );
}
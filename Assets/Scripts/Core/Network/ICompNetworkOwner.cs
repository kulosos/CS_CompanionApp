using UnityEngine;
using System.Collections;

interface ICompNetworkOwner  {

	void setAsOwner();
	void allocateNewNetworkViewID(NetworkViewID newID);
}

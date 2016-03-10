//
// Copyright (C) Microsoft. All rights reserved.
//

using UnityEngine;
using UAudioTools;

public class AudioTester : MonoBehaviour
{
    [SerializeField]
    private GameObject testEmitter;
    [SerializeField]
    private string eventName;
    [SerializeField]
    private RoomSize updateRoom;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
            UAudioManager.Instance.SetAllRoomModels(this.updateRoom, true);
            UAudioManager.Instance.PlayEvent(this.eventName, this.testEmitter);
            //UAudioManager.Instance.PlayEvent("Test");
        }
	}
}
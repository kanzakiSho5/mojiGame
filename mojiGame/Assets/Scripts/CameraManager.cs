using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CameraManager : MonoBehaviour
{
	GameObject[] StageCamera;

	private void Start()
	{
		// Stage用のカメラの取得
		var ChildTransform = gameObject.GetComponentsInChildren<Transform>()
			.Where(c => gameObject != c.gameObject)
			.Where(c => "cm" != c.gameObject.name).ToArray();
		var gameObjects = from t in ChildTransform select t.gameObject;
		StageCamera = gameObjects.ToArray();
		print(StageCamera.Length);
		StageCameraAllOff();
        MoveStageCamera(3);

        SceneController.StartNextStageEvent += StageCameraAllOff;
        SceneController.StartStartEvent += MoveStartCamera;
	}

    private void MoveStartCamera()
    {
        MoveStageCamera(3);
    }

	public void StageCameraAllOff()
	{
		foreach (var sc in StageCamera)
		{
			sc.SetActive(false);
		}
	}

	public void MoveStageCamera(int stage)
	{
		StageCamera[stage].SetActive(true);
	}
}

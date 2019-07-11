using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrearType
{
	Score,
	WordLength
}

public enum SceneType
{
	Start,
	Game,
	NextStage,
	Pause,
	Clear
}

public class SceneController : MonoBehaviour
{
	[SerializeField]
	ClearTerm[] clearTerm;

	public SceneType CurrentScene { get; protected set; }

	GameManager gameMan;

	public void Start()
	{
		gameMan = GameManager.Instance;
		CurrentScene = SceneType.Start;
	}

	public ClearTerm CurrentClearTermByStage(int Stage)
	{
		return clearTerm[Stage];
	}

	public bool CheckClear()
	{

		int stage = gameMan.stage;
		switch (clearTerm[stage].type)
		{
			case CrearType.Score:
				if (gameMan.score >= clearTerm[stage].value)
				{
					CurrentScene = SceneType.Clear;
					return true;
				}
				break;
			case CrearType.WordLength:
				if (gameMan.createdWordLength >= clearTerm[stage].value)
				{
					CurrentScene = SceneType.Clear;
					return true;
				}
				break;
		}

		return false;
	}

	/// <summary>
	/// CurrentSceneをNextStageに変更する
	/// (Animationを再生する)
	/// </summary>
	public void MoveNextScene()
	{
		CurrentScene = SceneType.NextStage;
	}

}

[System.Serializable]
public class ClearTerm
{
	[SerializeField]
	public CrearType type;
	[SerializeField]
	public int value;
}
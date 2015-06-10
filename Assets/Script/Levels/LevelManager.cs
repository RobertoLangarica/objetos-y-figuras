using System.IO;
using UnityEngine;

/**
 * Singleton para control de niveles.
 * 
 * */
public class LevelManager  
{
	//Version para controlar cambios a los datos en el futuro
	public const int version = 0;

	protected static LevelManager _instance;
	
	//niveles guardados
	protected Levels data;
	//Ruta de los datos
	protected string path;

	public int maxLevel;

	/**
	 * LevelManager instance -> Read-Only 
	 */
	public static LevelManager instance
	{
		get{
			if(_instance == null)
			{
				_instance = new LevelManager();
			}
			
			return _instance;
		}
	}

	//Constructor
	LevelManager()
	{
		//path = Path.Combine(Application.dataPath,"Resources/Levels/levels.xml");

		Debug.Log("LM-> Loading levels.");
		TextAsset tempTxt = (TextAsset)Resources.Load ("Levels/levels");
		//Ya eixste el archivo y solo checamos la version
		data = Levels.LoadFromText(tempTxt.text);//Levels.Load(path);

		Level[] levels = data.levels;

		maxLevel = 3;

		foreach(Level lvl in levels)
		{
			maxLevel = lvl.difficulty > maxLevel ? lvl.difficulty:maxLevel;
		}
	}

	//Busca un nivel o lo cre en caso de que no exista
	public Level getLevel(string lvlName)
	{
		return data.getLevelByName(lvlName);
	}

	//niveles con dificultad especifica
	public Level[] getLevels(int difficulty)
	{
		return data.getLevelsFromDifficulty(difficulty);
	}

	//Todos los niveles
	public Level[] getAllLevels()
	{
		return data.levels;
	}

	//Obtiene el nombre de la imagen de referencia para el nivel indicado
	public string getImageReferenceNameFromLevel(string levelName)
	{
		return data.getLevelByName(levelName).referenceImage;
	}
}

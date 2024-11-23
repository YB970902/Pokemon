using System.Collections;
using UnityEngine.SceneManagement;

public class Initializer : MonoSingleton<Initializer>
{
    protected override void Init()
    {
        base.Init();
        StartCoroutine(CheckManagerProcess());
    }

    private IEnumerator CheckManagerProcess()
    {
        while (true)
        {
            yield return null;
            if (TileManager.IsInit && Locale.IsInit) break;
        }
        
        OnProcessEnd();
    }

    private void OnProcessEnd()
    {
        SceneManager.LoadScene("Main");
    }
}
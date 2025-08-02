using UnityEngine;

public interface IUIScene 
{
    IUIScene Init();

    IUIScene Enter();
    
    IUIScene Exit();
}

public interface IUIModule 
{
    IUIModule Init(IUIScene scene);
    
    IUIModule Enter(IUIScene scene);

    IUIModule Exit(IUIScene scene);
}
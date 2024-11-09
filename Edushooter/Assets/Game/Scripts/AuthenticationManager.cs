using MoreMountains.Tools;
using UnityEngine;
using Firebase.Auth;

public class AuthenticationManager : MMPersistentSingleton<AuthenticationManager>
{

    protected const string _saveFolderName = "EdugameProgressData";
    protected const string _saveFileName = "EdushooterAuthentication.data";

    protected FirebaseAuth _auth;

    private void Start()
    {
        //initialize the firebase auth
        
    }

    public virtual void SaveGame()
    {
        //save into file
        //MMSaveLoadManager.Save(PlayerData, _saveFileName, _saveFolderName);
        Debug.Log("game saved");
    }
}

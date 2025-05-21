using Firebase;
using Firebase.Auth;
using System;
using System.Collections;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseUser user;

    string userId = null;
    string displayName = null;

    public string UserId
    {
        get
        {
            return userId;
        }
    }

    public string DisplayName
    {
        get
        {
            return displayName;
        }
    }

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Login(string userName, string password, Action<bool, string> onComplete)
    {
        StartCoroutine(LoginNow(userName, password, onComplete));
    }

    IEnumerator LoginNow(string userName, string password, Action<bool, string> onComplete)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(userName, password);

        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            string message = "Login Failed!";

            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
                default:
                    message = "Password not accepted";
                    Debug.LogError(loginTask.Exception.Message);

                    break;
            }

            onComplete?.Invoke(false, message);
        }
        else
        {
            var user = loginTask.Result;

            userId = user.User.UserId;
            displayName = user.User.DisplayName;
            onComplete?.Invoke(true, "OK");
        }
    }

    public void Register(string userName, string password, Action<bool, string> onComplete)
    {
        StartCoroutine(RegisterNow(userName, password, onComplete));
    }

    IEnumerator RegisterNow(string userName, string password, Action<bool, string> onComplete)
    {
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(userName, password);

        yield return new WaitUntil(predicate: () => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            FirebaseException firebaseEx = registerTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Registration Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WeakPassword:
                    message = "Weak Password";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "Email Already In Use";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                default:
                    message = "Server side error";
                    Debug.LogError(registerTask.Exception.Message);

                    break;
            }

            onComplete?.Invoke(false, message);
        }
        else
        {
            user = registerTask.Result.User;
            userId = user.UserId;
            displayName = user.DisplayName;
            onComplete?.Invoke(true, "OK");
        }
    }

    public void ResetPassword(string userName, Action<bool, string> onComplete)
    {
        StartCoroutine(ResetPasswordNow(userName, onComplete));
    }

    IEnumerator ResetPasswordNow(string userName, Action<bool, string> onComplete)
    {
        var authTask = auth.SendPasswordResetEmailAsync(userName);

        yield return new WaitUntil(predicate: () => authTask.IsCompleted);

        if (authTask.Exception != null)
        {
            FirebaseException firebaseEx = authTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Registration Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                default:
                    message = "Server side error";
                    Debug.LogError(authTask.Exception.Message);

                    break;
            }

            onComplete?.Invoke(false, message);
        }
        else
        {
            onComplete?.Invoke(true, "OK");
        }
    }
}

using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public RectTransform WindowLogin;
    public RectTransform WindowRegister;
    public RectTransform WindowError;
    public RectTransform WindowSuccess;
    public RectTransform WindowLoading;

    public FirebaseManager FBManager;

    void Start()
    {
        DisplayLoginWindow();
    }

    private void HideAllWindows()
    {
        HideRegisterWindow();
        HideLoginWindow();
        HideErrorBox();
        HideSuccessBox();
        HideLoadingWindow();
    }

    public void DisplayRegisterWindow()
    {
        HideAllWindows();
        WindowRegister.gameObject.SetActive(true);
    }
    public void DisplayLoginWindow()
    {
        HideAllWindows();
        WindowLogin.gameObject.SetActive(true);
    }
    public void DisplayError(string message)
    {
        TextMeshProUGUI txtMessage = WindowError.Find("Message").GetComponent<TextMeshProUGUI>();
        txtMessage.text = message.Trim();

        WindowError.SetAsLastSibling();
        WindowError.gameObject.SetActive(true);
    }
    public void DisplayLoadingWindow()
    {
        WindowError.SetAsLastSibling();
        WindowLoading.gameObject.SetActive(true);
    }
    public void DisplaySuccess(string message)
    {
        HideAllWindows();
        WindowSuccess.gameObject.SetActive(true);
    }

    public void HideRegisterWindow()
    {
        WindowRegister.gameObject.SetActive(false);
    }
    public void HideLoginWindow()
    {
        WindowLogin.gameObject.SetActive(false);
    }
    public void HideErrorBox()
    {
        WindowError.gameObject.SetActive(false);
    }
    public void HideSuccessBox()
    {
        WindowSuccess.gameObject.SetActive(false);
    }
    public void HideLoadingWindow()
    {
        WindowLoading.gameObject.SetActive(false);
    }

    public void AttemptToLogin()
    {
        TMP_InputField txtUserName = WindowLogin.Find("InputUserName/Input").GetComponent<TMP_InputField>();
        TMP_InputField txtPassword = WindowLogin.Find("InputPassword/Input").GetComponent<TMP_InputField>();

        // Validation
        if (string.IsNullOrEmpty(txtUserName.text.Trim()))
        {
            DisplayError("Please do not leave 'user name' field blank");
            return;
        }

        if (string.IsNullOrEmpty(txtPassword.text.Trim()))
        {
            DisplayError("Please do not leave 'password' field blank");
            return;
        }

        // Progress
        DisplayLoadingWindow();

        FBManager.Login(txtUserName.text.Trim(), txtPassword.text.Trim(), (result, message) => {
            HideLoadingWindow();

            if (result)
            {
                DisplaySuccess("Login successful");
                Debug.Log("User id: " + FBManager.UserId);
                Debug.Log("Display name: " + FBManager.DisplayName);
            }
            else
            {
                DisplayError("Login failed (reason: " + message + ")");
            }
        });
    }

    public void AttemptToRegister()
    {
        TMP_InputField txtUserName = WindowRegister.Find("InputUserName/Input").GetComponent<TMP_InputField>();
        TMP_InputField txtPassword = WindowRegister.Find("InputPassword/Input").GetComponent<TMP_InputField>();
        TMP_InputField txtPasswordRetype = WindowRegister.Find("InputPasswordRetype/Input").GetComponent<TMP_InputField>();

        // Validation
        if (string.IsNullOrEmpty(txtUserName.text.Trim()))
        {
            DisplayError("Please do not leave 'user name' field blank");
            return;
        }

        if (string.IsNullOrEmpty(txtPassword.text.Trim()))
        {
            DisplayError("Please do not leave 'password' field blank");
            return;
        }

        if (string.IsNullOrEmpty(txtPasswordRetype.text.Trim()))
        {
            DisplayError("Please do not leave 'password retype' field blank");
            return;
        }

        if (txtPassword.text.Trim() != txtPasswordRetype.text.Trim())
        {
            DisplayError("Password and its retype should be the same");
            return;
        }

        if (txtPassword.text.Trim().Length < 8)
        {
            DisplayError("Password has to be at least 8 characters long");
            return;
        }

        // Progress
        DisplayLoadingWindow();

        FBManager.Register(txtUserName.text.Trim(), txtPassword.text.Trim(), (result, message) => {
            HideLoadingWindow();

            if (result)
            {
                DisplaySuccess("Registration successful");
                Debug.Log("User id: " + FBManager.UserId);
                Debug.Log("Display name: " + FBManager.DisplayName);
            }
            else
            {
                DisplayError("Login failed (reason: " + message + ")");
            }
        });
    }
}

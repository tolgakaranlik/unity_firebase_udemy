using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public RectTransform WindowLogin;
    public RectTransform WindowRegister;
    public RectTransform WindowForgotPassword;
    public RectTransform WindowError;
    public RectTransform WindowSuccess;
    public RectTransform WindowLoading;

    public FirebaseManager FBManager;

    void Start()
    {
        DisplayLoginWindow();
    }

    public void HideAllWindows()
    {
        WindowRegister.gameObject.SetActive(false);
        WindowLogin.gameObject.SetActive(false);
        WindowError.gameObject.SetActive(false);
        WindowSuccess.gameObject.SetActive(false);
        WindowLoading.gameObject.SetActive(false);
        WindowForgotPassword.gameObject.SetActive(false);
    }

    public void HideErrorWindow()
    {
        WindowError.gameObject.SetActive(false);
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

        TextMeshProUGUI txtMessage = WindowSuccess.Find("Message").GetComponent<TextMeshProUGUI>();
        txtMessage.text = message.Trim();
    }
    public void DisplayForgotPasswordWindow()
    {
        HideAllWindows();
        WindowForgotPassword.gameObject.SetActive(true);
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
            HideAllWindows();

            if (result)
            {
                DisplaySuccess("Login successful");
                Debug.Log("User id: " + FBManager.UserId);
                Debug.Log("Display name: " + FBManager.DisplayName);
            }
            else
            {
                DisplayLoginWindow();
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
            HideAllWindows();

            if (result)
            {
                DisplaySuccess("Registration successful");
                StartCoroutine(HideSuccessWindowIn(1));
                Debug.Log("User id: " + FBManager.UserId);
                Debug.Log("Display name: " + FBManager.DisplayName);
            }
            else
            {
                DisplayRegisterWindow();
                DisplayError("Registration failed (reason: " + message + ")");
            }
        });
    }

    public void ResetPassword()
    {
        TMP_InputField txtUserName = WindowForgotPassword.Find("InputUserName/Input").GetComponent<TMP_InputField>();

        // Validation
        if (string.IsNullOrEmpty(txtUserName.text.Trim()))
        {
            DisplayError("Please do not leave 'user name' field blank");
            return;
        }

        // Progress
        DisplayLoadingWindow();

        FBManager.ResetPassword(txtUserName.text.Trim(), (result, message) =>
        {
            HideAllWindows();

            if (result)
            {
                DisplaySuccess("Password reset was successful");
                StartCoroutine(HideSuccessWindowIn(1));
            }
            else
            {
                DisplayForgotPasswordWindow();
                DisplayError("Password reset failed (reason: " + message + ")");
            }
        });
    }

    IEnumerator HideSuccessWindowIn(float delay)
    {
        yield return new WaitForSeconds(delay);

        HideAllWindows();
        DisplayLoginWindow();
    }
}

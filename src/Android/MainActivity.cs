using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Security.Keystore;
using Avalonia.Android;
using de.openelp.regatta;
using de.openelp.regatta.Interfaces;
using Java.Security;
using Javax.Crypto;
using Javax.Crypto.Spec;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Text;

namespace de.openelp.luebeckregatta.android;

[Activity(
    Label = "de.openelp.regatta.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity
{
    private const string ConfigName = "de.openelp.lrv";
    private const string KeyAlias = "de.openelp.lrv.password_key";

    /// <summary>
    /// Legacy plaintext key used before encrypted storage was introduced.
    /// Read once for migration, then removed.
    /// </summary>
    private const string LegacyPasswordKey = "Password";

    /// <summary>
    /// Key used to store the AES/GCM-encrypted password (IV prepended, Base64-encoded).
    /// </summary>
    private const string EncryptedPasswordKey = "EncryptedPassword";

    /// <summary>GCM standard IV length in bytes.</summary>
    private const int GcmIvLengthBytes = 12;

    /// <summary>GCM authentication tag length in bits.</summary>
    private const int GcmTagLengthBits = 128;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
    }

    protected override void OnResume()
    {
        base.OnResume();

        var config = App.Services.GetRequiredService<IAppConfiguration>();
        var preferences = GetSharedPreferences(ConfigName, FileCreationMode.Private);

        if (preferences != null && preferences.Contains("WebApiBaseUrl"))
        {
            config.WebApiBaseUrl = preferences.GetString("WebApiBaseUrl", string.Empty) ?? string.Empty;
            config.SelectedEventId = preferences.GetInt("SelectedEvent", -1);
            config.UserName = preferences.GetString("UserName", string.Empty) ?? string.Empty;
            config.Password = LoadPassword(preferences);
        }
    }

    protected override void OnPause()
    {
        base.OnPause();

        var config = App.Services.GetRequiredService<IAppConfiguration>();
        var preferences = GetSharedPreferences(ConfigName, FileCreationMode.Private);

        if (preferences != null)
        {
            var editor = preferences.Edit()!
                .PutString("WebApiBaseUrl", config.WebApiBaseUrl)!
                .PutInt("SelectedEvent", config.SelectedEventId)!
                .PutString("UserName", config.UserName)!;

            SavePassword(editor, config.Password);

            editor.Apply();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    /// <summary>
    /// Reads the password from preferences. Migrates legacy plaintext value if present.
    /// Returns an empty string if the password cannot be decrypted.
    /// </summary>
    private string LoadPassword(ISharedPreferences preferences)
    {
        // Migrate legacy plaintext value to encrypted storage
        if (preferences.Contains(LegacyPasswordKey))
        {
            var plaintext = preferences.GetString(LegacyPasswordKey, string.Empty) ?? string.Empty;
            try
            {
                var encrypted = EncryptPassword(plaintext);
                preferences.Edit()!
                    .PutString(EncryptedPasswordKey, encrypted)!
                    .Remove(LegacyPasswordKey)!
                    .Apply();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Password migration failed: {ex.Message}");
            }

            // Return the in-memory plaintext directly; the encrypted form is now persisted
            return plaintext;
        }

        if (!preferences.Contains(EncryptedPasswordKey))
        {
            return string.Empty;
        }

        try
        {
            var encryptedBase64 = preferences.GetString(EncryptedPasswordKey, null);
            if (string.IsNullOrEmpty(encryptedBase64))
            {
                return string.Empty;
            }

            return DecryptPassword(encryptedBase64);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Password decryption failed: {ex.Message}");
            return string.Empty;
        }
    }

    /// <summary>
    /// Encrypts the password and writes it to the preference editor.
    /// Removes any legacy plaintext entry. Does nothing on error.
    /// </summary>
    private void SavePassword(ISharedPreferencesEditor editor, string password)
    {
        try
        {
            var encrypted = EncryptPassword(password);
            editor.PutString(EncryptedPasswordKey, encrypted);
            editor.Remove(LegacyPasswordKey);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Password encryption failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Returns (or creates) the AES-256 key stored in the Android Keystore under <see cref="KeyAlias"/>.
    /// </summary>
    private static IKey GetOrCreateSecretKey()
    {
        var keyStore = KeyStore.GetInstance("AndroidKeyStore")!;
        keyStore.Load(null);

        if (!keyStore.ContainsAlias(KeyAlias))
        {
            var keyGenerator = KeyGenerator.GetInstance(KeyProperties.KeyAlgorithmAes, "AndroidKeyStore")!;
            var keySpec = new KeyGenParameterSpec.Builder(KeyAlias,
                    KeyStorePurpose.Encrypt | KeyStorePurpose.Decrypt)
                .SetBlockModes(KeyProperties.BlockModeGcm)!
                .SetEncryptionPaddings(KeyProperties.EncryptionPaddingNone)!
                .SetKeySize(256)!
                .Build()!;
            keyGenerator.Init(keySpec);
            keyGenerator.GenerateKey();
        }

        return keyStore.GetKey(KeyAlias, null)!;
    }

    /// <summary>
    /// Encrypts <paramref name="password"/> with AES/GCM.
    /// The returned string is a Base64 representation of [12-byte IV | ciphertext+tag].
    /// </summary>
    private static string EncryptPassword(string password)
    {
        var key = GetOrCreateSecretKey();
        var cipher = Cipher.GetInstance("AES/GCM/NoPadding")!;
        cipher.Init(CipherMode.EncryptMode, key);

        var iv = cipher.GetIV()!;
        var ciphertext = cipher.DoFinal(Encoding.UTF8.GetBytes(password))!;

        var combined = new byte[iv.Length + ciphertext.Length];
        Buffer.BlockCopy(iv, 0, combined, 0, iv.Length);
        Buffer.BlockCopy(ciphertext, 0, combined, iv.Length, ciphertext.Length);

        return Convert.ToBase64String(combined);
    }

    /// <summary>
    /// Decrypts a Base64-encoded [IV | ciphertext+tag] blob produced by <see cref="EncryptPassword"/>.
    /// </summary>
    private static string DecryptPassword(string encryptedBase64)
    {
        var combined = Convert.FromBase64String(encryptedBase64);

        var iv = combined[..GcmIvLengthBytes];
        var ciphertext = combined[GcmIvLengthBytes..];

        var key = GetOrCreateSecretKey();
        var cipher = Cipher.GetInstance("AES/GCM/NoPadding")!;
        cipher.Init(CipherMode.DecryptMode, key, new GCMParameterSpec(GcmTagLengthBits, iv));

        var plaintext = cipher.DoFinal(ciphertext)!;
        return Encoding.UTF8.GetString(plaintext);
    }
}